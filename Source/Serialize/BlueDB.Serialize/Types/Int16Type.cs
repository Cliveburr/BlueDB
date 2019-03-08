using BlueDB.Serialize.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class Int16Provider : IProvider
    {
        public bool Test(Type type)
        {
            return type.FullName == "System.Int16";
        }

        public ISerializeType GetSerializeType(Type type)
        {
            return new Int16Type();
        }
    }

    public class Int16Type : SerializeType<short>
    {
        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((short)value);
        }

        public override object Deserialize(BinaryReader reader)
        {
            return reader.ReadInt16();
        }

        public override int CalculateSize()
        {
            return 2;
        }
    }

    public class UInt16Provider : IProvider
    {
        public bool Test(Type type)
        {
            return type.FullName == "System.UInt16";
        }

        public ISerializeType GetSerializeType(Type type)
        {
            return new UInt16Type();
        }
    }

    public class UInt16Type : SerializeType<ushort>
    {
        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((ushort)value);
        }

        public override object Deserialize(BinaryReader reader)
        {
            return reader.ReadUInt16();
        }

        public override int CalculateSize()
        {
            return 2;
        }
    }
}