using API.DTOs;
using API.Entities;
using API.Interfaces;
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
        private readonly ITokenService _tokenService;

        public UsersController(DatabaseContext databaseContext, ITokenService tokenService)
        {
            _databaseContext = databaseContext;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return await _databaseContext.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            return await _databaseContext.Users.FindAsync(id);
        }
    }
}
