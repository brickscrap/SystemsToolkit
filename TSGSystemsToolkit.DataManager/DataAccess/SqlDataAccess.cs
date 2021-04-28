using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _isClosed = false;
        private readonly IConfiguration _config;
        public string ConnectionStringName { get; set; } = "ToolkitDB";

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
            // return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public async Task<List<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters)
        {
            string connectionString = GetConnectionString(ConnectionStringName);

            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                var rows = await cnn.QueryAsync<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);

                return rows.ToList();
            }
        }

        public async Task SaveDataAsync<T>(string storedProcedure, T parameters)
        {
            string connectionString = GetConnectionString(ConnectionStringName);

            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                await cnn.ExecuteAsync(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public void StartTransaction()
        {
            string connectionString = GetConnectionString(ConnectionStringName);

            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();

            _isClosed = false;
        }

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure,
                parameters,
                    commandType: CommandType.StoredProcedure,
                    transaction: _transaction);
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {

            List<T> rows = _connection.Query<T>(storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure,
                transaction: _transaction)
                .ToList();

            return rows;
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();

            _isClosed = true;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();

            _isClosed = true;
        }

        public void Dispose()
        {
            if (_isClosed == false)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    // TODO: Log this issue
                }
            }

            _transaction = null;
            _connection = null;
        }
    }
}
