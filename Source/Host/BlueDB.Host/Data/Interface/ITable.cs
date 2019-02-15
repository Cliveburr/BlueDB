using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data.Interface
{
    public interface ITable
    {
        uint LastId { get; }
        void Open(IDatabase database, string name, Action callBack);
        IData ReadById(uint id);
    }
}