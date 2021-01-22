using RMWPFUi.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RMWPFUi.Library.Api
{
    public class APIHelper : IAPIHelper
    {
        private readonly ILoggedInUserModel _loggedInUserModel;
        private HttpClient _apiClient;

        public APIHelper(ILoggedInUserModel loggedInUserModel)
        {
            _loggedInUserModel = loggedInUserModel;
            InitializeClient();
        }

        public HttpClient ApiClient
        {
            get => _apiClient;
        }
        

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(
                "application/json"));
        }

        public void LogOffUser()
        {
            _apiClient.DefaultRequestHeaders.Clear();
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
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

        public async Task GetLoggedInUserInfo(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(
                "application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");

            using (HttpResponseMessage res = await _apiClient.GetAsync("/api/user"))
            {
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUserModel.CreatedDate = result.CreatedDate;
                    _loggedInUserModel.EmailAddress = result.EmailAddress;
                    _loggedInUserModel.FirstName = result.FirstName;
                    _loggedInUserModel.LastName = result.LastName;
                    _loggedInUserModel.Id = result.Id;
                    _loggedInUserModel.Token = token;
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
        }

    }
}
