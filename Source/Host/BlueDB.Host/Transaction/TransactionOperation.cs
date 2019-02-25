using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Transaction
{
    public class TransactionOperation : IDisposable
    {
        public Dictionary<string, DatabaseInter> Databases { get; set; }

        public TransactionOperation()
        {
            Databases = new Dictionary<string, DatabaseInter>();
        }

        public void OpenDatabase(string databaseName, Action<DatabaseInter> callBack)
        {
            if (Databases.ContainsKey(databaseName))
            {
                callBack(Databases[databaseName]);
            }
            else
            {
                var newDatabase = new DatabaseInter(databaseName);
                Databases.Add(databaseName, newDatabase);

                newDatabase.Open(callBack);
            }
        }

        public void Commit(Action callBack)
        {
            foreach (var database in Databases)
            {
                database.Value.Commit();
            }
            Dispose();

            callBack();
        }

        public void Rollback()
        {
            foreach (var database in Databases)
            {
                database.Value.Rollback();
            }
            Dispose();
        }

        public void Dispose()
        {
            Databases = null;
        }
    }
}