using FuelPOS.MutationCreator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysTk.DataManager.DataAccess;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public class MutationHandler : IMutationHandler
    {
        private readonly ILogger<MutationHandler> _logger;
        private readonly ICardIdentificationData _crdIdData;

        public MutationHandler(ILogger<MutationHandler> logger, ICardIdentificationData crdIdData)
        {
            _logger = logger;
            _crdIdData = crdIdData;
        }

        public int RunHandlerAndReturnExitCode(CreateMutationOptions options)
        {
            int result = 1;
            if (!string.IsNullOrWhiteSpace(options.CardIdMutPath))
                result = RunCardIdMut(options);

            return result;
        }

        private int RunCardIdMut(CreateMutationOptions options)
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
}
