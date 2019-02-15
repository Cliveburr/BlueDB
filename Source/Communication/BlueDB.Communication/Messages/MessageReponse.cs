using BlueDB.Communication.Messages.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Messages
{
    public class MessageReponse
    {
        public ushort Id { get; set; }
        public IResults[] Results { get; set; }
        public string Error { get; set; }
    }
}