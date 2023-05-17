using API.Entities;
using API.Persistanse;
using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Principal;

namespace API.Services
{
    public class UserService
    {
        private readonly DatabaseContext _databaseContext;
        public UserService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public int GetUserId (ClaimsIdentity _identity) 
        {
            if (_identity == null)
            {
                return 0;
            }
            var userIdClaim = _identity.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (userIdClaim != null)
            {
                var userId = int.Parse(userIdClaim.Value);
                return userId;
            }
            return 0;
        }

        public string GetUsername(ClaimsIdentity _identity)
        {
            if (_identity == null)
            {
                return "";
            }
            var usernameClaim = _identity.Claims.FirstOrDefault(c => c.Type == "name");
            var username = usernameClaim.Value;
            if (usernameClaim != null) 
                return username;

            return "";
        }

        public async Task<bool> CheckAdminStatus(ClaimsIdentity _identity)
        {
            var userId = GetUserId(_identity);
            if (userId == 0) { return false; }

            var user = await _databaseContext.Users.FindAsync(userId);
            if (user.IsAdmin == true)
                return true;
            return false;

        }
    }
}
