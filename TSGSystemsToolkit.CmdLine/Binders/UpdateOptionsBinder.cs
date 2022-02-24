using AutoMapper.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.Handlers;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Binders
{
    public class UpdateOptionsBinder : BinderBase<UpdateOptions>
    {
        private readonly Option<bool> _updateOption;
        private readonly IHost _host;

        public UpdateOptionsBinder(Option<bool> updateOption, IHost host)
        {
            _updateOption = updateOption;
            _host = host;
        }

        protected override UpdateOptions GetBoundValue(BindingContext bindingContext)
        {
            AddDependencies(bindingContext);

            return new()
            {
                Check = bindingContext.ParseResult.GetValueForOption(_updateOption)
            };
        }

        private void AddDependencies(BindingContext bindingContext)
        {
            bindingContext.AddService<IConfiguration>(x =>
                (IConfiguration)_host.Services.GetService(typeof(IConfiguration)));

            bindingContext.AddService<ILogger<UpdateHandler>>(x =>
                (ILogger<UpdateHandler>)_host.Services.GetService(typeof(ILogger<UpdateHandler>)));
        }
    }
}