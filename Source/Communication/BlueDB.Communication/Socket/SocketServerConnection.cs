using BlueDB.Communication.Messages;
using BlueDB.Serialize;
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

        private Dictionary<string, object> _bag;

        public SocketServerConnection(System.Net.Sockets.Socket socket, SocketServer server)
            : base(socket)
        {
            Server = server;
            _bag = new Dictionary<string, object>();
        }

        public void Start()
        {
            BeginReceive();
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

        protected override void OnReceived(byte[] bytes)
        {
            if (bytes.Length > 0)
            {
                var serializeType = BinarySerialize.From<MessageRequest>();
                var request = serializeType.Deserialize(bytes);

                Server.BeginMessageProcess?.Invoke(this, request, MessageProcessCallback);
            }
        }

        private void MessageProcessCallback(MessageReponse response)
        {
            var serializeType = BinarySerialize.From<MessageReponse>();
            var bytes = serializeType.Serialize(response);

            Send(bytes);
        }

        protected override void OnSent(byte[] bytes)
        {
        }

        protected override void HandleSendError(Exception err)
        {
            //TODO:
        }

        public void SetItemInBag(string tag, object item)
        {
            if (_bag.ContainsKey(tag))
            {
                _bag.Remove(tag);
            }
            _bag.Add(tag, item);
        }

        public T GetItemInBag<T>(string tag)
        {
            if (_bag.ContainsKey(tag))
            {
                return (T)_bag[tag];
            }
            else
            {
                return default(T);
            }
        }

        public bool ContainsItemInBag(string tag)
        {
            return _bag.ContainsKey(tag);
        }

        public void RemoteItemInBag(string tag)
        {
            _bag.Remove(tag);
        }

        public void CleanBag()
        {
            _bag = new Dictionary<string, object>();
        }
    }
}