using FuelPOSToolkit.WPF.Core;
using FuelPOSToolkit.WPF.Dialogs;
using FuelPOSToolkit.WPF.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Windows;
using TSGSystemsToolkit.DesktopUI.Library.API;
using TSGSystemsToolkit.DesktopUI.Library.Models;

namespace FuelPOSToolkitWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var window = Container.Resolve<MainWindow>();

            InitializeShell(window);

            return window;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register services
            containerRegistry.RegisterSingleton<ILoggedInUserModel, LoggedInUserModel>();
            containerRegistry.RegisterSingleton<IAPIHelper, APIHelper>();
            containerRegistry.RegisterScoped<IStationEndpoint, StationEndpoint>();
            containerRegistry.RegisterScoped<IPosEndpoint, PosEndpoint>();

            // Register views
            containerRegistry.RegisterForNavigation<MainWindow>();
            containerRegistry.RegisterForNavigation<HardwareView>();
            containerRegistry.RegisterForNavigation<StationView>();
            containerRegistry.RegisterForNavigation<StationDetailView>();
            containerRegistry.RegisterForNavigation<PosDetailView>();

            // Register dialogs
            containerRegistry.RegisterDialog<LoginDialog, LoginDialogViewModel>();
        }

        protected override void OnInitialized()
        {
            System.Console.WriteLine();
        }

        protected override void InitializeShell(Window shell)
        {
            var loggedInUser = Container.Resolve<ILoggedInUserModel>();
            var dialogService = Container.Resolve<IDialogService>();

            if (loggedInUser.EmailAddress == null)
            {
                dialogService.ShowDialog(DialogNames.LoginDialog);
            }

            // TODO: Handle loading of Admin module
            
            Current.MainWindow.Show();
        }
    }
}
