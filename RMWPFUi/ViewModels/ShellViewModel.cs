using Caliburn.Micro;
using RMWPFUi.EventModels;

namespace RMWPFUi.ViewModels
{
    public class ShellViewModel:Conductor<object>,IHandle<LogOnEventModel>
    {
        private readonly IEventAggregator _events;
        private readonly SalesViewModel _salesVm;
        //private readonly SimpleContainer _simpleContainer;

        //public ShellViewModel(IEventAggregator events,SalesViewModel salesVM, SimpleContainer _simpleContainer)
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM)
        {
            _events = events;
            _salesVm = salesVM;
           // this._simpleContainer = _simpleContainer;
            _events.Subscribe(this);
            //ActivateItem(_simpleContainer.GetInstance<LoginViewModel>());
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEventModel message)
        {
            ActivateItem(_salesVm);
        }
    }
}
