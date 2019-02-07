using BlueDB.Communication.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages
{
    public class MessageRequest : MessageData
    {
        public ICommand[] Commands { get; set; }
    }
}