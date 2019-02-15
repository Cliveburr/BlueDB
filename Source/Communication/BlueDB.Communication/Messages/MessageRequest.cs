using BlueDB.Communication.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages
{
    public class MessageRequest
    {
        public ushort Id { get; set; }
        public ICommand[] Commands { get; set; }
    }
}