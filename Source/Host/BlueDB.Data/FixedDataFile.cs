using BlueDB.Serialize;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Data
{
    public class FixedDataFile<T> : SystemFile, IEnumerable, IEnumerable<T> where T : class
    {
        private SerializeType<T> _serialize;
        private int _serializeSize;

        public FixedDataFile(string path)
            : base(path)
        {
            SetSerialize();
        }

        private void SetSerialize()
        {
            _serialize = BinarySerialize.From<T>();
            _serializeSize = _serialize.CalculateSize();
            if (_serializeSize == -1)
            {
                throw new Exception("All properties must be fixed size to be used in FixedDataFile!");
            }
        }

        public new T Read(int pos)
        {
            _file.Position = (pos * _serializeSize);
            using (var reader = new BinaryReader(_file, Encoding.UTF8, true))
            {
                return _serialize.Deserialize(reader) as T;
            }
        }

        public void Write(int pos, T data)
        {
            _file.Position = (pos * _serializeSize);
            CheckFileLength(_file.Position + _serializeSize);
            using (var writer = new BinaryWriter(_file, Encoding.UTF8, true))
            {
                _serialize.Serialize(writer, data);
            }
        }

        public int Count
        {
            get
            {
                return (int)(_file.Length / _serializeSize);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new FixedDataFileEnumerator<T>(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new FixedDataFileEnumerator<T>(this);
        }
    }
}