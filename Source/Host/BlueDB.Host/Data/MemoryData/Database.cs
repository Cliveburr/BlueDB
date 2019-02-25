using BlueDB.Host.Data.Interface;
using BlueDB.Host.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data.MemoryData
{
    public class Database : IDatabase
    {
        private ConcurrentOpen _concurrentOpen;
        private ConcurrentDoubleChecked<ITable> _tables;

        public Database()
        {
            _concurrentOpen = new ConcurrentOpen();
        }

        public void Open(string name, Action<IDatabase> callBack)
        {
            _concurrentOpen.Open(() => callBack(this), ExecuteOpen);
        }

        private void ExecuteOpen()
        {
            _tables = new ConcurrentDoubleChecked<ITable>();

            _concurrentOpen.SetOpened();
        }

        public void OpenTable(string name, Action<ITable> callBack)
        {
            var table = _tables.GetOrAdd(name, GenerateTable);

            table.Open(this, name, callBack);
        }

        private ITable GenerateTable(string name)
        {
            return Activator.CreateInstance(DatabaseInstance.TableType) as ITable;
        }

        public void Save()
        {
        }
    }
}