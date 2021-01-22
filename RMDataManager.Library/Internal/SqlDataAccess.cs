using System;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RMDataManager.Library.Internal
{
    /*internal（内部）：限定的是只有在同一程序集中可访问，可以跨类
    protected（受保护）：限定的是只有在继承的子类中可访问，可以跨程序集
    protected internal：受保护“或”内部修饰符修饰成员,当父类与子类在同一个程序集中，internal成员可见。
    当父类与子类不在同一个程序集中，子类不能访问父类internal成员，而子类可以访问父类的ptotected internal成员，
    即从当前程序集或从包含类派生的类型，可以访问具有访问修饰符 protected internal 的类型或成员。*/
    internal class SqlDataAccess:IDisposable
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string storeProcedure, U parameter, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(
                    storeProcedure, parameter, commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storeProcedure, T parameter, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storeProcedure, parameter, commandType: CommandType.StoredProcedure);
            }
        }

        //open connect/start transaction method
        //load using the transaction
        //Save using the transaction
        //Close connection/stop transaction method
        //Dispose

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTransaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            _connection = new SqlConnection(connectionString);
            _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
        }

        public void Dispose()
        {
            CommitTransaction();
        }

        public void SaveDataInTransaction<T>(string storeProcedure, T parameters)
        { 
            _connection.Execute(storeProcedure, parameters, 
                commandType: CommandType.StoredProcedure, transaction:_transaction);
        }

        public List<T> LoadDataInTransaction<T, U>(string storeProcedure, U parameter)
        {
            List<T> rows = _connection.Query<T>(
                storeProcedure, parameter, commandType: CommandType.StoredProcedure,transaction:_transaction).ToList();

            return rows;
        }
    }
}
