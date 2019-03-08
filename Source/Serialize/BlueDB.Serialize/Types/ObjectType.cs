using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Serialize.Types
{
    public class ObjectProvider : IProvider
    {
        private ObjectType _instance;

        public bool Test(Type type)
        {
            return type.FullName == "System.Object";
        }

        public ISerializeType GetSerializeType(Type type)
        {
            if (_instance == null)
            {
                _instance = new ObjectType();
            }
            return _instance;
        }
    }

    public class ObjectType : SerializeType<object>
    {
        public override void Serialize(BinaryWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                var valueType = value.GetType();
                var serialize = BinarySerialize.From(valueType);

                var fullNameValue = value.GetType().AssemblyQualifiedName;
                var fullNamevalueBytes = Encoding.UTF8.GetBytes(fullNameValue);
                writer.Write((uint)(fullNamevalueBytes.Length + 1));
                writer.Write(fullNamevalueBytes);

                serialize.Serialize(writer, value);
            }
        }

        public override object Deserialize(BinaryReader reader)
        {
            var length = reader.ReadUInt32();

            if (length == 0)
            {
                return null;
            }
            else
            {
                length--;
                var fullNameValueBytes = reader.ReadBytes((int)length);
                var fullNameValue = Encoding.UTF8.GetString(fullNameValueBytes);

                var valueType = Type.GetType(fullNameValue);
                if (valueType == null)
                {
                    throw new Exception(string.Format("Type not found: \"{0}\"!", fullNameValue));
                }

                var serialize = BinarySerialize.From(valueType);

                return serialize.Deserialize(reader);
            }
        }

        public override int CalculateSize()
        {
            return -1;
        }
    }
}