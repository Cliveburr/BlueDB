using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize.Tests.AttributeSize
{
    [TestClass]
    public class AttributeSizeTest
    {
        [TestMethod]
        public void DirectCalculateSizeTest()
        {
            TestIfValueIsRight(BinarySerialize.From<byte>(), 1);
            TestIfValueIsRight(BinarySerialize.From<System.DateTime>(), 8);
            TestIfValueIsRight(BinarySerialize.From<Int16>(), 2);
            TestIfValueIsRight(BinarySerialize.From<UInt16>(), 2);
            TestIfValueIsRight(BinarySerialize.From<Int32>(), 4);
            TestIfValueIsRight(BinarySerialize.From<UInt32>(), 4);
            TestIfValueIsRight(BinarySerialize.From<Int64>(), 8);
            TestIfValueIsRight(BinarySerialize.From<UInt64>(), 8);

            TestIfValueIsRight(BinarySerialize.From<SizeEnumEntity>(), 1);
            TestIfValueIsRight(BinarySerialize.From<SizeEnumEntity2>(), 4);

            TestIfValueIsRight(BinarySerialize.From<ClassSizeEntity1>(), 1);
            TestIfValueIsRight(BinarySerialize.From<ClassSizeEntity5>(), 5);
            TestIfValueIsRight(BinarySerialize.From<ClassSizeEntity13>(), 13);
            TestIfValueIsRight(BinarySerialize.From<ClassSizeEntity45>(), 45);
            TestIfValueIsRight(BinarySerialize.From<ClassSizeEntity127>(), 127);
            TestIfValueIsRight(BinarySerialize.From<ClassSizeEntity179>(), 179);
        }

        private void TestIfValueIsRight(ISerializeType serialize, int size)
        {
            var fromCalculate = serialize.CalculateSize();
            Assert.AreEqual(fromCalculate, size);
        }
    }
}