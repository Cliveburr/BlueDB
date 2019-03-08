using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Serialize
{
    public interface ISerializeType
    {
        void Initialize();
        void Serialize(BinaryWriter writer, object value);
        object Deserialize(BinaryReader reader);
        int CalculateSize();
    }

    public abstract class SerializeType<T> : ISerializeType
    {
        public abstract void Serialize(BinaryWriter writer, object value);
        public abstract object Deserialize(BinaryReader reader);
        public abstract int CalculateSize();

        public virtual void Initialize()
        {
        }

        public byte[] Serialize(T obj)
        {
            using (var memoryStream = new MemoryStream())
            using (var writeStream = new BinaryWriter(memoryStream))
            {
                Serialize(writeStream, obj);

                return memoryStream.ToArray();
            }
        }

        public T Deserialize(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            using (var readerStream = new BinaryReader(memoryStream))
            {
                return (T)Deserialize(readerStream);
            }
        }
    }
}