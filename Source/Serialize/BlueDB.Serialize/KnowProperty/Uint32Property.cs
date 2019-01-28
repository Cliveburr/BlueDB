using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class UInt32KnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return propertyType.FullName == "System.UInt32";
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new UInt32Property(propertyInfo);
        }
    }

    public class UInt32Property : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;

        public UInt32Property(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = _propertyInfo.GetValue(obj);
            if (value != null)
            {
                writer.Write((uint)value);
            }
            else
            {
                writer.Write((uint)0);
            }
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var value = reader.ReadUInt32();
            _propertyInfo.SetValue(obj, value);
        }
    }
}