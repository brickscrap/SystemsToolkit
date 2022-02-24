using Microsoft.Extensions.Hosting;
using System.CommandLine;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Binders
{
    public class AddStationsBinder : BinderBase<AddStationsOptions>
    {
        private readonly Option<string> _many;
        private readonly IHost _host;

        public AddStationsBinder(Option<string> many, IHost host)
        {
            _many = many;
            _host = host;
        }
        protected override AddStationsOptions GetBoundValue(BindingContext bindingContext)
        {
            AddDependencies(bindingContext);

            return new()
            {
                Many = bindingContext.ParseResult.GetValueForOption(_many)
            };
        }

        private void AddDependencies(BindingContext bindingContext)
        {
            bindingContext.AddService<SysTkApiClient>(x =>
                (SysTkApiClient)_host.Services.GetService(typeof(SysTkApiClient)));
        }
    }
}
