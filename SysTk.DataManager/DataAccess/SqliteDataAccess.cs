using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace SysTk.DataManager.DataAccess
{
    public class SqliteDataAccess : ISqliteDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string pathToDb)
        {
            string connectionString = GetConnectionString(pathToDb);

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var rows = cnn.Query<T>(sqlStatement, parameters);

                return rows.ToList();
            }
        }

        public void SaveData<T>(string sqlStatement, T parameters, string pathToDb)
        {
            string connectionString = GetConnectionString(pathToDb);

            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                cnn.Execute(sqlStatement, parameters);
            }
        }

        private static string GetConnectionString(string pathToDb)
        {
            return $"Data Source={pathToDb};Version=3;";
        }
    }
}
