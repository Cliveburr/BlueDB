using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class DateTimeKnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return propertyType.FullName == "System.DateTime";
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new DateTimeProperty(propertyInfo);
        }
    }

    public class DateTimeProperty : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;

        public DateTimeProperty(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = (DateTime)_propertyInfo.GetValue(obj);
            var valueLong = value.ToBinary();
            writer.Write(valueLong);
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var value = reader.ReadInt64();
            var dateTime =  DateTime.FromBinary(value);
            _propertyInfo.SetValue(obj, dateTime);
        }
    }
}