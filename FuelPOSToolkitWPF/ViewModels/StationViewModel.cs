using FuelPOSToolkitDesktopUI.Library.API;
using FuelPOSToolkitDesktopUI.Library.Models;
using FuelPOSToolkitWPF.Core;
using FuelPOSToolkitWPF.Models;
using Mapster;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace FuelPOSToolkitWPF.ViewModels
{
    public class StationViewModel : BindableBase, INavigationAware
    {
        private readonly IStationEndpoint _stationEndpoint;
        private readonly IRegionManager _regionManager;
        private readonly IPosEndpoint _posEndpoint;
        private StationDisplayModel _selectedStation;
        private string _filter = "";
        private ICollectionView _stationsView;

        public StationDisplayModel SelectedStation
        {
            get { return _selectedStation; }
            set { SetProperty(ref _selectedStation, value); }
        }

        public string Filter
        {
            get { return _filter; }
            set 
            {
                SetProperty(ref _filter, value);
                StationsView.Refresh();
            }
        }

        public ICollectionView StationsView
        {
            get { return _stationsView; }
            set { SetProperty(ref _stationsView, value); }
        }

        public DelegateCommand OpenStationCommand { get; private set; }

        public StationViewModel(IStationEndpoint stationEndpoint, IRegionManager regionManager, IPosEndpoint posEndpoint)
        {
            _stationEndpoint = stationEndpoint;
            _regionManager = regionManager;
            _posEndpoint = posEndpoint;
            OpenStationCommand = new DelegateCommand(OpenStation);
        }

        private async void OpenStation()
        {
            if (_selectedStation == null)
            {
                return;
            }

            var posList = await _posEndpoint.GetAllByStationId(_selectedStation.Id);
            var posDisplayList = posList.Adapt<IEnumerable<POSDisplayModel>>().ToList();

            var p = new NavigationParameters();
            p.Add("station", _selectedStation);
            p.Add("pos", posDisplayList);

            _regionManager.RequestNavigate(RegionNames.StationDetailRegion, ViewNames.StationDetailView, p);
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var stations = await _stationEndpoint.GetAll();

            StationsView = new ListCollectionView(stations.Adapt<IEnumerable<StationDisplayModel>>().ToList());
            StationsView.CurrentChanged += SelectedStationChanged;
        }

        private void SelectedStationChanged(object sender, EventArgs e)
        {
            _selectedStation = StationsView.CurrentItem as StationDisplayModel;
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
