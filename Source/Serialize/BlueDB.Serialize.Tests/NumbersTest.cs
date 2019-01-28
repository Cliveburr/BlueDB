using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BlueDB.Serialize.Tests
{
    [TestClass]
    public class NumbersTest
    {
        private NumbersEntity _values = new NumbersEntity
        {
            ByteValue = 128,
            SimplyByte8 = byte.MinValue,
            Byte8 = byte.MaxValue,

            ShortValue = 16000,
            SimplyShort16 = short.MinValue,
            Short16 = short.MaxValue,
            UShortValue = 32000,
            SimplyUshort16 = ushort.MinValue,
            UShort16 = ushort.MaxValue,

            IntValue = 32000,
            SimplyInteger32 = int.MinValue,
            Integer32 = int.MaxValue,
            UIntValue = 64000,
            SimplyUInteger32 = uint.MinValue,
            UInteger32 = uint.MaxValue,

            LongValue = 64000,
            SimplyLong64 = long.MinValue,
            Long64 = long.MaxValue,
            ULong64 = 128000,
            SimplyULong64 = ulong.MinValue,
            ULongValue = ulong.MaxValue
        };

        [TestMethod]
        public void CustomSerialize()
        {

            var sw = Stopwatch.StartNew();

            var serialize = BinarySerialize.From<NumbersEntity>();

            var bytes = serialize.Serialize(_values);

            var returnedValues = serialize.Deserialize(bytes);

            sw.Stop();

            Compare(_values, returnedValues);

            Debug.WriteLine($"TestNumber - Bytes: {bytes.Length.ToString()} - Elapsed: {sw.Elapsed.ToString()}");
        }

        [TestMethod]
        public void DNETSerialize()
        {
            var sw = Stopwatch.StartNew();

            using (var serializeStream = new MemoryStream())
            {
                var serializeFormatter = new BinaryFormatter();
                serializeFormatter.Serialize(serializeStream, _values);
                serializeStream.Flush();
                serializeStream.Position = 0;
                var bytes = serializeStream.ToArray();

                using (var deserializeStream = new MemoryStream(bytes))
                {
                    var deserializeFormatter = new BinaryFormatter();
                    deserializeStream.Seek(0, SeekOrigin.Begin);
                    var returnedValues = deserializeFormatter.Deserialize(deserializeStream) as NumbersEntity;

                    sw.Stop();

                    Compare(_values, returnedValues);

                    Debug.WriteLine($"DNETSerialize - Bytes: {bytes.Length.ToString()} - Elapsed: {sw.Elapsed.ToString()}");
                }
            }
        }

        private void Compare(NumbersEntity from, NumbersEntity to)
        {
            Assert.AreEqual(from.ByteValue, to.ByteValue);
            Assert.AreEqual(from.SimplyByte8, to.SimplyByte8);
            Assert.AreEqual(from.Byte8, to.Byte8);
            Assert.AreEqual(from.ShortValue, to.ShortValue);
            Assert.AreEqual(from.SimplyShort16, to.SimplyShort16);
            Assert.AreEqual(from.Short16, to.Short16);
            Assert.AreEqual(from.UShortValue, to.UShortValue);
            Assert.AreEqual(from.SimplyUshort16, to.SimplyUshort16);
            Assert.AreEqual(from.UShort16, to.UShort16);
            Assert.AreEqual(from.IntValue, to.IntValue);
            Assert.AreEqual(from.SimplyInteger32, to.SimplyInteger32);
            Assert.AreEqual(from.Integer32, to.Integer32);
            Assert.AreEqual(from.UIntValue, to.UIntValue);
            Assert.AreEqual(from.SimplyUInteger32, to.SimplyUInteger32);
            Assert.AreEqual(from.UInteger32, to.UInteger32);
            Assert.AreEqual(from.LongValue, to.LongValue);
            Assert.AreEqual(from.SimplyLong64, to.SimplyLong64);
            Assert.AreEqual(from.Long64, to.Long64);
            Assert.AreEqual(from.ULongValue, to.ULongValue);
            Assert.AreEqual(from.SimplyULong64, to.SimplyULong64);
            Assert.AreEqual(from.ULong64, to.ULong64);
        }
    }
}