using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RMWPFUi.Models;

namespace RMWPFUi.Helpers
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient _apiClient;

        public APIHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            _apiClient=new HttpClient();
            _apiClient.BaseAddress=new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(
                "application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data =  new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type","password"),
                new KeyValuePair<string, string>("username",username),
                new KeyValuePair<string, string>("password",password), 
            });

            using (HttpResponseMessage res = await _apiClient.PostAsync("/token", data))
            {
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }

            }
        }
    }
};
