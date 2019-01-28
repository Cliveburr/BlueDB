using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class ByteKnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return propertyType.FullName == "System.Byte";
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new ByteProperty(propertyInfo);
        }
    }

    public class ByteProperty : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;

        public ByteProperty(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = _propertyInfo.GetValue(obj);
            if (value != null)
            {
                writer.Write((byte)value);
            }
            else
            {
                writer.Write((byte)0);
            }
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var value = reader.ReadByte();
            _propertyInfo.SetValue(obj, value);
        }
    }
}