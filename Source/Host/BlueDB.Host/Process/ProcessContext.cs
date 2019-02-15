using BlueDB.Communication.Messages;
using BlueDB.Communication.Socket;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Process
{
    public class ProcessContext
    {
        public SocketServerConnection Connection { get; set; }
        public MessageRequest Request { get; set; }
        public Action<MessageReponse> Callback { get; set; }
        public string[] TablesToLock { get; set; }
        public string[] TablesToRead { get; set; }
    }
}