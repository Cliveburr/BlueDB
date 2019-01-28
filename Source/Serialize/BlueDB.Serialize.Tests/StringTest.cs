using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlueDB.Serialize.Tests
{
    [TestClass]
    public class StringTest
    {
        [TestMethod]
        public void CustomSerialize()
        {
            var values = new StringEntity();
            values.Populate();

            var sw = Stopwatch.StartNew();

            var serialize = BinarySerialize.From<StringEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            sw.Stop();

            Compare(values, returnedValues);

            Debug.WriteLine($"StringTest.CustomSerialize - Bytes: {bytes.Length.ToString()} - Elapsed: {sw.Elapsed.ToString()}");
        }

        public static void Compare(StringEntity from, StringEntity to)
        {
            Assert.AreEqual(from.SomeString, to.SomeString);
            Assert.AreEqual(from.NullString, to.NullString);
            Assert.AreEqual(from.EmptyString, to.EmptyString);
            Assert.AreEqual(from.ByStringEmptyString, to.ByStringEmptyString);
            Assert.AreEqual(from.WhiteSpaceString, to.WhiteSpaceString);
            Assert.AreEqual(from.LargeString, to.LargeString);
        }
    }
}