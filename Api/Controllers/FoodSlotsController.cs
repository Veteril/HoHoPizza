using API.DTOs;
using API.Entities;
using API.Persistanse;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public FoodSlotsController(DatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewFoodSlot(AddFoodSlotDto foodDto)
        {
            var foodSlot = _mapper.Map<FoodSlot>(foodDto);
            await _databaseContext.AddAsync(foodSlot);

            var linesCount = await _databaseContext.SaveChangesAsync();
            return Ok(linesCount == 1);
        }

        [HttpPut("status/change")]
        public async Task<ActionResult> ConfirmOrDisableFoodSlot(int foodSlotId)
        {
            var foodSlot = await _databaseContext.FoodSlots.FindAsync(foodSlotId);
            if (foodSlot != null) 
            {
                if (!(foodSlot.IsActive))
                {
                    foodSlot.IsActive = true;
                    await _databaseContext.SaveChangesAsync();
                }
                else
                {
                    foodSlot.IsActive = false;
                    await _databaseContext.SaveChangesAsync();
                }
            }
            return Ok();
        }

        [HttpPut("change")]
        public async Task<ActionResult> ChangeFoodSlot(ChangeFoodSlotDto foodSlotDto)
        {
            var foodSlot = await _databaseContext.FoodSlots.FindAsync(foodSlotDto.Id);
            if (foodSlot != null) 
            {
                foodSlot.Price = foodSlotDto.Price;
                foodSlot.ImageUrl = foodSlotDto.ImageUrl;
                foodSlot.Name = foodSlotDto.Name;
                await _databaseContext.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut("ingridients/change")]
        public async Task<ActionResult> ChangeIngridientInFoodSlot(ChangeIngridientCompositionDto ingCmpDto)
        {
            var ingCmp = await _databaseContext.IngridientCompositions.FindAsync(ingCmpDto.Id);
            if (ingCmp != null)
            {
                var foodSlot = await _databaseContext.FoodSlots.FindAsync(ingCmpDto.FoodSlotId);
                if (ingCmp.Weight < ingCmpDto.Weight)
                    foodSlot.TotalWeight += ingCmpDto.Weight - ingCmp.Weight;
                else
                    foodSlot.TotalWeight -= ingCmp.Weight - ingCmpDto.Weight;

                ingCmp.Weight = ingCmpDto.Weight;
                ingCmp.IngridientId = ingCmpDto.IngridientId;

                await _databaseContext.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost("ingridients")]
        public async Task<ActionResult> AddNewIngridientToFoodSlot(AddIngridientCompositionDto ingCmpDto)
        {
            var ingCmp = _mapper.Map<IngridientComposition>(ingCmpDto);
            var foodSlot = await _databaseContext.FoodSlots
                .FirstOrDefaultAsync(x => x.Id == ingCmp.FoodSlotId);
            await _databaseContext.AddAsync(ingCmp);
            foodSlot.TotalWeight += ingCmp.Weight;
            var linesCount = await _databaseContext.SaveChangesAsync();
            return Ok(linesCount == 2);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetFoodSlotDto>>> GetAllFoodSlots()
        {
            var foodSlots = await _databaseContext.FoodSlots
                .Include(fs => fs.IngridientCompositions)
                .ThenInclude(ic => ic.Ingridient)
                .Where(fs => fs.IsActive == true)
                .ToListAsync();

            var foodSlotsDto = _mapper.Map<IEnumerable<GetFoodSlotDto>>(foodSlots);
            return Ok(foodSlotsDto);
        }
    }
}
