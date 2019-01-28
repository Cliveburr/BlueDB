using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Serialize
{
    public class SerializeObjectGenericType<T> : SerializeObjectType
    {
        public SerializeObjectGenericType(Type type)
            : base(type)
        {
        }

        public byte[] Serialize(T obj)
        {
            return base.Serialize(obj);
        }

        public new T Deserialize(byte[] bytes)
        {
            return (T)base.Deserialize(bytes);
        }
    }
}