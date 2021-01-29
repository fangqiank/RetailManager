using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMWPFUi.EventModels;
using RMWPFUi.Library.Api;
using RMWPFUi.Library.Models;

namespace RMWPFUi.ViewModels
{
    public class ShellViewModel:Conductor<object>,IHandle<LogOnEventModel>
    {
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _loggedInUserModel;
        private readonly IAPIHelper _apiHelper;
        //private readonly SimpleContainer _simpleContainer;

        //public ShellViewModel(IEventAggregator events,SalesViewModel salesVM, SimpleContainer _simpleContainer)
        public ShellViewModel(IEventAggregator events, ILoggedInUserModel loggedInUserModel,IAPIHelper apiHelper)
        {
            _events = events;
            _loggedInUserModel = loggedInUserModel;
            _apiHelper = apiHelper;
            // this._simpleContainer = _simpleContainer;
            _events.SubscribeOnPublishedThread(this);
            //ActivateItem(_simpleContainer.GetInstance<LoginViewModel>());
            ActivateItemAsync(IoC.Get<LoginViewModel>(),new CancellationToken());
        }


        public bool IsLoggedIn
        {
            get
            {
                bool output = string.IsNullOrWhiteSpace(_loggedInUserModel.Token) == false;

                return output;
            }
        }


        public bool IsLoggedOut => !IsLoggedIn;


        public bool IsAccountVisible
        {
            get
            {
                bool output = string.IsNullOrWhiteSpace(_loggedInUserModel.Token) == false;

                return output;
            }
        }

        //public void Handle(LogOnEventModel message)
        //{
        //    ActivateItem(_salesVm);
        //    NotifyOfPropertyChange(()=>IsAccountVisible);
        //}

        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public async Task UserManagement()
        {
           await ActivateItemAsync(IoC.Get<UserViewModel>(),new CancellationToken());
        }

        public async Task LogOut()
        {
            _loggedInUserModel.ResetUserModel();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(),new CancellationToken());
            NotifyOfPropertyChange(() => IsAccountVisible);
        }

        public async Task HandleAsync(LogOnEventModel message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(),cancellationToken);
            NotifyOfPropertyChange(()=> IsAccountVisible);
        }
    }
}
