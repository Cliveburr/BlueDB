using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class Int64Provider : IProvider
    {
        public bool Test(Type type)
        {
            return type.FullName == "System.Int64";
        }

        public ISerializeType GetSerializeType(Type type)
        {
            return new Int64Type();
        }
    }

    public class Int64Type : SerializeType<long>
    {
        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((long)value);
        }

        public override object Deserialize(BinaryReader reader)
        {
            return reader.ReadInt64();
        }

        public override int CalculateSize()
        {
            return 8;
        }
    }

    public class UInt64Provider : IProvider
    {
        public bool Test(Type type)
        {
            return type.FullName == "System.UInt64";
        }

        public ISerializeType GetSerializeType(Type type)
        {
            return new UInt64Type();
        }
    }

    public class UInt64Type : SerializeType<ulong>
    {
        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((ulong)value);
        }

        public override object Deserialize(BinaryReader reader)
        {
            return reader.ReadUInt64();
        }

        public override int CalculateSize()
        {
            return 8;
        }
    }
}