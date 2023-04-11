using API.Entities;
using API.Persistanse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
