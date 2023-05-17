using API.DTOs;
using API.Entities;
using API.Persistanse;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Data;
using System.Security.Claims;
using System.Xml.Schema;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public OrdersController(DatabaseContext databaseContext, UserService userService, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");
            var orders = await _databaseContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderStatus)
                .Include(o => o.PaymentType)
                .Include(o => o.ShoppingCarts)
                .ThenInclude(sc => sc.FoodSlot)
                .Where(x => x.OrderStatusId == 2)
                .ToListAsync();

            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(ordersDto);
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrdersHisory()
        {
            var userId = _userService.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
            if (userId == 0)
                return BadRequest("Cannot read token");

            var orders = await _databaseContext.Orders
                .Include(o => o.PaymentType)
                .Include(o => o.ShoppingCarts)
                .ThenInclude(sc => sc.FoodSlot)
                .Where(x => x.UserId == userId)
                .Where(x => x.OrderStatusId == 3)
                .ToListAsync();

            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(ordersDto);
        }

        [HttpPut("status")]
        public async Task<ActionResult> ChangeOrderStatusAsReady(int orderId)
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");
            var order = _databaseContext.Orders.FirstOrDefault(x => x.Id == orderId);
            order.OrderStatusId = 3;
            await _databaseContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("checkout")]
        public async Task<ActionResult<OrderDto>> GetUnpaidOrderWithShoppingCart()
        {
            var userId = _userService.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
            if (userId == 0)
                return BadRequest("Cannot read token");

            var order = _databaseContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderStatus)
                .Include(o => o.PaymentType)
                .Include(o => o.ShoppingCarts)
                .ThenInclude(sc => sc.FoodSlot)
                .Where(x => x.UserId == userId)
                .FirstOrDefault(x => x.OrderStatusId == 1 || x.OrderStatusId == 4);

            if (order != null)
            {
                float totalPrice = 0;
                foreach (var shoppingCart in order.ShoppingCarts)
                {
                    totalPrice += shoppingCart.FoodSlot.Price * shoppingCart.Count;
                }
                order.TotalPrice = totalPrice;
                order.OrderStatusId = 4;
                await _databaseContext.SaveChangesAsync();
            }
            else  return Ok("Your shopping cart is empty");

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        [HttpPut("confirm")]
        public async Task<ActionResult> ConfirmOrder(int paymentType)
        {
            var userId = _userService.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
            if (userId == 0)
                return BadRequest("Cannot read token");

            var user = await _databaseContext.Users
                .Include(o => o.Orders)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                var order = user.Orders.FirstOrDefault(o => o.OrderStatusId == 4);
                if (order != null)
                {
                    if (user.Money < order.TotalPrice)
                        return BadRequest("Not enough money in your wallet");
                    user.Money -= order.TotalPrice;
                    order.PaymentTypeId = paymentType;
                    order.OrderStatusId = 2;
                    await _databaseContext.SaveChangesAsync();
                }
            }
            return Ok();
        }
    }
}
