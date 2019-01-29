using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class ArrayKnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return propertyType.IsArray;
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new ArrayProperty(propertyInfo);
        }
    }

    public class ArrayProperty : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;

        public ArrayProperty(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = _propertyInfo.GetValue(obj);
            if (value == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                var array = value as Array;

                writer.Write((uint)(array.Length + 1));

                //_serialize.Serialize(writer, value); todo
            }
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var value = reader.ReadByte();
            _propertyInfo.SetValue(obj, value);
        }
    }
}