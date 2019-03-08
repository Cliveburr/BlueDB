using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlueDB.Data.Tests
{
    [TestClass]
    public class SystemFileTest
    {
        [TestMethod]
        public async Task FullTest()
        {
            var file1Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file1_test.bin");

            if (File.Exists(file1Path))
            {
                File.Delete(file1Path);
            }

            using (var file = new SystemFile(file1Path))
            {
                var A200 = GenerateFixedData(200, (byte)'A');
                var M100 = GenerateFixedData(100, (byte)'M');
                var Y100 = GenerateFixedData(100, (byte)'Y');

                file.Position = 0;
                await file.Write(A200);
                await file.Write(Y100);
                await file.Write(100, M100);

                file.Position = 0;
                var A100back = await file.Read(100);
                var M100back = await file.Read(100);
                var Y100back = await file.Read(100);

                TestArrayFullOf(A100back, 100, (byte)'A');
                TestArrayFullOf(M100back, 100, (byte)'M');
                TestArrayFullOf(Y100back, 100, (byte)'Y');

                var M100backAgain = await file.Read(100, 100);
                TestArrayFullOf(M100backAgain, 100, (byte)'M');
            }
        }

        private byte[] GenerateFixedData(int length, byte data)
        {
            return Enumerable.Range(0, length)
                .Select(a => data)
                .ToArray();
        }

        private void TestArrayFullOf(byte[] array, int count, byte data)
        {
            if (array != null)
            {
                if (array.Length == count)
                {
                    for (var i = 0; i < count; i++)
                    {
                        Assert.AreEqual(array.GetValue(i), data);
                    }
                    return;
                }
            }
            throw new AssertFailedException();
        }

        private void TestArrays(byte[] arrayA, byte[] arrayB)
        {
            if (!(arrayA != null ^ arrayB != null))
            {
                if (arrayA != null)
                {
                    for (var i = 0; i < arrayA.Length; i++)
                    {
                        Assert.AreEqual(arrayA.GetValue(i), arrayB.GetValue(i));
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