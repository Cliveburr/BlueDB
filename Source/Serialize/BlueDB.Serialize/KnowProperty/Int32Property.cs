using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class Int32KnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return propertyType.FullName == "System.Int32";
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new Int32Property(propertyInfo);
        }
    }

    public class Int32Property : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;

        public Int32Property(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = _propertyInfo.GetValue(obj);
            if (value != null)
            {
                writer.Write((int)value);
            }
            else
            {
                writer.Write(0);
            }
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var value = reader.ReadInt32();
            _propertyInfo.SetValue(obj, value);
        }
    }
}