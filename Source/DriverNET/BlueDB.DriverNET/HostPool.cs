using BlueDB.Communication.Socket;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlueDB.DriverNET
{
    public class HostPool
    {
        public IPEndPoint EndPoint { get; internal set; }
        public SocketClient Client { get; internal set; }

        private ManualResetEvent _connectSync;

        public HostPool(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            Client = new SocketClient(endPoint);

            _connectSync = new ManualResetEvent(false);
            Client.Connect(new Action(ConnectCallback));
            _connectSync.WaitOne();
        }

        private void ConnectCallback()
        {
            _connectSync.Set();
        }
    }
}