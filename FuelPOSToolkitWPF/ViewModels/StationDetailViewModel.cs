using FuelPOSToolkitDesktopUI.Library.Models;
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
        private StationModel _selectedStation;
        public StationModel SelectedStation
        {
            get { return _selectedStation; }
            set { SetProperty(ref _selectedStation, value); }
        }

        public StationDetailViewModel()
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("station"))
            {
                SelectedStation = navigationContext.Parameters.GetValue<StationModel>("station");
            }
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
    }
}
