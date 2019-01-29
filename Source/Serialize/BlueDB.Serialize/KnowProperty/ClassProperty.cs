using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.KnowProperty
{
    public class ClassKnowProperty : ISerilizeObjectKnowProperty
    {
        public bool Test(Type propertyType)
        {
            return !propertyType.IsPrimitive &&
                propertyType.IsClass;
        }

        public ISerializeObjectProperty Create(PropertyInfo propertyInfo)
        {
            return new ClassProperty(propertyInfo);
        }
    }

    public class ClassProperty : ISerializeObjectProperty
    {
        private PropertyInfo _propertyInfo;
        private SerializeObjectType _serialize;

        public ClassProperty(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;

            _serialize = BinarySerialize.From(_propertyInfo.PropertyType);
        }

        public void Serialize(BinaryWriter writer, object obj)
        {
            var value = _propertyInfo.GetValue(obj);
            if (value == null)
            {
                writer.Write((byte)0);
            }
            else
            {
                writer.Write((byte)1);

                _serialize.Serialize(writer, value);
            }
        }

        public void Deserialize(BinaryReader reader, object obj)
        {
            var value = reader.ReadByte();

            switch (value)
            {
                case 0:
                    _propertyInfo.SetValue(obj, null);
                    break;
                case 1:
                    {
                        var deeper = Activator.CreateInstance(_propertyInfo.PropertyType);
                        _serialize.Deserialize(reader, deeper);
                        _propertyInfo.SetValue(obj, deeper);
                        break;
                    }
            }
        }
    }
}