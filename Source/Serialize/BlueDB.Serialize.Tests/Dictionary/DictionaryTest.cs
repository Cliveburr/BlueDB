using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace BlueDB.Serialize.Tests.Dictionary
{
    [TestClass]
    public class DictionaryTest
    {
        [TestMethod]
        public void CustomSerialize()
        {
            var values = new DictionaryEntity();
            values.Populate();

            var sw = Stopwatch.StartNew();

            var serialize = BinarySerialize.From<DictionaryEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            sw.Stop();

            Compare(values, returnedValues);

            Debug.WriteLine($"DictionaryTest.CustomSerialize - Bytes: {bytes.Length.ToString()} - Elapsed: {sw.Elapsed.ToString()}");

        }

        [TestMethod]
        public void DNETSerialize()
        {
            var values = new DictionaryEntity();
            values.Populate();

            var tested = DNETSerializeHelper.TestDNETSerialize(values);

            Compare(values, tested.Item3 as DictionaryEntity);

            Debug.WriteLine($"DictionaryTest.DNETSerialize - Bytes: {tested.Item2.ToString()} - Elapsed: {tested.Item1.ToString()}");
        }

        private void Compare(DictionaryEntity from, DictionaryEntity to)
        {
            CompareDictionary(from.Dic1, to.Dic1);
        }

        private void CompareDictionary(System.Collections.IDictionary from, System.Collections.IDictionary to)
        {
            if (!(from != null ^ to != null))
            {
                if (from != null)
                {
                    var fromKeys = new object[from.Count];
                    var fromValues = new object[from.Count];
                    var toKeys = new object[from.Count];
                    var toValues = new object[from.Count];

                    var index = 0;
                    var fromEnumerator = from.GetEnumerator();
                    fromEnumerator.Reset();
                    while (fromEnumerator.MoveNext())
                    {
                        fromKeys[index] = fromEnumerator.Key;
                        fromValues[index] = fromEnumerator.Value;
                        index++;
                    }

                    index = 0;
                    var toEnumerator = to.GetEnumerator();
                    toEnumerator.Reset();
                    while (toEnumerator.MoveNext())
                    {
                        toKeys[index] = toEnumerator.Key;
                        toValues[index] = toEnumerator.Value;
                        index++;
                    }

                    for (var i = 0; i < fromKeys.Length; i++)
                    {
                        Assert.AreEqual(fromKeys[i], toKeys[i]);
                        Assert.AreEqual(fromValues[i], toValues[i]);
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