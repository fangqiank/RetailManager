using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TRMDataManager.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        // GET: User/Details/5
        [HttpGet]
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(userId).First();
        }

        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [HttpGet]
        [Route("admin/getalluasers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
           List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var users = userManager.Users.ToList();

                var roles = context.Roles.ToList();

                foreach (var user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email
                    };

                    foreach (var role in user.Roles)
                    {
                        u.Roles.Add(role.RoleId,roles.Where(x=>x.Id == role.RoleId).First().Name);
                    }
                    output.Add(u);
                }
            }
            return output;
        }

       
    }
}
