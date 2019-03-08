using BlueDB.Serialize.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests.AttributeSize
{
    public enum SizeEnumEntity : byte
    {
        value0 = 0,
        value1 = 1
    }

    public enum SizeEnumEntity2 : uint
    {
        value0 = 0,
        value1 = 1
    }

    public class ClassSizeEntity1
    {
        public byte Byte { get; set; }
    }

    public class ClassSizeEntity5 : ClassSizeEntity1
    {
        public int Value { get; set; }
    }

    public class ClassSizeEntity13 : ClassSizeEntity5
    {
        public long MoreValue { get; set; }
    }

    public class ClassSizeEntity45 : ClassSizeEntity13
    {
        [SerializeSize(30)]
        public string TestingString { get; set; }
    }

    public class ClassSizeEntity127 : ClassSizeEntity45
    {
        [SerializeSize(20)]
        public int[] FirstArray { get; set; }
    }

    public class ClassSizeEntity179 : ClassSizeEntity127
    {
        [SerializeSize(10)]
        public Dictionary<byte, int> FirstDictionary { get; set; } // 2 ushort + ((1 byte + 4 int) * size) = 52
    }
}