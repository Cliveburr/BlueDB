using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Entity
{
    public class Property
    {
        public PropertyType Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        public Property()
        {
        }

        public Property(PropertyType type, string name, object value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public uint ValueUInt
        {
            get
            {
                return Convert.ToUInt32(Value);
            }
        }
    }
}