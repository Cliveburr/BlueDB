using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class Int16KnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return propertyType.FullName == "System.Int16";
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new Int16Property(propertyInfo);
        }
    }

    public class Int16Property : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;

        public Int16Property(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = _propertyInfo.GetValue(obj);
            if (value != null)
            {
                writer.Write((short)value);
            }
            else
            {
                writer.Write((short)0);
            }
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var value = reader.ReadInt16();
            _propertyInfo.SetValue(obj, value);
        }
    }
}