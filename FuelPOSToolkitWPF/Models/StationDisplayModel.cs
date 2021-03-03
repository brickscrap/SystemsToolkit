using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FuelPOSToolkitWPF.Models
{
    public class StationDisplayModel : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string KimoceId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
        public int NumberOfPOS { get; set; }
        public string Header
        {
            get
            {
                return $"{Id} - {Name}";
            }
        }

        public string Description
        {
            get
            {
                return $"{Id} - {Company} - {Name}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CallPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
