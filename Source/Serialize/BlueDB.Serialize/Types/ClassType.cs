﻿using BlueDB.Serialize.Attributes;
using BlueDB.Serialize.Types;
using System;
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
            var attribute = property.GetCustomAttributes(typeof(SerializeTypeAttribute), true)
                .FirstOrDefault() as SerializeTypeAttribute;
            if (attribute == null)
            {
                return new Tuple<ISerializeType, PropertyInfo>(BinarySerialize.From(property.PropertyType), property);
            }
            else
            {
                var serialize = attribute.GetSerializeType(property.PropertyType);

                serialize.Initialize();

                return new Tuple<ISerializeType, PropertyInfo>(serialize, property);
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

        public override object Deserialize(BinaryReader reader, Type type)
        {
            var value = reader.ReadByte();

            switch (value)
            {
                case 1:
                    {
                        var obj = Activator.CreateInstance(Type);

                        foreach (var property in Properties)
                        {
                            var propValue = property.Item1.Deserialize(reader, property.Item2.PropertyType);

                            property.Item2.SetValue(obj, propValue);
                        }

                        return obj;
                    }
                default:
                    return null;
            }
        }
    }
}