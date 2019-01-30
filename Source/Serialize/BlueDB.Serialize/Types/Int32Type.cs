﻿using BlueDB.Serialize.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class Int32Type : SerializeType<int>
    {
        public override bool Test(Type type)
        {
            return type.FullName == "System.Int32";
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((int)value);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            return reader.ReadInt32();
        }
    }

    public class UInt32Type : SerializeType<uint>
    {
        public override bool Test(Type type)
        {
            return type.FullName == "System.UInt32";
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            writer.Write((uint)value);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            return reader.ReadUInt32();
        }
    }
}