using BlueDB.Host.Data.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data.MemoryData
{
    public class Database : IDatabase
    {

        private List<ITable> _tables;

        public void Open(string name, Action callBack)
        {
            _tables = new List<ITable>();

            callBack();
        }
    }
}