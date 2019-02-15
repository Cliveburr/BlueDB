using BlueDB.Host.Data.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data.MemoryData
{
    public class Table : ITable
    {
        public uint LastId { get; private set; }

        private Dictionary<uint, IData> _persistentData;

        public void Open(IDatabase database, string name, Action callBack)
        {
            LastId = 0;
            _persistentData = new Dictionary<uint, IData>();

            callBack();
        }

        public IData ReadById(uint id)
        {
            if (_persistentData.ContainsKey(id))
            {
                return _persistentData[id];
            }
            else
            {
                return null;
            }
        }
    }
}