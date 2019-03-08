using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class EnumProvider : IProvider
    {
        public bool Test(Type type)
        {
            return type.IsEnum;
        }

        public ISerializeType GetSerializeType(Type type)
        {
            var genericNowType = typeof(EnumType<>).MakeGenericType(type);
            return (ISerializeType)Activator.CreateInstance(genericNowType);
        }
    }

    public class EnumType<T> : SerializeType<T>
    {
        private ISerializeType _underSerialize;

        public override void Initialize()
        {
            var type = typeof(T);
            var underType = Enum.GetUnderlyingType(type);
            _underSerialize = BinarySerialize.From(underType);
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            _underSerialize.Serialize(writer, value);
        }

        public override object Deserialize(BinaryReader reader)
        {
            return _underSerialize.Deserialize(reader);
        }

        public override int CalculateSize()
        {
            return _underSerialize.CalculateSize();
        }
    }
}