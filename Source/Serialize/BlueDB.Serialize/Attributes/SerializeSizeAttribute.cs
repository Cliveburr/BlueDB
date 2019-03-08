using System;

namespace BlueDB.Serialize.Attributes
{
    public class SerializeSizeAttribute : Attribute
    {
        public int Size { get; private set; }

        public SerializeSizeAttribute(int size)
        {
            Size = size;
        }
    }
}