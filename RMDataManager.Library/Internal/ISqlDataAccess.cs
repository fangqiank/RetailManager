using System;
using System.Collections.Generic;

namespace RMDataManager.Library.Internal
{
    public interface ISqlDataAccess : IDisposable
    {
        string GetConnectionString(string name);
        List<T> LoadData<T, U>(string storeProcedure, U parameter, string connectionStringName);
        void SaveData<T>(string storeProcedure, T parameter, string connectionStringName);
        void StartTransaction(string connectionStringName);
        void CommitTransaction();
        void RollbackTransaction();
        void Dispose();
        void SaveDataInTransaction<T>(string storeProcedure, T parameters);
        List<T> LoadDataInTransaction<T, U>(string storeProcedure, U parameter);
    }
}