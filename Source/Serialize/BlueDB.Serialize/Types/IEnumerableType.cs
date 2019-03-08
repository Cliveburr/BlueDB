using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class IEnumerableProvider : IProvider
    {
        public bool Test(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        public ISerializeType GetSerializeType(Type type)
        {
            var genericNowType = typeof(IEnumerableType<>).MakeGenericType(type);
            return (ISerializeType)Activator.CreateInstance(genericNowType);
        }
    }

    public class IEnumerableType<T> : SerializeType<T>
    {
        private Type _elementType;
        private ISerializeType _elementSerialize;

        public override void Initialize()
        {
            var type = typeof(T);
            _elementType = type.GenericTypeArguments[0];
            _elementSerialize = BinarySerialize.From(_elementType);
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                var ienum = value as IEnumerable;

                var count = 0;

                using (var memoryStream = new MemoryStream())
                using (var writeStream = new BinaryWriter(memoryStream))
                {
                    foreach (var item in ienum)
                    {
                        count++;
                        _elementSerialize.Serialize(writeStream, item);
                    }

                    writer.Write((uint)(count + 1));
                    writer.Write(memoryStream.ToArray());
                }
            }
        }

        public override object Deserialize(BinaryReader reader)
        {
            var length = reader.ReadUInt32();

            if (length == 0)
            {
                return null;
            }
            else
            {
                length--;
                var obj = Array.CreateInstance(_elementType, length) as Array;

                for (var i = 0; i < length; i++)
                {
                    obj.SetValue(_elementSerialize.Deserialize(reader), i);
                }

                return obj;
            }
        }

        public override int CalculateSize()
        {
            return _elementSerialize.CalculateSize();
        }
    }
}