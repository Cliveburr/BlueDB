using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class Int64KnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return propertyType.FullName == "System.Int64";
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new Int64Property(propertyInfo);
        }
    }

    public class Int64Property : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;

        public Int64Property(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = _propertyInfo.GetValue(obj);
            if (value != null)
            {
                writer.Write((long)value);
            }
            else
            {
                writer.Write((long)0);
            }
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var value = reader.ReadInt64();
            _propertyInfo.SetValue(obj, value);
        }
    }
}