using Prism.Commands;
using Prism.Mvvm;
using System;

namespace FuelPOSToolkitWPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "FuelPOS Toolkit";

        public DelegateCommand ExitCommand { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
            ExitCommand = new DelegateCommand(Exit);
        }

        private void Exit()
        {
            Environment.Exit(0);
        }
    }
}
