using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Host.Data.Interface
{
    public interface IDatabase
    {
        void Open(string name, Action<IDatabase> callBack);
        void OpenTable(string name, Action<ITable> callBack);
        void Save();
    }
}