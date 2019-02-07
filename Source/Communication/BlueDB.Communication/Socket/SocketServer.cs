using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace BlueDB.Communication.Socket
{
    public class SocketServer
    {
        public IPEndPoint IPEndPoint { get; private set; }
        public System.Net.Sockets.Socket Listener { get; private set; }
        public List<SocketServerConnection> Connections { get; private set; }
        public int BufferSize { get; set; }
        public Action<SocketServerConnection, ReceiveMessage, Action<SendMessage>> BeginMessageProcess { get; set; }

        public SocketServer()
        {
            Connections = new List<SocketServerConnection>();
            BufferSize = 1024;
        }

        public void Start(string address, int port)
        {
            Start(IPAddress.Parse(address), port);
        }

        public void Start(IPAddress address, int port)
        {
            Listener = new System.Net.Sockets.Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint = new IPEndPoint(address, port);
            Listener.Bind(IPEndPoint);
            Listener.Listen(100);

            Listener.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        public void Stop()
        {
            if (Listener?.Connected ?? false)
            {
                Listener.Close();
                IPEndPoint = null;
                Listener = null;
            }
            Connections.ForEach(c => c.Stop());
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            var handler = Listener.EndAccept(ar);

            Listener.BeginAccept(new AsyncCallback(AcceptCallback), null);

            var clientHandler = new SocketServerConnection(handler, this);
            clientHandler.OnClose += OnClientHandler_Close;
            Connections.Add(clientHandler);

            clientHandler.Start();
        }

        private void OnClientHandler_Close(SocketServerConnection sender)
        {
            Connections.Remove(sender);
        }
    }
}