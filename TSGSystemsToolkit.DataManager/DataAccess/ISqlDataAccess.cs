﻿using System;
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
        Task<List<T>> LoadDataInTransactionAsync<T, U>(string storedProcedure, U parameters);
        Task<List<T>> LoadMultiMappedAsync<T, V, U>(string storedProcedure, Func<T, V, T> mapping, U parameters, string splitOn);
        void RollbackTransaction();
        Task SaveDataAsync<T>(string storedProcedure, T parameters);
        Task SaveDataInTransactionAsync<T>(string storedProcedure, T parameters);
        void StartTransaction();
    }
}