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

        public async Task<List<T>> LoadMultiMappedAsync<T, V, U>(string storedProcedure, Func<T, V, T> mapping, U parameters, string splitOn)
        {
            string connectionString = GetConnectionString(ConnectionStringName);

            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                var rows = await cnn.QueryAsync<T, V, T>(storedProcedure, mapping, parameters, splitOn: splitOn,
                                                         commandType: CommandType.StoredProcedure);

                return rows.ToList();
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

        public async Task SaveDataInTransactionAsync<T>(string storedProcedure, T parameters)
        {
            await _connection.ExecuteAsync(storedProcedure,
                parameters,
                    commandType: CommandType.StoredProcedure,
                    transaction: _transaction);
        }

        public async Task<List<T>> LoadDataInTransactionAsync<T, U>(string storedProcedure, U parameters)
        {

            var rows = await _connection.QueryAsync<T>(storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure,
                transaction: _transaction);

            return rows.ToList();
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
