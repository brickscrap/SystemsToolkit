using FuelPOSToolkitWPF.Core.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Handlers;
using TSGSystemsToolkit.DesktopUI.Library.Models;

namespace FuelPOSToolkit.WPF.ViewModels
{
    public class StatusBarViewModel : BindableBase
    {
        private readonly ILoggedInUserModel _loggedInUser;
        private string _loggedInUserName;

        public string LoggedInUserName
        {
            get { return _loggedInUserName; }
            set { SetProperty(ref _loggedInUserName, value); }
        }

        public StatusBarViewModel(ILoggedInUserModel loggedInUser)
        {
            _loggedInUser = loggedInUser;

            LoggedInUserName = _loggedInUser.EmailAddress;
        }
    }
}
