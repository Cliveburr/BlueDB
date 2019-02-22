using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlueDB.Serialize.Tests.Class
{
    [TestClass]
    public class ClassTest
    {
        [TestMethod]
        public void CustomSerialize()
        {
            var values = new ClassEntity();
            values.Populate();

            var sw = Stopwatch.StartNew();

            var serialize = BinarySerialize.From<ClassEntity>();

            var bytes = serialize.Serialize(values);

            var returnedValues = serialize.Deserialize(bytes);

            sw.Stop();

            Compare(values, returnedValues);

            Debug.WriteLine($"ClassTest.CustomSerialize - Bytes: {bytes.Length.ToString()} - Elapsed: {sw.Elapsed.ToString()}");
        }

        [TestMethod]
        public void DNETSerialize()
        {
            var values = new ClassEntity();
            values.Populate();

            var tested = DNETSerializeHelper.TestDNETSerialize(values);

            Compare(values, tested.Item3 as ClassEntity);

            Debug.WriteLine($"ClassTest.DNETSerialize - Bytes: {tested.Item2.ToString()} - Elapsed: {tested.Item1.ToString()}");
        }

        public static void Compare(ClassEntity from, ClassEntity to)
        {
            if (!(from != null ^ to != null))
            {
                if (from != null)
                {
                    Assert.AreEqual(from.Name, to.Name);
                    Assert.AreEqual(from.Index, to.Index);
                    Assert.AreEqual(from.Created, to.Created);
                    Assert.AreEqual(from.Deeper, to.Deeper);
                    Assert.AreEqual(from.DeeperNull, to.DeeperNull);
                    Compare(from.Recursive, to.Recursive);
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
}