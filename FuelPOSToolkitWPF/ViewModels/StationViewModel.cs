using FuelPOSToolkitDesktopUI.Library.API;
using FuelPOSToolkitDesktopUI.Library.Models;
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
        private StationModel _selectedStation;
        private string _filter = "";
        private ICollectionView _stationsView;

        public DelegateCommand OpenStationCommand { get; private set; }

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

        public StationViewModel(IStationEndpoint stationEndpoint, IRegionManager regionManager)
        {
            _stationEndpoint = stationEndpoint;
            _regionManager = regionManager;
            OpenStationCommand = new DelegateCommand(OpenStation);
        }

        private void OpenStation()
        {
            if (_selectedStation == null)
            {
                return;
            }

            var p = new NavigationParameters();
            p.Add("station", _selectedStation);

            _regionManager.RequestNavigate("StationDetailRegion", "StationDetailView", p);
        }

        private bool FilterStations(object obj)
        {
            StationModel station = obj as StationModel;
            bool output = false;

            if (station != null)
            {
                if (station.Id.StartsWith(Filter) || station.Company.StartsWith(Filter) || station.Name.StartsWith(Filter))
                {
                    output = true;
                }
            }

            return output;
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            List<StationModel> stations = await _stationEndpoint.GetAll();

            StationsView = new ListCollectionView(stations);
            StationsView.Filter = FilterStations;
            StationsView.CurrentChanged += SelectedStationChanged;
        }

        private void SelectedStationChanged(object sender, EventArgs e)
        {
            _selectedStation = StationsView.CurrentItem as StationModel;
            OpenStation();
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
