using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data
{
    public enum DataState : byte
    {
        Pristine = 0,
        Create = 1,
        Update = 2,
        Remove = 3
    }
}