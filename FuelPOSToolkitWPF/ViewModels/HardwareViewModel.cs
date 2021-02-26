using FuelPOSToolkitDesktopUI.Library.API;
using FuelPOSToolkitDesktopUI.Library.Models;
using FuelPOSToolkitWPF.Core.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Handlers;

namespace FuelPOSToolkitWPF.ViewModels
{
    public class HardwareViewModel : BindableBase, INavigationAware
    {
        private readonly ILoggedInUserModel _loggedInUser;
        private readonly IStationEndpoint _stationEndpoint;
        private ObservableCollection<StationModel> _stations = new ObservableCollection<StationModel>();
        public ObservableCollection<StationModel> Stations
        {
            get { return _stations; }
            set { SetProperty(ref _stations, value); }
        }

        public HardwareViewModel(ILoggedInUserModel loggedInUser, IStationEndpoint stationEndpoint, IEventAggregator events)
        {
            _loggedInUser = loggedInUser;
            _stationEndpoint = stationEndpoint;
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            List<StationModel> stations = await _stationEndpoint.GetAll();

            Stations = new ObservableCollection<StationModel>(stations);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
