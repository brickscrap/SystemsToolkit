using FuelPOSToolkitDesktopUI.Library.Models;
using FuelPOSToolkitWPF.Core.Events;
using FuelPOSToolkitWPF.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace FuelPOSToolkitWPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        LoggedInEvent _loggedInEvent;
        SubscriptionToken _token;

        private string _title = "FuelPOS Toolkit";
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _events;
        private readonly ILoggedInUserModel _loggedInUser;
        private readonly IDialogService _dialogService;

        public DelegateCommand ExitCommand { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator events, ILoggedInUserModel loggedInUser,
            IDialogService dialogService)
        {
            _regionManager = regionManager;
            _events = events;
            _loggedInUser = loggedInUser;
            _dialogService = dialogService;

            _loggedInEvent = _events.GetEvent<LoggedInEvent>();
            _token = _loggedInEvent.Subscribe(OnLoggedIn);

            ExitCommand = new DelegateCommand(Exit);

            if (_loggedInUser.EmailAddress == null)
            {
                _dialogService.ShowDialog("LoginDialog");
            }
        }

        private void OnLoggedIn()
        {

        }

        private void Exit()
        {
            Environment.Exit(0);
        }
    }
}
