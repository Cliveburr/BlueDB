using BlueDB.Host.Data.Interface;
using BlueDB.Host.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data.MemoryData
{
    public class Table : ITable
    {
        public uint LastId { get; set; }

        private ConcurrentOpen _concurrentOpen;
        private Dictionary<uint, IData> _memoryData;

        public Table()
        {
            _concurrentOpen = new ConcurrentOpen();
        }

        public void Open(IDatabase database, string name, Action<ITable> callBack)
        {
            _concurrentOpen.Open(() => callBack(this), ExecuteOpen);
        }

        private void ExecuteOpen()
        {
            LastId = 0;
            _memoryData = new Dictionary<uint, IData>();

            _concurrentOpen.SetOpened();
        }

        public IData ReadById(uint id)
        {
            if (_memoryData.ContainsKey(id))
            {
                return _memoryData[id];
            }
            else
            {
                return null;
            }
        }

        public void Save()
        {
        }

        public void Insert(IData value)
        {
            _memoryData.Add(value.Id, value);
        }

        public void Update(IData value)
        {
            _memoryData[value.Id] = value;
        }

        public void Remove(IData value)
        {
            _memoryData.Remove(value.Id);
        }
    }
}