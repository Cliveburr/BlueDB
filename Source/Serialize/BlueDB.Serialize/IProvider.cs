using BlueDB.Serialize.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize
{
    public interface IProvider
    {
        bool Test(Type type);
        ISerializeType GetSerializeType(Type type);
    }
}