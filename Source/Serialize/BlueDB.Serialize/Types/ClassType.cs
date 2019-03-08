using BlueDB.Serialize.Attributes;
using BlueDB.Serialize.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class ClassProvider : IProvider
    {
        public bool Test(Type type)
        {
            return true;
        }

        public ISerializeType GetSerializeType(Type type)
        {
            var genericNowType = typeof(ClassType<>).MakeGenericType(type);
            return (ISerializeType)Activator.CreateInstance(genericNowType);
        }
    }

    public class ClassType<T> : SerializeType<T>
    {
        public Tuple<ISerializeType, PropertyInfo>[] Properties { get; private set; }
        public Type Type { get; private set; }

        public override void Initialize()
        {
            Type = typeof(T);

            ReadProperties();
        }

        private void ReadProperties()
        {
            Properties = Type.GetProperties()
                .Where(p => p.CanWrite && p.CanRead)
                .Select(p => ReadSerializeFromProperty(p))
                .ToArray();
        }

        private Tuple<ISerializeType, PropertyInfo> ReadSerializeFromProperty(PropertyInfo property)
        {
            if (property.GetCustomAttributes(typeof(SerializeTypeAttribute), true)
                .FirstOrDefault() is SerializeTypeAttribute attribute)
            {
                var serialize = attribute.GetSerializeType(property.PropertyType);

                serialize.Initialize();

                return new Tuple<ISerializeType, PropertyInfo>(serialize, property);
            }
            else
            {
                return new Tuple<ISerializeType, PropertyInfo>(BinarySerialize.From(property.PropertyType), property);
            }
        }

        public override void Serialize(BinaryWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write((byte)0);
            }
            else
            {
                writer.Write((byte)1);

                foreach (var property in Properties)
                {
                    var propValue = property.Item2.GetValue(value);

                    property.Item1.Serialize(writer, propValue);
                }
            }
        }

        public override object Deserialize(BinaryReader reader)
        {
            var value = reader.ReadByte();

            switch (value)
            {
                case 1:
                    {
                        var obj = Activator.CreateInstance(Type);

                        foreach (var property in Properties)
                        {
                            var propValue = property.Item1.Deserialize(reader);

                            property.Item2.SetValue(obj, propValue);
                        }

                        return obj;
                    }
                default:
                    return null;
            }
        }

        public override int CalculateSize()
        {
            var sum = 0;
            foreach (var property in Properties)
            {
                if (property.Item2.GetCustomAttributes(typeof(SerializeSizeAttribute), true)
                    .FirstOrDefault() is SerializeSizeAttribute attribute)
                {
                    var size = attribute.Size;
                    if (typeof(string).Equals(property.Item2.PropertyType))
                    {
                        sum += 2 + size;
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(property.Item2.PropertyType))
                    {
                        var value = property.Item1.CalculateSize();
                        if (value == -1)
                        {
                            return -1;
                        }
                        else
                        {
                            sum += 2 + (value * size);
                        }
                    }
                    else
                    {
                        sum += size;
                    }
                }
                else
                {
                    var value = property.Item1.CalculateSize();
                    if (value == -1)
                    {
                        return -1;
                    }
                    else
                    {
                        sum += value;
                    }
                }
            }
            return sum;
        }
    }
}