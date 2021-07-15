using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Constants;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public class PosDebugData : IPosDebugData
    {
        private readonly ISqlDataAccess _db;

        public PosDebugData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<DebugProcessModel>> GetAllDebugProcessesAsync()
        {
            var output = await _db.LoadDataAsync<DebugProcessModel, dynamic>(StoredProcedures.DebugProcesses.GetAll,
                                                                               new { });

            return output.ToList();
        }

        public async Task<DebugProcessModel> AddDebugProcess(DebugProcessModel process)
        {
            DebugProcessModel output = new();

            try
            {
                _db.StartTransaction();

                await _db.SaveDataInTransactionAsync(StoredProcedures.DebugProcesses.Insert, process);

                var newProcess = await _db.LoadDataInTransactionAsync<DebugProcessModel, dynamic>(StoredProcedures.DebugProcesses.GetByName,
                                                                                                    new { process.Name });

                if (newProcess is not null)
                {
                    output = newProcess.FirstOrDefault();
                }

                _db.CommitTransaction();
            }
            catch
            {
                _db.RollbackTransaction();
            }

            return output;
        }

        public async Task<List<DebugProcessParametersModel>> GetAllWithParameters()
        {
            var processDictionary = new Dictionary<int, DebugProcessParametersModel>();

            var output = await _db.LoadMultiMappedAsync<DebugProcessParametersModel, DebugParametersModel, dynamic>(
                StoredProcedures.DebugProcesses.GetAllWithParams,
                (proc, param) =>
                {
                    DebugProcessParametersModel processEntry = new();

                    if (!processDictionary.TryGetValue(proc.Id, out processEntry))
                    {
                        processEntry = proc;
                        processEntry.Parameters = new List<DebugParametersModel>();
                        processDictionary.Add(processEntry.Id, processEntry);
                    }

                    if (param is not null)
                    {
                        processEntry.Parameters.Add(param);
                    }


                    return processEntry;
                },
                new { }, "ParameterId");

            var result = processDictionary.Values.ToList();

            return result;
        }
    }
}
