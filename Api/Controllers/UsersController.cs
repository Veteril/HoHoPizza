using API.DTOs;
using API.Entities;
using API.Persistanse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;

        public UsersController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return await _databaseContext.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        
        public async Task<ActionResult<User>> GetUser(int id)
        {
            return await _databaseContext.Users.FindAsync(id);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(UserDto userDto)
        {
            if (await _databaseContext.Users.AnyAsync(x => x.UserName == userDto.UserName.ToLower()))
                return BadRequest("This username is already taken");

            using var hmac = new HMACSHA512();

            var user = new User()
            {
                UserName = userDto.UserName.ToLower(),
                Password = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF32.GetBytes(userDto.Password))),
                IsAdmin = false,
                Money = 0,
            };

            _databaseContext.Users.Add(user);
            await _databaseContext.SaveChangesAsync();

            return Ok(user);
        }

    }
}
