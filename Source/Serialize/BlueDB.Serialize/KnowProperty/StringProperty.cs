using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class StringKnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return propertyType.FullName == "System.String";
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new StringProperty(propertyInfo);
        }
    }

    public class StringProperty : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;

        public StringProperty(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = _propertyInfo.GetValue(obj) as string;

            if (value == null)
            {
                writer.Write((uint)0);
            }
            else if (value == string.Empty)
            {
                writer.Write((uint)1);
            }
            else
            {
                writer.Write((uint)(value.Length + 1));
                writer.Write(Encoding.UTF8.GetBytes(value));  //BinaryStringFormat.NoPrefixOrTermination
            }
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var length = reader.ReadUInt32();

            if (length == 0)
            {
                _propertyInfo.SetValue(obj, null);
            }
            else if (length == 1)
            {
                _propertyInfo.SetValue(obj, string.Empty);
            }
            else
            {
                var bytes = reader.ReadBytes((int)(length - 1));

                var a = bytes.Where(b => b == 0).ToArray();

                _propertyInfo.SetValue(obj, Encoding.UTF8.GetString(bytes));

            }
        }
    }
}