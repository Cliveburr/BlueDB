using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlueDB.Serialize.Tests.Enum
{
    [TestClass]
    public class EnumTest
    {
        [TestMethod]
        public void SingleSerialize()
        {
            var serialize = BinarySerialize.From<EnumToTest>();

            var value = EnumToTest.Value2;

            var bytes = serialize.Serialize(value);

            var returnedValue = serialize.Deserialize(bytes);

            Assert.AreEqual(value, returnedValue);
        }


        [TestMethod]
        public void CustomSerialize()
        {
            var values = new EnumEntity();
            values.Populate();

            var sw = Stopwatch.StartNew();

            var serialize = BinarySerialize.From<EnumEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            sw.Stop();

            Compare(values, returnedValues);

            Debug.WriteLine($"ArrayTest.CustomSerialize - Bytes: {bytes.Length.ToString()} - Elapsed: {sw.Elapsed.ToString()}");
        }

        private void Compare(EnumEntity from, EnumEntity to)
        {
            Assert.AreEqual(from.EnumTest0, to.EnumTest0);
            Assert.AreEqual(from.EnumTest1, to.EnumTest1);
            Assert.AreEqual(from.EnumTest2, to.EnumTest2);
            Assert.AreEqual(from.EnumTest3, to.EnumTest3);
        }
    }
}