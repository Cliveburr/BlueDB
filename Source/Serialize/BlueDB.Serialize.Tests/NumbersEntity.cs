using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests
{
#pragma warning disable
    [Serializable]
    public class NumbersEntity
    {
        public byte ByteValue { get; set; }
        public byte SimplyByte8 { get; set; }
        public Byte Byte8 { get; set; }

        public short ShortValue { get; set; }
        public short SimplyShort16 { get; set; }
        public Int16 Short16 { get; set; }
        public ushort UShortValue { get; set; }
        public ushort SimplyUshort16 { get; set; }
        public UInt16 UShort16 { get; set; }

        public int IntValue { get; set; }
        public int SimplyInteger32 { get; set; }
        public Int32 Integer32 { get; set; }
        public uint UIntValue { get; set; }
        public uint SimplyUInteger32 { get; set; }
        public UInt32 UInteger32 { get; set; }

        public long LongValue { get; set; }
        public long SimplyLong64 { get; set; }
        public Int64 Long64 { get; set; }
        public ulong ULongValue { get; set; }
        public ulong SimplyULong64 { get; set; }
        public UInt64 ULong64 { get; set; }
    }
#pragma warning restore
}