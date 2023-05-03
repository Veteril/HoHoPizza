using API.Entities;
using API.Persistanse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Schema;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public OrdersController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _databaseContext.Orders.ToListAsync();
        }
        [HttpGet("checkout")]
        public async Task<ActionResult<Order>> GetUnpaidOrderWithShoppingCart(int userId)
        {
            var order = _databaseContext.Orders
                .Include(o => o.ShoppingCarts)
                .ThenInclude(sc => sc.FoodSlot)
                .Where(x => x.UserId == userId)
                .FirstOrDefault(x => x.OrderStatusId == 1);

            if (order != null)
            {
                float totalPrice = 0;
                foreach (var shoppingCart in order.ShoppingCarts)
                {
                    totalPrice += shoppingCart.FoodSlot.Price;
                }   
                order.TotalPrice = totalPrice;
                order.OrderStatusId = 4;
                await _databaseContext.SaveChangesAsync();
            }
            return Ok(new {Order = order, ShoppingCart = order.ShoppingCarts});
        }


        [HttpPut("confirm")]
        public async Task<ActionResult> ConfirmOrder(int userId)
        {
            var user = await _databaseContext.Users
                .Include(o => o.Orders)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var order = user.Orders.FirstOrDefault(o => o.OrderStatusId == 4);
                if (order != null)
                {
                    user.Money -= order.TotalPrice;
                    order.OrderStatusId = 2;
                    await _databaseContext.SaveChangesAsync();
                }
            }
            return Ok();
        }
    }
}
