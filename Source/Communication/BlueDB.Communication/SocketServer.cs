using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace BlueDB.Communication
{
    public class SocketServer
    {
        public IPHostEntry IPHostEntry { get; private set; }
        public IPEndPoint IPEndPoint { get; private set; }
        public Socket Listener { get; private set; }
        public List<SocketClientHandler> Clients { get; private set; }
        public int BufferSize { get; set; }

        public SocketServer()
        {
            Clients = new List<SocketClientHandler>();
            BufferSize = 1024;
        }

        public void Start(int port)
        {
            IPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = IPHostEntry.AddressList[0];

            Listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint = new IPEndPoint(ipAddress, port);
            Listener.Bind(IPEndPoint);
            Listener.Listen(100);

            Listener.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        public void Stop()
        {
            if (Listener?.Connected ?? false)
            {
                Listener.Close();
                IPHostEntry = null;
                IPEndPoint = null;
                Listener = null;
            }
            Clients.ForEach(c => c.Stop());
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            var handler = Listener.EndAccept(ar);

            Listener.BeginAccept(new AsyncCallback(AcceptCallback), null);

            var clientHandler = new SocketClientHandler(handler, this);
            clientHandler.OnClose += OnClientHandler_Close;
            Clients.Add(clientHandler);

            clientHandler.Start();
        }

        private void OnClientHandler_Close(SocketClientHandler sender)
        {
            Clients.Remove(sender);
        }
    }
}