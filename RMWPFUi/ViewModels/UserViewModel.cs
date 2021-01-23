using Caliburn.Micro;
using RMWPFUi.Library.Api;
using RMWPFUi.Library.Models;
using System;
using System.ComponentModel;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;

namespace RMWPFUi.ViewModels
{
    public class UserViewModel:Screen
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        private readonly IUserEndPoint _userEndPoint;
        private BindingList<UserModel> _users;

        public BindingList<UserModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyOfPropertyChange(()=>Users);
            }
        }

        public UserViewModel(StatusInfoViewModel status,IWindowManager window,IUserEndPoint userEndPoint)
        {
            _status = status;
            _window = window;
            _userEndPoint = userEndPoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await LoadUsers();
            }
            catch(Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";
            }

            
        }

        private async Task LoadUsers()
        {
            var userList = await _userEndPoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }
    }
}
