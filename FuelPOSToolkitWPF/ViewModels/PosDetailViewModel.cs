using FuelPOSToolkitWPF.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuelPOSToolkitWPF.ViewModels
{
    public class PosDetailViewModel : BindableBase, INavigationAware
    {
        private POSDisplayModel _posDisplay;
        private string _posId;

        public POSDisplayModel PosDisplay
        {
            get { return _posDisplay; }
            set { SetProperty(ref _posDisplay, value); }
        }
        
        public string PosId
        {
            get { return _posId; }
            set { SetProperty(ref _posId, value); }
        }

        public PosDetailViewModel()
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var stationId = navigationContext.Parameters.GetValue<string>("siteId");
            var posNumber = navigationContext.Parameters.GetValue<POSDisplayModel>("pos").Number;

            if (PosId == $"{stationId}{posNumber}")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("pos"))
            {
                PosDisplay = navigationContext.Parameters.GetValue<POSDisplayModel>("pos");
                var stationId = navigationContext.Parameters.GetValue<string>("siteId");
                PosId = $"{stationId}{PosDisplay.Number}";
            }
        }
    }
}
