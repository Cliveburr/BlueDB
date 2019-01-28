using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class UInt64KnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return propertyType.FullName == "System.UInt64";
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new UInt64Property(propertyInfo);
        }
    }

    public class UInt64Property : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;

        public UInt64Property(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = _propertyInfo.GetValue(obj);
            if (value != null)
            {
                writer.Write((ulong)value);
            }
            else
            {
                writer.Write((ulong)0);
            }
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var value = reader.ReadUInt64();
            _propertyInfo.SetValue(obj, value);
        }
    }
}