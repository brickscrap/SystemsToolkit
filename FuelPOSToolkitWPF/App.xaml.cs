using FuelPOSToolkitDesktopUI.Library.API;
using FuelPOSToolkitDesktopUI.Library.Models;
using FuelPOSToolkitWPF.Dialogs;
using FuelPOSToolkitWPF.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Windows;

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

            // Register views
            containerRegistry.RegisterForNavigation<MainWindow>();

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
                dialogService.ShowDialog("LoginDialog");
            }

            // TODO: Handle loading of Admin moduleI 
            
            Current.MainWindow.Show();
        }
    }
}
