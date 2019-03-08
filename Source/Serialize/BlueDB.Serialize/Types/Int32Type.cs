using BlueDB.Serialize.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class Int32Provider : IProvider
    {
        public bool Test(Type type)
        {
            return type.FullName == "System.Int32";
        }

        public ISerializeType GetSerializeType(Type type)
        {
            return new Int32Type();
        }
    }

    public class Int32Type : SerializeType<int>
    {
        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((int)value);
        }

        public override object Deserialize(BinaryReader reader)
        {
            return reader.ReadInt32();
        }

        public override int CalculateSize()
        {
            return 4;
        }
    }

    public class UInt32Provider : IProvider
    {
        public bool Test(Type type)
        {
            return type.FullName == "System.UInt32";
        }

        public ISerializeType GetSerializeType(Type type)
        {
            return new UInt32Type();
        }
    }

    public class UInt32Type : SerializeType<uint>
    {
        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((uint)value);
        }

        public override object Deserialize(BinaryReader reader)
        {
            return reader.ReadUInt32();
        }

        public override int CalculateSize()
        {
            return 4;
        }
    }
}