using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class ByteProvider : IProvider
    {
        public bool Test(Type type)
        {
            return type.FullName == "System.Byte";
        }

        public ISerializeType GetSerializeType(Type type)
        {
            return new ByteType();
        }
    }

    public class ByteType : SerializeType<byte>
    {
        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((byte)value);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            return reader.ReadByte();
        }
    }
}