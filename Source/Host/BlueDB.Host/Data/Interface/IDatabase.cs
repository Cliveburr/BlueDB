using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data.Interface
{
    public interface IDatabase
    {
        void Open(string name, Action callBack);
    }
}