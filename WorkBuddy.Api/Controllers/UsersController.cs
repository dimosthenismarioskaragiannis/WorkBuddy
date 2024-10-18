using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkBuddy.Api.Data;
using WorkBuddy.Api.Entities;

namespace WorkBuddy.Api.Controllers
{
    
    
    public class UsersController(DataContext context): BaseApiController
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {

            var users = await context.Users.ToListAsync();

          

            return users;
        }

        [Authorize]
        [HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> GetUsers(string username)
        {

            var user = await context.Users.SingleOrDefaultAsync(x=>x. UserName== username);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }




    }
}
