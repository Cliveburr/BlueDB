using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Entity
{
    public enum PropertyType : byte
    {
        Invalid = 0,
        Id = 1,
        Delete = 2,
        Byte = 3,
        Int32 = 4,
        String = 5,
        Link = 6
    }
}