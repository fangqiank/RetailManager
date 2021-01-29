using Caliburn.Micro;
using RMWPFUi.Library.Api;
using RMWPFUi.Library.Models;
using System;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
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

        private UserModel _selectedUser;

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                SelectedUserName = value.Email;
                UserRoles = new BindingList<string>(value.Roles.Select(x=>x.Value).ToList());
                LoadRoles();
                NotifyOfPropertyChange(()=>SelectedUser);
            }
        }

        private string _selectedUserName;

        public string SelectedUserName
        {
            get => _selectedUserName;
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(()=>SelectedUserName);
            }
        }

        private BindingList<string> _userRoles =new BindingList<string>();

        public BindingList<string> UserRoles
        {
            get => _userRoles;
            set
            {
                _userRoles = value;
                NotifyOfPropertyChange(()=>UserRoles);
            }
        }

        private BindingList<string> _availableRoles = new BindingList<string>();

        public BindingList<string> AvailableRoles
        {
            get => _availableRoles;
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        private string _selectedUserRole;

        public string SelectedUserRole
        {
            get =>_selectedUserRole;
            set
            {
                _selectedUserRole = value;
                NotifyOfPropertyChange(()=>SelectedUserRole);
                NotifyOfPropertyChange(() => CanRemoveSelectedRole);
            }
        }

        private string _selectedAvailableRole;

        public string SelectedAvailableRole
        {
            get => _selectedAvailableRole;
            set
            {
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(()=> SelectedAvailableRole);
                NotifyOfPropertyChange(() => CanAddSelectedRole);
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

        private async Task LoadRoles()
        {
            var roles = await _userEndPoint.GetAllRoles();

            AvailableRoles.Clear();

            foreach (var role in roles)
            {
                if (UserRoles.IndexOf(role.Value) < 0)
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }

        public bool CanAddSelectedRole
        {
            get
            {
                if (SelectedUser is null || SelectedAvailableRole is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }

        public async void AddSelectedRole()
        {
            await _userEndPoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);

            UserRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);
        }

        public bool CanRemoveSelectedRole
        {
            get
            {
                if (SelectedUser is null || SelectedUserRole is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }

        public async void RemoveSelectedRole()
        {
            await _userEndPoint.RemoveUserToRole(SelectedUser.Id, SelectedUserRole);

            AvailableRoles.Add(SelectedUserRole);
            UserRoles.Remove(SelectedUserRole);
        }
    }
}
