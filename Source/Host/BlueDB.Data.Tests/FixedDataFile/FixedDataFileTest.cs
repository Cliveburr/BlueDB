using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Data.Tests.FixedDataFile
{
    [TestClass]
    public class FixedDataFileTest
    {
        [TestMethod]
        public void FullTest()
        {
            var file1Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file2_test.bin");

            if (File.Exists(file1Path))
            {
                File.Delete(file1Path);
            }

            var datas = GenerateData();

            using (var file = new FixedDataFile<FixedDataFileEntity>(file1Path))
            {
                file.Write(0, datas[0]);
                file.Write(1, datas[1]);
                file.Write(2, datas[2]);

                var data0 = file.Read(0);
                var data1 = file.Read(1);
                var data2 = file.Read(2);

                Assert.AreEqual(datas[0], data0);
                Assert.AreEqual(datas[1], data1);
                Assert.AreEqual(datas[2], data2);
            }

            using (var file = new FixedDataFile<FixedDataFileEntity>(file1Path))
            {
                var data1 = file.Read(1);
                Assert.AreEqual(datas[1], data1);
            }

            using (var file = new FixedDataFile<FixedDataFileEntity>(file1Path))
            {
                Assert.AreEqual(file.Count, 3);

                var i = 0;
                foreach (var data in file)
                {
                    Assert.AreEqual(data, datas[i]);
                    i++;
                }

                i = 0;
                foreach (var data in file)
                {
                    Assert.AreEqual(data, datas[i]);
                    i++;
                }
            }
        }

        private FixedDataFileEntity[] GenerateData()
        {
            return new FixedDataFileEntity[]
            {
                new FixedDataFileEntity
                {
                    Id = 1,
                    Name = "the fist one",
                    CreatedDatetime = DateTime.Now
                },
                new FixedDataFileEntity
                {
                    Id = 2,
                    Name = "this is the second!!",
                    CreatedDatetime = DateTime.Now.AddMinutes(1)
                },
                new FixedDataFileEntity
                {
                    Id = 1,
                    Name = "three is awesome!",
                    CreatedDatetime = DateTime.Now.AddMinutes(2)
                }
            };
        }

    }
}