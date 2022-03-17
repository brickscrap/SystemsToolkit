using FuelPOS.StatDevParser;
using Microsoft.Extensions.Hosting;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.Handlers;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Binders;

public class SurveyOptionsBinder : BinderBase<SurveyOptions>
{
    private readonly Argument<string> _filePathArg;
    private readonly Option<string> _outputOpt;
    private readonly Option<bool> _fuelPosOpt;
    private readonly Option<bool> _serialNumOpt;
    private readonly IHost _host;

    public SurveyOptionsBinder(Argument<string> filePathArg, Option<string> outputOpt, Option<bool> fuelPosOpt, Option<bool> serialNumOpt, IHost host)
    {
        _filePathArg = filePathArg;
        _outputOpt = outputOpt;
        _fuelPosOpt = fuelPosOpt;
        _serialNumOpt = serialNumOpt;
        _host = host;
    }
    protected override SurveyOptions GetBoundValue(BindingContext bindingContext)
    {
        AddDependencies(bindingContext);

        return new()
        {
            FilePath = bindingContext.ParseResult.GetValueForArgument(_filePathArg),
            OutputPath = bindingContext.ParseResult.GetValueForOption(_outputOpt),
            FuelPosSurvey = bindingContext.ParseResult.GetValueForOption(_fuelPosOpt),
            SerialNumberSurvey = bindingContext.ParseResult.GetValueForOption(_serialNumOpt)
        };
    }

    private void AddDependencies(BindingContext bindingContext)
    {
        bindingContext.AddService<ILogger<SurveyHandler>>(x =>
            _host.Services.GetService(typeof(ILogger<SurveyHandler>)) as ILogger<SurveyHandler>);

        bindingContext.AddService<IStatDevParser>(x =>
            _host.Services.GetService(typeof(IStatDevParser)) as IStatDevParser);
    }
}
