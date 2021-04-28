using System.Collections.Generic;
using System.Threading.Tasks;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public interface ISqlDataAccess
    {
        string ConnectionStringName { get; set; }

        void CommitTransaction();
        void Dispose();
        string GetConnectionString(string name);
        Task<List<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters);
        List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);
        void RollbackTransaction();
        Task SaveDataAsync<T>(string storedProcedure, T parameters);
        void SaveDataInTransaction<T>(string storedProcedure, T parameters);
        void StartTransaction();
    }
}