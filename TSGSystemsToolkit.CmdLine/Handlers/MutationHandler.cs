using FuelPOS.MutationCreator;
using System.IO;
using SysTk.DataManager.DataAccess;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers;

public class MutationHandler : ICommandHandler
{
    private readonly MutationOptions _options;
    private readonly CancellationToken _ct;
    private ILogger<MutationHandler> _logger;
    private ICardIdentificationData _crdIdData;

    public MutationHandler(MutationOptions options, CancellationToken ct = default)
    {
        _options = options;
        _ct = ct;
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        GetDependencies(context);

        int result = 1;
        if (!string.IsNullOrWhiteSpace(_options.CardIdMutPath))
        {
            result = RunCardIdMut(_options);
        }

        return result;
    }

    private void GetDependencies(InvocationContext context)
    {
        _logger = context.BindingContext.GetService(typeof(ILogger<MutationHandler>)) as ILogger<MutationHandler>;
        _crdIdData = context.BindingContext.GetService(typeof(ICardIdentificationData)) as ICardIdentificationData;
    }

    private int RunCardIdMut(MutationOptions options)
    {
        var data = _crdIdData.GetAllCards(options.CardIdMutPath);

        if (options.OutputPath is null)
        {
            string outputDir = Path.GetDirectoryName(options.CardIdMutPath);
            CrdIdMut.Create(data, outputDir);
        }
        else
        {
            CrdIdMut.Create(data, options.OutputPath);
        }

        return 1;
    }
}
