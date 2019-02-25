using BlueDB.Host.Data.Interface;
using BlueDB.Host.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Host.Data
{
    public static class DatabaseInstance
    {
        private static ConcurrentDoubleChecked<IDatabase> _databases;

        public static readonly Type DatabaseType = typeof(MemoryData.Database);
        public static readonly Type TableType = typeof(MemoryData.Table);

        static DatabaseInstance()
        {
            _databases = new ConcurrentDoubleChecked<IDatabase>();
        }

        public static void Open(string name, Action<IDatabase> callBack)
        {
            var database = _databases.GetOrAdd(name, GenerateDatabase);

            database.Open(name, callBack);
        }

        private static IDatabase GenerateDatabase(string name)
        {
            return Activator.CreateInstance(DatabaseType) as IDatabase;
        }
    }
}