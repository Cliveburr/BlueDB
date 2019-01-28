using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlueDB.Serialize
{
    public class SerializeObjectType
    {
        public Type ObjectType { get; private set; }
        public ISerializeObjectProperty[] Properties { get; private set; }

        public SerializeObjectType(Type type)
        {
            ObjectType = type;

            ReadProperties();
        }

        private void ReadProperties()
        {
            Properties = ObjectType.GetProperties()
                .Select(p => CreateProperty(p))
                .ToArray();
        }

        private ISerializeObjectProperty CreateProperty(PropertyInfo property)
        {
            var firstProperty = BinarySerialize.KnowProperties
                .Where(kp => kp.Test(property.PropertyType))
                .FirstOrDefault();

            if (firstProperty == null)
            {
                throw new NotSupportedException($"Property of type \"{property.PropertyType.FullName}\" on object \"{property.DeclaringType.FullName}\" not supported for this serialize!");
            }

            return firstProperty.Create(property);
        }

        public byte[] Serialize(object obj)
        {
            using (var memoryStream = new MemoryStream())
            using (var writeStream = new BinaryWriter(memoryStream))
            {
                foreach (var property in Properties)
                {
                    property.Serialize(writeStream, obj);
                }

                return memoryStream.GetBuffer();
            }
        }

        public object Deserialize(byte[] bytes)
        {
            var obj = Activator.CreateInstance(ObjectType);

            using (var memoryStream = new MemoryStream(bytes))
            using (var readerStream = new BinaryReader(memoryStream))
            {
                foreach (var property in Properties)
                {
                    property.Deserialize(readerStream, obj);
                }

                return obj;
            }
        }
    }
}