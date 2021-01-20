using System;
using Caliburn.Micro;
using RMWPFUi.Helpers;
using System.Threading.Tasks;

namespace RMWPFUi.ViewModels
{
    public class LoginViewModel:Screen
    {
        private readonly IAPIHelper _apiHelper;
        private string _userName;
        private string _password;

        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(()=>UserName);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(()=>Password);
                NotifyOfPropertyChange(()=>CanLogInButton);
            }

        }

        public bool CanLogInButton
        {
            get
            {
                bool output = UserName?.Length > 0 && Password?.Length > 0;

                return output;
            }

        }

        public async Task LogInButton()
        {
            try
            {
                var result = await _apiHelper.Authenticate(UserName, Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
