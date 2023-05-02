using API.Entities;
using API.Persistanse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public ShoppingCartsController(DatabaseContext dababaseContext)
        {
            _databaseContext = dababaseContext;
        }


        [HttpPost]
        public async Task<ActionResult> AddToShoppingCart(int userid, int FoodSlotId)
        {
            var order = _databaseContext.Orders.FirstOrDefault(x => x.UserId == userid);
            if (!(order.OrderStatusId == 1 || order.OrderStatusId == 4))
            {
                var orderNew = new Order
                {
                    UserId = userid,
                    OrderStatusId = 1,
                    PaymentTypeId = 1,
                    TotalPrice = 0                    
                };
                await _databaseContext.AddAsync(order);
                var linescount = await _databaseContext.SaveChangesAsync();
            }
            var orderId = _databaseContext.Orders
                .FirstOrDefault(x => x.UserId == userid);
            var foodSlot = _databaseContext.FoodSlots
                .FirstOrDefault(x => x.Id == FoodSlotId);

            var shoppingCart = new ShoppingCart
            {
                OrderId = orderId.Id,
                FoodSlotId = FoodSlotId,
                Count = 1,
                FoodSlot = foodSlot,
                Order = orderId
            };

            await _databaseContext.AddAsync(shoppingCart);
            var _linescount = await _databaseContext.SaveChangesAsync();

            return Ok(shoppingCart);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetShoppingCarts()
        {
            return await _databaseContext.ShoppingCarts.ToListAsync();
        }
    }
}
