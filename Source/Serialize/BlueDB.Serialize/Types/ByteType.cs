using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class ByteType : SerializeType<byte>
    {
        public override bool Test(Type type)
        {
            return type.FullName == "System.Byte";
        }

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