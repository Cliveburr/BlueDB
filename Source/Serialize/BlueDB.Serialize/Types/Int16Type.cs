using BlueDB.Serialize.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class Int16Type : SerializeType<short>
    {
        public override bool Test(Type type)
        {
            return type.FullName == "System.Int16";
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((short)value);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            return reader.ReadInt16();
        }
    }

    public class UInt16Type : SerializeType<ushort>
    {
        public override bool Test(Type type)
        {
            return type.FullName == "System.UInt16";
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((ushort)value);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            return reader.ReadUInt16();
        }
    }
}