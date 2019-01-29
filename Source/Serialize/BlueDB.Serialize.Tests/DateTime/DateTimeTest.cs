using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlueDB.Serialize.Tests.DateTime
{
    [TestClass]
    public class DateTimeTest
    {
        [TestMethod]
        public void CustomSerialize()
        {
            var values = new DateTimeEntity();
            values.Populate();

            var sw = Stopwatch.StartNew();

            var serialize = BinarySerialize.From<DateTimeEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            sw.Stop();

            Compare(values, returnedValues);

            Debug.WriteLine($"DateTimeTest.CustomSerialize - Bytes: {bytes.Length.ToString()} - Elapsed: {sw.Elapsed.ToString()}");
        }

        [TestMethod]
        public void DNETSerialize()
        {
            var values = new DateTimeEntity();
            values.Populate();

            var tested = DNETSerializeHelper.TestDNETSerialize(values);

            Compare(values, tested.Item3 as DateTimeEntity);

            Debug.WriteLine($"DateTimeTest.DNETSerialize - Bytes: {tested.Item2.ToString()} - Elapsed: {tested.Item1.ToString()}");
        }

        public static void Compare(DateTimeEntity from, DateTimeEntity to)
        {
            Assert.AreEqual(from.DateTime, to.DateTime);
            Assert.AreEqual(from.MinDateTime, to.MinDateTime);
            Assert.AreEqual(from.MaxDateTime, to.MaxDateTime);
            Assert.AreEqual(from.ZeroDateTime, to.ZeroDateTime);
            Assert.AreEqual(from.JustNewDateTime, to.JustNewDateTime);
        }
    }
}