using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class UInt16KnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return propertyType.FullName == "System.UInt16";
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new UInt16Property(propertyInfo);
        }
    }

    public class UInt16Property : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;

        public UInt16Property(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = _propertyInfo.GetValue(obj);
            if (value != null)
            {
                writer.Write((ushort)value);
            }
            else
            {
                writer.Write((ushort)0);
            }
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var value = reader.ReadUInt16();
            _propertyInfo.SetValue(obj, value);
        }
    }
}