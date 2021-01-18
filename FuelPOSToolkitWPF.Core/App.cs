using FuelPOSToolkitWPF.Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;

namespace FuelPOSToolkitWPF.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            // TODO: Dependency Injection here

            RegisterAppStart<LoginViewModel>();
        }
    }
}
