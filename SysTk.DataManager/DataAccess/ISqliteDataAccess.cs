using System.Collections.Generic;

namespace SysTk.DataManager.DataAccess
{
    public interface ISqliteDataAccess
    {
        List<T> LoadData<T, U>(string sqlStatement, U parameters, string pathToDb);
        void SaveData<T>(string sqlStatement, T parameters, string pathToDb);
    }
}