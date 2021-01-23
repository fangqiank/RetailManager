using Caliburn.Micro;
using RMWPFUi.EventModels;
using RMWPFUi.Library.Api;
using RMWPFUi.Library.Models;

namespace RMWPFUi.ViewModels
{
    public class ShellViewModel:Conductor<object>,IHandle<LogOnEventModel>
    {
        private readonly IEventAggregator _events;
        private readonly SalesViewModel _salesVm;

        private readonly ILoggedInUserModel _loggedInUserModel;

        private readonly IAPIHelper _apiHelper;
        //private readonly SimpleContainer _simpleContainer;

        //public ShellViewModel(IEventAggregator events,SalesViewModel salesVM, SimpleContainer _simpleContainer)
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM,
            ILoggedInUserModel loggedInUserModel,IAPIHelper apiHelper)
        {
            _events = events;
            _salesVm = salesVM;
            _loggedInUserModel = loggedInUserModel;
            _apiHelper = apiHelper;
            // this._simpleContainer = _simpleContainer;
            _events.Subscribe(this);
            //ActivateItem(_simpleContainer.GetInstance<LoginViewModel>());
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public bool IsAccountVisible
        {
            get
            {
                bool output = string.IsNullOrWhiteSpace(_loggedInUserModel.Token) == false;

                return output;
            }
        }

        public void Handle(LogOnEventModel message)
        {
            ActivateItem(_salesVm);
            NotifyOfPropertyChange(()=>IsAccountVisible);
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void UserManagement()
        {
            ActivateItem(IoC.Get<UserViewModel>());
        }

        public void LogOut()
        {
            _loggedInUserModel.ResetUserModel();
            _apiHelper.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsAccountVisible);
        }
    }
}
