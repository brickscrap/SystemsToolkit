using Microsoft.Extensions.Hosting;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Binders;

public class AddStationsBinder : BinderBase<AddStationsOptions>
{
    private readonly Option<string> _file;
    private readonly Option<string> _individual;
    private readonly IHost _host;

    public AddStationsBinder(Option<string> file, Option<string> individual, IHost host)
    {
        _file = file;
        _individual = individual;
        _host = host;
    }
    protected override AddStationsOptions GetBoundValue(BindingContext bindingContext)
    {
        AddDependencies(bindingContext);

        return new()
        {
            File = bindingContext.ParseResult.GetValueForOption(_file),
            Individual = bindingContext.ParseResult.GetValueForOption(_individual)
        };
    }

    private void AddDependencies(BindingContext bindingContext)
    {
        bindingContext.AddService<SysTkApiClient>(x =>
            (SysTkApiClient)_host.Services.GetService(typeof(SysTkApiClient)));
    }
}
