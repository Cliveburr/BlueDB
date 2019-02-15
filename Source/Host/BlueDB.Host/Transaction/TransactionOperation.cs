using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Transaction
{
    public class TransactionOperation
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
    }
}