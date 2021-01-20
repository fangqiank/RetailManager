using Caliburn.Micro;

namespace RMWPFUi.ViewModels
{
    public class ShellViewModel:Conductor<object>
    {
        private readonly LoginViewModel _loginVM;

        public ShellViewModel(LoginViewModel loginVM)
        {
            _loginVM = loginVM;
            ActivateItem(_loginVM);
        }
    }
}
