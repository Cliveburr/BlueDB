using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class DictionaryProvider : IProvider
    {
        public bool Test(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        public ISerializeType GetSerializeType(Type type)
        {
            var genericNowType = typeof(DictionaryType<>).MakeGenericType(type);
            return (ISerializeType)Activator.CreateInstance(genericNowType);
        }
    }

    public class DictionaryType<T> : SerializeType<T>
    {
        private Type _type;
        private Type _keyElementType;
        private ISerializeType _keyElementSerialize;
        private Type _valueElementType;
        private ISerializeType _valueElementSerialize;


        public override void Initialize()
        {
            _type = typeof(T);

            _keyElementType = _type.GetGenericArguments()[0];
            _keyElementSerialize = BinarySerialize.From(_keyElementType);

            _valueElementType = _type.GetGenericArguments()[1];
            _valueElementSerialize = BinarySerialize.From(_valueElementType);
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                var dic = value as System.Collections.IDictionary;

                writer.Write((uint)(dic.Count + 1));

                var enumerable = dic.GetEnumerator();
                enumerable.Reset();

                while (enumerable.MoveNext())
                {
                    _keyElementSerialize.Serialize(writer, enumerable.Key);

                    _valueElementSerialize.Serialize(writer, enumerable.Value);
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
                
                var dic = Activator.CreateInstance(_type) as System.Collections.IDictionary;

                for (var i = 0; i < length; i++)
                {
                    var key = _keyElementSerialize.Deserialize(reader);
                    var value = _valueElementSerialize.Deserialize(reader);

                    dic.Add(key, value);
                }

                return dic;
            }
        }

        public override int CalculateSize()
        {
            return _keyElementSerialize.CalculateSize() + _valueElementSerialize.CalculateSize();
        }
    }
}