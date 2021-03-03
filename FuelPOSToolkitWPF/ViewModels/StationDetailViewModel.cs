using FuelPOSToolkitDesktopUI.Library.API;
using FuelPOSToolkitDesktopUI.Library.Models;
using FuelPOSToolkitWPF.Core;
using FuelPOSToolkitWPF.Models;
using Mapster;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuelPOSToolkitWPF.ViewModels
{
    public class StationDetailViewModel : BindableBase, INavigationAware
    {
        private StationDisplayModel _selectedStation;
        private List<POSDisplayModel> _posList = new List<POSDisplayModel>();
        private readonly IPosEndpoint _posApi;
        private readonly IRegionManager _regionManager;

        public StationDisplayModel SelectedStation
        {
            get { return _selectedStation; }
            set { SetProperty(ref _selectedStation, value); }
        }

        public List<POSDisplayModel> PosList
        {
            get { return _posList; }
            set { SetProperty(ref _posList, value); }
        }

        public StationDetailViewModel(IPosEndpoint posApi, IRegionManager regionManager)
        {
            _posApi = posApi;
            _regionManager = regionManager;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            // TODO: Implement caching
            if (navigationContext.Parameters.ContainsKey("station"))
            {
                SelectedStation = navigationContext.Parameters.GetValue<StationDisplayModel>("station");

                PosList = navigationContext.Parameters.GetValue<List<POSDisplayModel>>("pos");

                foreach (var pos in PosList)
                {
                    var p = new NavigationParameters();
                    p.Add("pos", pos);
                    p.Add("siteId", SelectedStation.Id);

                    _regionManager.RequestNavigate(RegionNames.POSDetailRegion, ViewNames.PosDetaiView, p);
                }
            }            
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            ClearViewModel();
            IRegion region = _regionManager.Regions[RegionNames.POSDetailRegion];
            region.RemoveAll();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void ClearViewModel()
        {
            SelectedStation = null;
            PosList = new List<POSDisplayModel>();
        }
    }
}
