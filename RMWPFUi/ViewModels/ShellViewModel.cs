using Caliburn.Micro;
using RMWPFUi.EventModels;

namespace RMWPFUi.ViewModels
{
    public class ShellViewModel:Conductor<object>,IHandle<LogOnEventModel>
    {
        private readonly IEventAggregator _events;
        private readonly SalesViewModel _salesVm;

        public ShellViewModel(IEventAggregator events,SalesViewModel salesVM)
        {
            _events = events;
            _salesVm = salesVM;
            _events.Subscribe(this);
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEventModel message)
        {
            ActivateItem(_salesVm);
        }
    }
}
