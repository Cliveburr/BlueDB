﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class Int64Type : SerializeType<long>
    {
        public override bool Test(Type type)
        {
            return type.FullName == "System.Int64";
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((long)value);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            return reader.ReadInt64();
        }
    }

    public class UInt64Type : SerializeType<ulong>
    {
        public override bool Test(Type type)
        {
            return type.FullName == "System.UInt64";
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((ulong)value);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            return reader.ReadUInt64();
        }
    }
}