using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class ArrayProvider : IProvider
    {
        public bool Test(Type type)
        {
            return type.IsArray;
        }

        public ISerializeType GetSerializeType(Type type)
        {
            var genericNowType = typeof(ArrayType<>).MakeGenericType(type);
            return (ISerializeType)Activator.CreateInstance(genericNowType);
        }
    }

    public class ArrayType<T> : SerializeType<T>
    {
        private Type _elementType;
        private ISerializeType _elementSerialize;

        public override void Initialize()
        {
            var type = typeof(T);
            _elementType = type.GetElementType();
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
                var array = value as Array;
                
                writer.Write((uint)(array.Length + 1));

                for (var i = 0; i < array.Length; i++)
                {
                    _elementSerialize.Serialize(writer, array.GetValue(i));
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