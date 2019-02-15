using BlueDB.Communication.Messages;
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

        public HostPool(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            Client = new SocketClient(endPoint);
        }

        public void Release()
        {
            HostStorage.Release(this);
        }

        private void CheckConnect(Action callBack)
        {
            if (Client.IsConnected)
            {
                callBack();
            }
            else
            {
                Client.Connect(callBack);
            }
        }

        public void SendMessage(MessageRequest data, Action<MessageReponse> callBack)
        {
            CheckConnect(() =>
            {
                Client.SendMessage(data, callBack);
            });
        }
    }
}