using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RMApi.Data;
using RMApi.Models;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserData _data;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext context,UserManager<IdentityUser> userManager,
            IUserData data,ILogger<UserController> logger)
        {
            _context = context;
            _userManager = userManager;
            _data = data;
            _logger = logger;
        }
        // GET: User/Details/5
        [HttpGet]
        public UserModel GetById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //old way -- RequestContext.Principal.Identity.GetUserId();
            //UserData data = new UserData(_configuration);

            return _data.GetUserById(userId).First();
        }

        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [HttpGet]
        [Route("admin/getallusers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            //var userStore = new UserStore<ApplicationUser>(_context);
            //var userManager = new UserManager<ApplicationUser>(userStore);

            var users = _context.Users.ToList();
            var userRoles = from ur in _context.UserRoles
                                                     join r in _context.Roles on ur.RoleId equals r.Id
                                                     select new {ur.UserId, ur.RoleId, r.Name};
            //var roles = _context.Roles.ToList();

            foreach (var user in users)
            {
                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(
                    key => key.RoleId, val => val.Name);

                //foreach (var role in user.Roles)
                //{
                //    u.Roles.Add(role.RoleId, roles.Where(x => x.Id == role.RoleId).First().Name);
                //}
                output.Add(u);
            }

            return output;
        }

        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [HttpGet]
        [Route("admin/getallroles")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = _context.Roles.ToDictionary(
                x => x.Id, x => x.Name);
            return roles;
        }

        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [HttpPost]
        [Route("admin/addrole")]
        public async Task AddRole(UserRolePairModel pairing)
        {
            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _data.GetUserById(userId).First();
            
            var user =await _userManager.FindByIdAsync(pairing.UserId);

            _logger.LogInformation("Admin {Admin} added user {User} to role {Role}", userId, user.Id, pairing.RoleName);

            await _userManager.AddToRoleAsync(user, pairing.RoleName);
        }

        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [HttpPost]
        [Route("admin/removerole")]
        public async Task RemoveRole(UserRolePairModel pairing)
        {
            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _data.GetUserById(userId).First();

            var user = await _userManager.FindByIdAsync(pairing.UserId);

            _logger.LogInformation("Admin {Admin} removed user {User} to role {Role}", userId, user.Id, pairing.RoleName);

            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
        }
    }
}
