using FuelPOS.TankTableTools;
using Microsoft.Extensions.Hosting;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.Handlers;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Binders;

public class ProGaugeOptionsBinder : BinderBase<ProgaugeOptions>
{
    private readonly Argument<string> _filePathArg;
    private readonly Option<string?> _outputOpt;
    private readonly Option<bool> _fuelPosFileOption;
    private readonly IHost _host;

    public ProGaugeOptionsBinder(Argument<string> filePathArg, Option<string?> outputOpt, Option<bool> fuelPosFileOption, IHost host)
    {
        _filePathArg = filePathArg;
        _outputOpt = outputOpt;
        _fuelPosFileOption = fuelPosFileOption;
        _host = host;
    }
    protected override ProgaugeOptions GetBoundValue(BindingContext bindingContext)
    {
        AddDependencies(bindingContext);

        return new()
        {
            CreateFuelPosFile = bindingContext.ParseResult.GetValueForOption(_fuelPosFileOption),
            FilePath = bindingContext.ParseResult.GetValueForArgument(_filePathArg),
            OutputPath = bindingContext.ParseResult.GetValueForOption(_outputOpt)
        };
    }

    private void AddDependencies(BindingContext bindingContext)
    {
        bindingContext.AddService<ILogger<ProgaugeHandler>>(x =>
            _host.Services.GetService(typeof(ILogger<ProgaugeHandler>)) as ILogger<ProgaugeHandler>);

        bindingContext.AddService<IProgaugeFileParser>(x =>
            _host.Services.GetService(typeof(IProgaugeFileParser)) as IProgaugeFileParser);
    }
}
