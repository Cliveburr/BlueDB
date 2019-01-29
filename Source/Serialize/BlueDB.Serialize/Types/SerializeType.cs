using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public abstract class SerializeType
    {
        public abstract bool Test(Type type);
        public abstract void Serialize(BinaryWriter writer, object value);
        public abstract object Deserialize(BinaryReader reader, Type type);
    }

    public abstract class SerializeType<T> : SerializeType
    {
        public override abstract bool Test(Type type);
        public override abstract void Serialize(BinaryWriter writer, object value);
        public override abstract object Deserialize(BinaryReader reader, Type type);

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
                return (T)Deserialize(readerStream, typeof(T));
            }
        }
    }
}