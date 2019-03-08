using BlueDB.Serialize.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Serialize.Tests.AttributeProvider
{
#pragma warning disable
    public class CustomSerializeTypeAttribute : SerializeTypeAttribute
    {
        public override ISerializeType GetSerializeType(Type type)
        {
            return new CustomSerializeType();
        }
    }

    public class CustomSerializeType : SerializeType<AttributeProviderEntity>
    {

        public override void Serialize(BinaryWriter writer, object value)
        {
            var obj = value as AttributeProviderEntity;

            writer.Write(obj.SomeAnyValue);
        }

        public override object Deserialize(BinaryReader reader)
        {
            var someAnyValue = reader.ReadInt32();

            return new AttributeProviderEntity
            {
                SomeAnyValue = someAnyValue
            };
        }

        public override int CalculateSize()
        {
            return 2;
        }
    }

    [Serializable]
    [CustomSerializeType]
    public class AttributeProviderEntity
    {
        public int SomeAnyValue { get; set; }
    }
#pragma warning restore
}