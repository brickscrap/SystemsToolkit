using Microsoft.Extensions.Hosting;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.Handlers;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Binders;

internal class MutationOptionsBinder : BinderBase<MutationOptions>
{
    private readonly Option<string?> _cardId;
    private readonly Option<string?> _output;
    private readonly IHost _host;

    public MutationOptionsBinder(Option<string> cardId, Option<string> output, IHost host)
    {
        _cardId = cardId;
        _output = output;
        _host = host;
    }

    protected override MutationOptions GetBoundValue(BindingContext bindingContext)
    {
        AddDependencies(bindingContext);

        return new()
        {
            CardIdMutPath = bindingContext.ParseResult.GetValueForOption(_cardId),
            OutputPath = bindingContext.ParseResult.GetValueForOption(_output)
        };
    }

    private void AddDependencies(BindingContext context)
    {
        context.AddService<ILogger<MutationHandler>>(x =>
            _host.Services.GetService(typeof(ILogger<MutationHandler>)) as ILogger<MutationHandler>);
    }
}
