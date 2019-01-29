using BlueDB.Serialize.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class ClassType<T> : SerializeType<T>
    {
        public Tuple<SerializeType, PropertyInfo>[] Properties { get; private set; }
        public Type Type { get; private set; }

        public ClassType()
        {
            Type = typeof(T);

            ReadProperties();
        }

        private void ReadProperties()
        {
            Properties = Type.GetProperties()
                .Select(p => new Tuple<SerializeType, PropertyInfo>(BinarySerialize.From(p.PropertyType), p))
                .ToArray();
        }

        public override bool Test(Type type)
        {
            return type.Equals(Type);
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