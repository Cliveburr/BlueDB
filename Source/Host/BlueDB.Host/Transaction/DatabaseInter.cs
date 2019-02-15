using BlueDB.Host.Data.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Transaction
{
    public class DatabaseInter
    {
        public string Name { get; private set; }
        public TableInter TableSelected { get; set; }

        private IDatabase _data;
        private Dictionary<string, TableInter> _tables;

        public DatabaseInter(string name)
        {
            Name = name;
        }

        public void Open(Action<DatabaseInter> callBack)
        {
            _data = new Data.MemoryData.Database();
            _tables = new Dictionary<string, TableInter>();

            _data.Open(Name, () =>
            {
                callBack(this);
            });
        }

        public void OpenTable(string tableName, Action<TableInter> callBack)
        {
            if (_tables.ContainsKey(tableName))
            {
                callBack(_tables[tableName]);
            }
            else
            {
                var newTable = new TableInter(_data, tableName);
                _tables.Add(tableName, newTable);

                newTable.Open(callBack);
            }
        }
    }
}