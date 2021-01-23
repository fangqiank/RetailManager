using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RMWPFUi.Library.Models;

namespace RMWPFUi.Library.Api
{
    public class UserEndPoint : IUserEndPoint
    {
        private readonly IAPIHelper _apiHelper;

        public UserEndPoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<UserModel>> GetAll()
        {
            using (HttpResponseMessage res = await _apiHelper.ApiClient.GetAsync("/api/user/admin/getallusers"))
            {
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsAsync<List<UserModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
        }

        public async Task<Dictionary<string,string>> GetAllRoles()
        {
            using (HttpResponseMessage res = await _apiHelper.ApiClient.GetAsync("/api/user/admin/getallroles"))
            {
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsAsync<Dictionary<string,string>>();
                    return result;
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
        }

        public async Task AddUserToRole(string userId, string roleName)
        {
            var data = new {userId, roleName};

            using (HttpResponseMessage res = await _apiHelper.ApiClient.PostAsJsonAsync("/api/user/admin/addrole",data))
            {
                if (res.IsSuccessStatusCode == false)
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
        }

        public async Task RemoveUserToRole(string userId, string roleName)
        {
            var data = new { userId, roleName };

            using (HttpResponseMessage res = await _apiHelper.ApiClient.PostAsJsonAsync("/api/user/admin/removerole", data))
            {
                if (res.IsSuccessStatusCode == false)
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
        }

    }
}
