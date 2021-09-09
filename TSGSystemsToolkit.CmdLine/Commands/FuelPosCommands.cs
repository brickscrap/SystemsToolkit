using FuelPOS.MutationCreator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysTk.DataManager.DataAccess;
using SysTk.DataManager.Models;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Commands
{
    public static class FuelPosCommands
    {
        public static void RunCreateMutationCommands(CreateMutationOptions options)
        {
            CardIdentificationData db = new(options.CardIdentificationPath);

            var data = db.GetAllCards();

            if (options.OutputPath is null)
            {
                string outputDir = Path.GetDirectoryName(options.CardIdentificationPath);
                CrdIdMut.Create(data, outputDir);
            }
            else
            {
                CrdIdMut.Create(data, options.OutputPath);
            }
        }
    }
}
