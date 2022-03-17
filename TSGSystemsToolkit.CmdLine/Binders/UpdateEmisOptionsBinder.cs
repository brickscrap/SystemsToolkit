using Microsoft.Extensions.Hosting;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Binders;

internal class UpdateEmisOptionsBinder : BinderBase<UpdateEmisOptions>
{
    private readonly Option<string?> _filePathOption;
    private readonly IHost _host;

    public UpdateEmisOptionsBinder(Option<string?> filePathOption, IHost host)
    {
        _filePathOption = filePathOption;
        _host = host;
    }
    protected override UpdateEmisOptions GetBoundValue(BindingContext bindingContext)
    {
        AddDependencies(bindingContext);

        return new()
        {
            FilePath = bindingContext.ParseResult.GetValueForOption(_filePathOption)
        };
    }

    private void AddDependencies(BindingContext bindingContext)
    {
        bindingContext.AddService<SysTkApiClient>(x =>
            (SysTkApiClient)_host.Services.GetService(typeof(SysTkApiClient)));
    }
}
