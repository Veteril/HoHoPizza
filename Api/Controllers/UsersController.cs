using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Persistanse;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserService _userService;
        private readonly IMapper _mapper;


        public UsersController(DatabaseContext databaseContext, UserService userService, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            if (!(await _userService.CheckAdminStatus(HttpContext.User.Identity as ClaimsIdentity)))
                return Unauthorized("Cannot read token or you don`t have enough rights");

            var users = await _databaseContext.Users.ToListAsync();
            return Ok(_mapper.Map<UserDto>(users));
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userId = _userService.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
            if (userId == 0)
                return BadRequest("Cannot read token");

            var user = _databaseContext.Users.FirstOrDefault(x => x.Id == userId);
            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPut("refill")]
        public async Task<ActionResult> RefillWallet(float money)
        {
            var userId = _userService.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
            if (userId == 0)
                return BadRequest("Cannot read token");

            var user = _databaseContext.Users.FirstOrDefault(x => x.Id == userId);
            user.Money += money;

            await _databaseContext.SaveChangesAsync();
            return Ok();
        }
    }
}
