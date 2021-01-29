using FuelPOSToolkitDesktopUI.Library.API;
using FuelPOSToolkitDesktopUI.Library.Models;
using FuelPOSToolkitWPF.Core.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuelPOSToolkitWPF.Dialogs
{
    public class LoginDialogViewModel : BindableBase, IDialogAware
    {
        
        private readonly IAPIHelper _apiHelper;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _loggedInUser;
        private string _userName = "gary.macgregor@tsg-solutions.com";
        private string _password = "Lnfg%@0210";

        public string Title { get; } = "FuelPOS Toolkit Login";
        public string Username
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _canLogIn = false;
        public bool CanLogIn
        {
            get
            {
                if (Username.Length > 0 && Password.Length > 0)
                {
                    _canLogIn = true;
                }
                else
                {
                    _canLogIn = false;
                }
                return _canLogIn;
            }
            set
            {
                SetProperty(ref _canLogIn, value);
            }
        }

        public DelegateCommand LoginCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;

        public LoginDialogViewModel(IAPIHelper apiHelper, IRegionManager regionManager, IEventAggregator events,
            ILoggedInUserModel loggedInUser)
        {
            _apiHelper = apiHelper;
            _regionManager = regionManager;
            _events = events;
            _loggedInUser = loggedInUser;
            LoginCommand = new DelegateCommand(LogIn)
                .ObservesCanExecute(() => CanLogIn);
        }

        private async void LogIn()
        {
            try
            {
                var result = await _apiHelper.Authenticate(Username, Password);

                // Capture more info about the user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                RequestClose?.Invoke(new DialogResult());
            }
            catch (Exception)
            {
                // TODO: Handle this
                throw;
            }
        }

        protected virtual void CloseDialog(string parameter)
        {

        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            if (_loggedInUser.EmailAddress == null)
            {
                Environment.Exit(0);
            }
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
