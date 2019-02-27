using BlueDB.Serialize.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Serialize.Tests.AttributeProvider
{
#pragma warning disable
    [Serializable]
    public class AttributeProviderPropertyEntity
    {
        [FixedStringType(100)]
        public string TestSomeString { get; set; }
    }

    public class FixedStringTypeAttribute : SerializeTypeAttribute
    {
        private int _size;

        public FixedStringTypeAttribute(int size)
        {
            _size = size;
        }

        public override ISerializeType GetSerializeType(Type type)
        {
            if (type.FullName != "System.String")
            {
                throw new Exception("FixedStringTypeAttribute only be apply to string properties!");
            }

            return new FixedStringSerializeEntity(_size);
        }
    }

    public class FixedStringSerializeEntity : SerializeType<string>
    {
        private int _size;

        public FixedStringSerializeEntity(int size)
        {
            _size = size;
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            var stringValue = value as string;

            var bytes = Encoding.UTF8.GetBytes(stringValue);
            var fixedBytes = new byte[_size];
            System.Array.Copy(bytes, fixedBytes, bytes.Length);

            writer.Write(fixedBytes);
        }

        public override object Deserialize(BinaryReader reader, Type type)
        {
            var bytes = reader.ReadBytes(_size);
            return Encoding.UTF8.GetString(bytes);
        }
    }
#pragma warning restore
}