using FuelPOS.TankTableTools;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.Handlers;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Binders
{
    public class VeederRootOptionsBinder : BinderBase<VeederRootOptions>
    {
        private readonly Argument<string> _filePathArg;
        private readonly Option<string?> _outputOpt;
        private readonly Option<bool> _fuelPosFileOpt;
        private readonly Option<bool> _csvOpt;
        private readonly IHost _host;

        public VeederRootOptionsBinder(Argument<string> filePathArg,
                                       Option<string?> outputOpt,
                                       Option<bool> fuelPosFileOpt,
                                       Option<bool> csvOpt,
                                       IHost host)
        {
            _filePathArg = filePathArg;
            _outputOpt = outputOpt;
            _fuelPosFileOpt = fuelPosFileOpt;
            _csvOpt = csvOpt;
            _host = host;
        }

        protected override VeederRootOptions GetBoundValue(BindingContext bindingContext)
        {
            AddDependencies(bindingContext);

            return new()
            {
                CreateCsv = bindingContext.ParseResult.GetValueForOption(_csvOpt),
                FilePath = bindingContext.ParseResult.GetValueForArgument(_filePathArg),
                CreateFuelPosFile = bindingContext.ParseResult.GetValueForOption(_fuelPosFileOpt),
                OutputPath = bindingContext.ParseResult.GetValueForOption(_outputOpt)
            };
        }

        private void AddDependencies(BindingContext bindingContext)
        {
            bindingContext.AddService<IVdrRootFileParser>(x =>
                (IVdrRootFileParser)_host.Services.GetService(typeof(IVdrRootFileParser)));

            bindingContext.AddService<ILogger<VeederRootHandler>>(x =>
                (ILogger<VeederRootHandler>)_host.Services.GetService(typeof(ILogger<VeederRootHandler>)));
        }
    }
}
