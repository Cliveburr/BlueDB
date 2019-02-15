using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data
{
    public enum DataType : byte
    {
        Byte = 3,
        Int32 = 4,
        String = 5,
        Link = 6
    }
}