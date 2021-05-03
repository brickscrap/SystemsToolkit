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

        public async Task<List<DebugProcessDbModel>> GetAllDebugProcessesAsync()
        {
            var output = await _db.LoadDataAsync<DebugProcessDbModel, dynamic>(StoredProcedures.DebugProcesses_GetAll,
                                                                               new { });

            return output.ToList();
        }

        public async Task<DebugProcessDbModel> AddDebugProcess(DebugProcessModel process)
        {
            DebugProcessDbModel output = new();

            try
            {
                _db.StartTransaction();

                await _db.SaveDataInTransactionAsync(StoredProcedures.DebugProcesses_Insert, process);

                var newProcess = await _db.LoadDataInTransactionAsync<DebugProcessDbModel, dynamic>(StoredProcedures.DebugProcess_GetByName,
                                                                                                    new { Name = process.Name });

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
    }
}
