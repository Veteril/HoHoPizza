using API.Entities;
using API.Persistanse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodSlotsController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public FoodSlotsController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpPost]
        public async Task<ActionResult> Add()
        {
            var food = new FoodSlot
            {
                ImageUrl = "FGHJKJHGHJ",
                Name = "Margarita",
                Price = 0,
                Weight = 0,
                
            };
            await _databaseContext.AddAsync(food);
            await _databaseContext.SaveChangesAsync();
            return Ok(food);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodSlot>>> GetOrders()
        {
            return await _databaseContext.FoodSlots.ToListAsync();
        }

    }
}
