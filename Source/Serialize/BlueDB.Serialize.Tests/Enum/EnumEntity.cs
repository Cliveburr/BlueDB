using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests.Enum
{
#pragma warning disable
    [Serializable]
    public class EnumEntity
    {
        public EnumToTest EnumTest0 { get; set; }
        public EnumToTest EnumTest1 { get; set; }
        public EnumToTest EnumTest2 { get; set; }
        public EnumToTest EnumTest3 { get; set; }

        public void Populate()
        {
            EnumTest0 = EnumToTest.Value0;
            EnumTest1 = EnumToTest.Value1;
            EnumTest2 = EnumToTest.Value2;
            EnumTest3 = EnumToTest.Value3;
        }
    }

    public enum EnumToTest : byte
    {
        Value0 = 0,
        Value1 = 1,
        Value2 = 2,
        Value3 = 3
    }
#pragma warning restore
}