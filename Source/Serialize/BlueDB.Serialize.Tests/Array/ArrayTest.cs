using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlueDB.Serialize.Tests.Array
{
    [TestClass]
    public class ArrayTest
    {
        [TestMethod]
        public void CustomSerialize()
        {
            var values = new ArrayEntity();
            values.Populate();

            var sw = Stopwatch.StartNew();

            var serialize = BinarySerialize.From<ArrayEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            sw.Stop();

            Compare(values, returnedValues);

            Debug.WriteLine($"ArrayTest.CustomSerialize - Bytes: {bytes.Length.ToString()} - Elapsed: {sw.Elapsed.ToString()}");
        }

        [TestMethod]
        public void DNETSerialize()
        {
            var values = new ArrayEntity();
            values.Populate();

            var tested = DNETSerializeHelper.TestDNETSerialize(values);

            Compare(values, tested.Item3 as ArrayEntity);

            Debug.WriteLine($"ArrayTest.DNETSerialize - Bytes: {tested.Item2.ToString()} - Elapsed: {tested.Item1.ToString()}");
        }

        private void Compare(ArrayEntity from, ArrayEntity to)
        {
            CompareArray(from.IntArray, to.IntArray);
            CompareArray(from.NullStringArray, to.NullStringArray);
            CompareArray(from.ClassArrayEntityArray, to.ClassArrayEntityArray);
        }

        private void CompareArray(System.Array from, System.Array to)
        {
            if (!(from != null ^ to != null))
            {
                if (from != null)
                {
                    for (var i = 0; i < from.Length; i++)
                    {
                        Assert.AreEqual(from.GetValue(i), to.GetValue(i));
                    }
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}