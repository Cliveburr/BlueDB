﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlueDB.Serialize.Tests.IEnumerable
{
    [TestClass]
    public class IEnumerableTest
    {
        [TestMethod]
        public void CustomSerialize()
        {
            var values = new IEnumerableEntity();
            values.Populate();

            var sw = Stopwatch.StartNew();

            var serialize = BinarySerialize.From<IEnumerableEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            sw.Stop();

            //Compare(values, returnedValues);

            Debug.WriteLine($"IEnumerableTest.CustomSerialize - Bytes: {bytes.Length.ToString()} - Elapsed: {sw.Elapsed.ToString()}");
        }

        [TestMethod]
        public void DNETSerialize()
        {
            var values = new IEnumerableEntity();
            values.Populate();

            var tested = DNETSerializeHelper.TestDNETSerialize(values);

            Compare(values, tested.Item3 as IEnumerableEntity);

            Debug.WriteLine($"ArrayTest.DNETSerialize - Bytes: {tested.Item2.ToString()} - Elapsed: {tested.Item1.ToString()}");
        }

        private void Compare(IEnumerableEntity from, IEnumerableEntity to)
        {
            CompareArray(from.IEnumerableInt, to.IEnumerableInt);
        }

        private void CompareArray(System.Collections.IEnumerable from, System.Collections.IEnumerable to)
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