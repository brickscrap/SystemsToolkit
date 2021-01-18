using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuelPOSToolkitWPF.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private string _username;

        public IMvxCommand LoginCommand { get; set; }

        public string Username
        {
            get { return _username; }
            set 
            {
                SetProperty(ref _username, value);
                RaisePropertyChanged(() => CanLogIn);
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set 
            {
                SetProperty(ref _password, value);
                RaisePropertyChanged(() => CanLogIn);
            }
        }

        public LoginViewModel()
        {
            LoginCommand = new MvxCommand(LogIn);
        }

        public bool CanLogIn => Username?.Length > 0 && Password?.Length > 0;

        public void LogIn()
        {
            // TODO: Talk to API to login
            // Load main window
        }
    }
}
