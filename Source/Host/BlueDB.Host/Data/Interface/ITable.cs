using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data.Interface
{
    public interface ITable
    {
        uint LastId { get; set; }
        void Open(IDatabase database, string name, Action<ITable> callBack);
        IData ReadById(uint id);
        void Save();
        void Insert(IData value);
        void Update(IData value);
        void Remove(IData value);
    }
}