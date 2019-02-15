using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Data
{
    public class DataProperty
    {
        public DataType Type { get; set; }
        public object Value { get; set; }

        public DataProperty(DataType type, object value)
        {
            Type = type;
            Value = value;
        }
    }
}