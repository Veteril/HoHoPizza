using API.DTOs;
using API.Entities;
using API.Persistanse;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FoodSlotsController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public FoodSlotsController(DatabaseContext databaseContext, IMapper mapper, UserService userService)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddNewFoodSlot(AddFoodSlotDto foodDto)
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

            var foodSlot = _mapper.Map<FoodSlot>(foodDto);
            await _databaseContext.AddAsync(foodSlot);

            var linesCount = await _databaseContext.SaveChangesAsync();
            return Ok(linesCount == 1);
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteFoodSlot(int foodSlotId)
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

            var foodSlot = await _databaseContext.FoodSlots
                .Include(fs => fs.IngridientCompositions)
                .SingleOrDefaultAsync(fs => fs.Id == foodSlotId);
            if (foodSlot == null) { return NotFound(); }

            _databaseContext.RemoveRange(foodSlot.IngridientCompositions);
            _databaseContext.Remove(foodSlot);
            await _databaseContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("status/change")]
        [Authorize]
        public async Task<ActionResult> ConfirmOrDisableFoodSlot(int foodSlotId)
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

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
            else { return NotFound(); }
            return Ok();
        }
        
        [HttpPut("change")]
        [Authorize]
        public async Task<ActionResult> ChangeFoodSlot(ChangeFoodSlotDto foodSlotDto)
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

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
        [Authorize]
        public async Task<ActionResult> ChangeIngridientInFoodSlot(ChangeIngridientCompositionDto ingCmpDto)
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

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
        [Authorize]
        public async Task<ActionResult> AddNewIngridientToFoodSlot(AddIngridientCompositionDto ingCmpDto)
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

            var ingCmp = _mapper.Map<IngridientComposition>(ingCmpDto);
            var foodSlot = await _databaseContext.FoodSlots
                .FirstOrDefaultAsync(x => x.Id == ingCmp.FoodSlotId);
            await _databaseContext.AddAsync(ingCmp);
            foodSlot.TotalWeight += ingCmp.Weight;
            var linesCount = await _databaseContext.SaveChangesAsync();
            return Ok(linesCount == 2);
        }

        [HttpDelete("ingridients")]
        [Authorize]
        public async Task<ActionResult> DeleteIngridientFromFoodSlot(int ingCmpId)
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

            var ingCmp = await _databaseContext.IngridientCompositions.FindAsync(ingCmpId);
            _databaseContext.IngridientCompositions.Remove(ingCmp);
            await _databaseContext.SaveChangesAsync();
            return Ok();
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
