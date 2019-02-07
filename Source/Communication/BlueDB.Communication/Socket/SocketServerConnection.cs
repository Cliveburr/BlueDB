using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Communication.Socket
{
    public class SocketServerConnection : BaseSocket
    {
        public delegate void OnCloseDelegate(SocketServerConnection sender);
        public event OnCloseDelegate OnClose;

        public SocketServer Server { get; private set; }


        public SocketServerConnection(System.Net.Sockets.Socket socket, SocketServer server)
            : base(socket)
        {
            Server = server;
        }

        public void Start()
        {
            BeginReceive(new ReceiveMessage
            {
                Buffer = new byte[Server.BufferSize]
            });
        }

        public void Stop()
        {
            try
            {
                Socket?.Close();
            } catch { }
            Socket = null;
            OnClose?.Invoke(this);
        }

        protected override void HandleReceiveError(Exception err)
        {
            //TODO:

            Server.Connections.Remove(this);
        }

        protected override void FinishReceiving(ReceiveMessage message)
        {
            BeginReceive(new ReceiveMessage
            {
                Buffer = new byte[Server.BufferSize]
            });

            if (message.GetBytes.Length > 0)
            {
                Server.BeginMessageProcess?.Invoke(this, message, MessageProcessCallback);
            }
        }

        private void MessageProcessCallback(SendMessage message)
        {
            SendMessage(message);
        }

        protected override void MessageSent(SendMessage message)
        {
        }
    }
}