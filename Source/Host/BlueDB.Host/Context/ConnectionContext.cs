using BlueDB.Communication.Messages;
using BlueDB.Communication.Socket;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Host.Context
{
    public class ConnectionContext
    {
        // Cloned


        // Not Cloned
        public SocketServerConnection Connection { get; set; }
        public MessageRequest Request { get; set; }
        public Action<SendMessage> Callback { get; set; }

        public ConnectionContext Clone()
        {
            return new ConnectionContext
            {

            };
        }
    }
}