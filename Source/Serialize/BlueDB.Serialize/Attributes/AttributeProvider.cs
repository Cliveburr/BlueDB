using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueDB.Serialize.Attributes
{
    public abstract class SerializeTypeAttribute : Attribute
    {
        public abstract ISerializeType GetSerializeType(Type type);
    }

    public class AttributeProvider : IProvider
    {
        public bool Test(Type type)
        {
            return type.GetCustomAttributes(typeof(SerializeTypeAttribute), true)
                .Any();
        }

        public ISerializeType GetSerializeType(Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(SerializeTypeAttribute), true)
                .First() as SerializeTypeAttribute;

            return attribute.GetSerializeType(type);
        }
    }
}