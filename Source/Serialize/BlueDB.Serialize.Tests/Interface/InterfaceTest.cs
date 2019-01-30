using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlueDB.Serialize.Tests.Interface
{
    [TestClass]
    public class InterfaceTest
    {
        [TestMethod]
        public void CustomSerialize()
        {
            var values = new InterfaceEntity();
            values.Populate();

            var sw = Stopwatch.StartNew();

            var serialize = BinarySerialize.From<InterfaceEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            sw.Stop();

            Compare(values, returnedValues);

            Debug.WriteLine($"InterfaceTest.CustomSerialize - Bytes: {bytes.Length.ToString()} - Elapsed: {sw.Elapsed.ToString()}");
        }

        [TestMethod]
        public void DNETSerialize()
        {
            var values = new InterfaceEntity();
            values.Populate();

            var tested = DNETSerializeHelper.TestDNETSerialize(values);

            Compare(values, tested.Item3 as InterfaceEntity);

            Debug.WriteLine($"InterfaceTest.DNETSerialize - Bytes: {tested.Item2.ToString()} - Elapsed: {tested.Item1.ToString()}");
        }

        public static void Compare(InterfaceEntity from, InterfaceEntity to)
        {
            Assert.AreEqual(from.ImplementationOne, to.ImplementationOne);
            Assert.AreEqual(from.ImplementationTwo, to.ImplementationTwo);
            Assert.AreEqual(from.SomeNullValue, to.SomeNullValue);
            CompareArray(from.ListOfInterface, to.ListOfInterface);
            CompareArray(from.ArrayOfInterface, to.ArrayOfInterface);
        }

        private static void CompareArray(System.Collections.IEnumerable from, System.Collections.IEnumerable to)
        {
            if (!(from != null ^ to != null))
            {
                if (from != null)
                {
                    var fromList = new List<object>();
                    foreach (var item in from)
                    {
                        fromList.Add(item);
                    }

                    var toList = new List<object>();
                    foreach (var item in to)
                    {
                        toList.Add(item);
                    }

                    for (var i = 0; i < fromList.Count; i++)
                    {
                        Assert.AreEqual(fromList[i], toList[i]);
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