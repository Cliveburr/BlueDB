using BlueDB.Data.DataBlock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlueDB.Data.Tests
{
    [TestClass]
    public class BlockStreamTest
    {
        [TestMethod]
        public void FullTest()
        {
            var file3Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file3_test.bin");

            if (File.Exists(file3Path))
            {
                File.Delete(file3Path);
            }

            var apos = 0;
            var ypos = 0;
            var mpos = 0;

            using (var file = File.Open(file3Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var block = new FileBlockStream(file, 50))
            {
                var A100 = GenerateFixedData(100, (byte)'A');
                var M100 = GenerateFixedData(100, (byte)'M');
                var Y100 = GenerateFixedData(100, (byte)'Y');

                block.Position = -1;
                block.Write(A100);
                mpos = (int)block.Position + 1;
                block.Position = -1;
                block.Write(M100);
                ypos = (int)block.Position + 1;
                block.Position = -1;
                block.Write(Y100);

            }

            using (var file = File.Open(file3Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var block = new FileBlockStream(file, 50))
            {
                var A100back = new byte[100];
                block.Position = apos;
                block.Read(A100back);

                var M100back = new byte[100];
                block.Position = ypos;
                block.Read(A100back);

                var Y100back = new byte[100];
                block.Position = ypos;
                block.Read(Y100back);
                //var A100back = block.Read()
                //var M100back = await file.Read(100);
                //var Y100back = await file.Read(100);

                //TestArrayFullOf(A100back, 100, (byte)'A');
                //TestArrayFullOf(M100back, 100, (byte)'M');
                //TestArrayFullOf(Y100back, 100, (byte)'Y');

                //var M100backAgain = await file.Read(100, 100);
                //TestArrayFullOf(M100backAgain, 100, (byte)'M');
            }
        }

        private byte[] GenerateFixedData(int length, byte data)
        {
            return Enumerable.Range(0, length)
                .Select(a => data)
                .ToArray();
        }
    }

    public class FileBlockStream : BlockStream
    {
        public List<bool> BlocksFree { get; set; }

        public FileBlockStream(Stream ioStream, int blockSize)
            : base(ioStream, blockSize)
        {
            BlocksFree = new List<bool>();
        }

        public override long GetFreeBlockPosition()
        {
            var i = 0;
            var hasFree = BlocksFree
                .Select(b => new { i = i++, free = b })
                .FirstOrDefault(b => b.free);
            if (hasFree == null)
            {
                var pos = BlocksFree.Count;
                BlocksFree.Add(false);
                return pos;
            }
            else
            {
                BlocksFree[hasFree.i] = false;
                return hasFree.i;
            }
        }
    }
}