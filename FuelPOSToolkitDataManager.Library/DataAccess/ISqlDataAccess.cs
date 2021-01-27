using System.Collections.Generic;

namespace FuelPOSToolkitDataManager.Library.DataAccess
{
    public interface ISqlDataAccess
    {
        void CommitTransaction();
        void Dispose();
        string GetConnectionString(string name);
        List<T> LoadData<T, U>(string storedProcedure, U parameters);
        List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);
        void RollbackTransaction();
        void SaveData<T>(string storedProcedure, T parameters);
        void SaveDataInTransaction<T>(string storedProcedure, T parameters);
        void StartTransaction();
    }
}