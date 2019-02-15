using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data
{
    public enum DataState : byte
    {
        Pristine = 0,
        Created = 1,
        Updated = 2,
        Removed = 3
    }
}