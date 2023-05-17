using API.Entities;
using API.Persistanse;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class IngridientsController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public IngridientsController(DatabaseContext databaseContext, IMapper mapper, UserService userService)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllIngridients()
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

            var ing = await _databaseContext.Ingridients
                .Select(x => x.Name)
                .ToListAsync();

            return Ok(ing);
        }

        
        [HttpPost]
        public async Task<ActionResult> AddNewIngridient(string name)
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

            var ingridient = new Ingridient { Name = name };
            await _databaseContext.Ingridients.AddAsync(ingridient);
            await _databaseContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteIngridient(int ingId)
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

            var ingridient = await _databaseContext.Ingridients.FindAsync(ingId);
            if (ingridient == null) return NotFound();
            _databaseContext.Ingridients.Remove(ingridient);
            await _databaseContext.SaveChangesAsync();
            return Ok();
        }
    }
}
