using API.Entities;
using API.Persistanse;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;

        public ShoppingCartsController(DatabaseContext dababaseContext, UserService userService)
        {
            _databaseContext = dababaseContext;
            _userService = userService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddToShoppingCart(int FoodSlotId, int count = 1)
        {
            var userId = _userService.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
            if (userId == 0)
                return BadRequest("Cannot read token");

            var order = _databaseContext.Orders
                .Where(x => x.UserId == userId)
                .FirstOrDefault(o => o.OrderStatusId == 1 || o.OrderStatusId == 4);

            if (order == null)
            {
                var orderNew = new Order
                {
                    UserId = userId,
                    OrderStatusId = 1,
                    PaymentTypeId = 1,
                    TotalPrice = 0                    
                };
                await _databaseContext.AddAsync(orderNew);
                var linescount = await _databaseContext.SaveChangesAsync();
            }
            var orderId = _databaseContext.Orders
                .Include(o => o.ShoppingCarts)
                .Where(x => x.UserId == userId)
                .FirstOrDefault(o => o.OrderStatusId == 1 || o.OrderStatusId == 4);

            var foodSlot = _databaseContext.FoodSlots
                .FirstOrDefault(x => x.Id == FoodSlotId);

            foreach (var shoppingCarts in orderId.ShoppingCarts)
            {
                if(shoppingCarts != null && shoppingCarts.FoodSlotId == foodSlot.Id) 
                {
                    shoppingCarts.Count += count;
                    await _databaseContext.SaveChangesAsync();
                    return Ok();
                }
            }
            
            var shoppingCart = new ShoppingCart
            {
                OrderId = orderId.Id,
                FoodSlotId = FoodSlotId,
                Count = count,
                FoodSlot = foodSlot,
                Order = orderId
            };

            await _databaseContext.AddAsync(shoppingCart);
            var _linescount = await _databaseContext.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteShoppingCartFromOrder(int shoppingCartId)
        {
            var shoppingcart = await _databaseContext.ShoppingCarts.FindAsync(shoppingCartId);
            if (shoppingcart == null) { return NotFound(); }

            var orderId = shoppingcart.OrderId;

            _databaseContext.ShoppingCarts.Remove(shoppingcart);
            await _databaseContext.SaveChangesAsync();

            var order = await _databaseContext.Orders
                .Include(o => o.ShoppingCarts)
                .SingleOrDefaultAsync(x => x.Id == orderId);

            if (!order.ShoppingCarts.Any())
                _databaseContext.Orders.Remove(order);
            await _databaseContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetShoppingCarts()
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");
            return await _databaseContext.ShoppingCarts.ToListAsync();
        }
    }
}
