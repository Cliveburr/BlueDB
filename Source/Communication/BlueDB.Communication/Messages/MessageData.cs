using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages
{
    public class MessageData
    {
        public ushort Id { get; set; }
        public string TextView { get; set; }
        public byte[] Bytes { get; set; }
    }
}