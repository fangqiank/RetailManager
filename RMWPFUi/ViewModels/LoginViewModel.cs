﻿using System;
using System.Threading;
using Caliburn.Micro;
using RMWPFUi.Library.Api;
using System.Threading.Tasks;
using RMWPFUi.EventModels;

namespace RMWPFUi.ViewModels
{
    public class LoginViewModel:Screen
    {
        private readonly IAPIHelper _apiHelper;
        private readonly IEventAggregator _events;
        private string _userName = "zhangsan@mail.com";
        private string _password="G73gle73!";
        private string _errorMessage;

        public LoginViewModel(IAPIHelper apiHelper,IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public bool IsErrorVisible
        {
            get
            {
                bool output = ErrorMessage?.Length > 0;

                return output;
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage; 
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
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

                //capture more info about the user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                await _events.PublishOnUIThreadAsync(new LogOnEventModel(),new CancellationToken());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
