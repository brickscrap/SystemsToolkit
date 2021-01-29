using FuelPOSToolkitDesktopUI.Library.API;
using FuelPOSToolkitDesktopUI.Library.Models;
using FuelPOSToolkitWPF.Dialogs;
using FuelPOSToolkitWPF.Views;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Windows;

namespace FuelPOSToolkitWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register services
            containerRegistry.RegisterSingleton<ILoggedInUserModel, LoggedInUserModel>();
            containerRegistry.RegisterSingleton<IAPIHelper, APIHelper>();

            // Register views
            containerRegistry.RegisterForNavigation<LoginView>();
            containerRegistry.RegisterForNavigation<MainWindow>();

            // Register dialogs
            containerRegistry.RegisterDialog<LoginDialog, LoginDialogViewModel>();
        }
    }
}
