using BlueDB.Host.Data;
using BlueDB.Host.Data.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Transaction
{
    public class DatabaseInter : IDisposable
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
            _tables = new Dictionary<string, TableInter>();

            DatabaseInstance.Open(Name, (database) =>
            {
                _data = database;

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

        public void Commit()
        {
            _data.Save();

            foreach (var table in _tables)
            {
                table.Value.Commit();
            }
            Dispose();
        }

        public void Rollback()
        {
            foreach (var table in _tables)
            {
                table.Value.Rollback();
            }
            Dispose();
        }

        public void Dispose()
        {
            TableSelected = null;
            _data = null;
            _tables = null;
        }
    }
}