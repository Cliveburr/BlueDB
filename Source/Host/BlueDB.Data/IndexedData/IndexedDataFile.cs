using BlueDB.Data.DataBlock;
using BlueDB.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Data.IndexedData
{
    public class IndexedDataFile<T> : IDisposable where T : class
    {
        public string DataFilePath { get; private set; }
        public string IndexFilePath { get; private set; }
        public int BlockSize { get; private set; }

        private SerializeType<T> _serialize;

        protected BlockStream _dataFile;
        protected FileStream _indexFile;

        public IndexedDataFile(string path, int blockSize)
        {
            BlockSize = blockSize;

            Open(path);
            SetSerialize();
        }

        private void Open(string path)
        {
            DataFilePath = path + ".data";
            IndexFilePath = path + ".index0";

            var fileStream = File.Open(DataFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //_dataFile = new BlockStream(fileStream, BlockSize);
            _indexFile = File.Open(IndexFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }
        private void SetSerialize()
        {
            _serialize = BinarySerialize.From<T>();
        }

        public void Dispose()
        {
            _dataFile?.Flush();
            _dataFile?.Close();
            _dataFile?.Dispose();
            _dataFile = null;

            _indexFile?.Flush();
            _indexFile?.Close();
            _indexFile?.Dispose();
            _indexFile = null;
        }
    }
}