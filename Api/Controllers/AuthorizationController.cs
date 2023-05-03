using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Persistanse;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly DatabaseContext _databaseContext;

        public AuthorizationController(ITokenService tokenService, DatabaseContext databaseContext)
        {
            _tokenService = tokenService;
            _databaseContext = databaseContext;
        }

        [HttpPost("signin")]
        public async Task<ActionResult<UserDtoToken>> SignInUser(UserDtoRegister userDto)
        {
            var user = await _databaseContext.Users.SingleOrDefaultAsync(x => x.UserName == userDto.UserName);
            if (user == null) return Unauthorized();

            using var hmac = new HMACSHA512(Convert.FromBase64String(user.PasswordSalt));
            var computeHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password)));
            if (computeHash != user.Password)
                return Unauthorized();

            return new UserDtoToken
            {
                UserName = user.UserName,
                Token = _tokenService.GetToken(user)
            };
        }

        [HttpPost("signup")]
        public async Task<ActionResult<UserDtoToken>> SignUpUser(UserDtoRegister userDto)
        {
            if (await _databaseContext.Users.AnyAsync(x => x.UserName == userDto.UserName))
                return BadRequest("This username is already taken");

            using var hmac = new HMACSHA512();

            var user = new User()
            {
                UserName = userDto.UserName,
                Password = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password))),
                PasswordSalt = Convert.ToBase64String(hmac.Key),
                IsAdmin = false,
                Money = 0,
            };

            _databaseContext.Users.Add(user);
            await _databaseContext.SaveChangesAsync();

            return new UserDtoToken
            {
                UserName = user.UserName,
                Token = _tokenService.GetToken(user)
            };
        }
    }
}
