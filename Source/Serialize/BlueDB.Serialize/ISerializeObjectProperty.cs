using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Serialize
{
    public interface ISerializeObjectProperty
    {
        void Serialize(BinaryWriter writer, object obj);
        void Deserialize(BinaryReader reader, object obj);
    }
}