using BlueDB.Communication.Messages;
using BlueDB.Serialize;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BlueDB.Communication.Socket
{
    public class SocketClient : BaseSocket
    {
        private ushort _messageIdIndex;
        private IPEndPoint _ipEndPoint;
        private Dictionary<ushort, MessageHandle> _handles;

        public SocketClient(IPEndPoint ipEndPoint)
            : base(new System.Net.Sockets.Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
        {
            _messageIdIndex = 0;
            _ipEndPoint = ipEndPoint;
            _handles = new Dictionary<ushort, MessageHandle>();
        }

        public bool IsConnected { get { return Socket.Connected; } }

        public void Connect(Action connectedCallback)
        {
            Socket.BeginConnect(_ipEndPoint, new AsyncCallback(ConnectCallback), connectedCallback);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                var connectedCallback = ar.AsyncState as Action;

                Socket.EndConnect(ar);

                BeginReceive();

                connectedCallback();
            }
            catch (Exception e)
            {
                //TODO:

                Console.WriteLine(e.ToString());
            }
        }

        public void SendMessage(MessageRequest request, Action<MessageReponse> sentCallback)
        {
            request.Id = ++_messageIdIndex;

            var serializeType = BinarySerialize.From<MessageRequest>();
            var bytes = serializeType.Serialize(request);

            var handle = new MessageHandle
            {
                Id = request.Id,
                Request = request,
                SentCallback = sentCallback
            };
            _handles.Add(request.Id, handle);

            Send(bytes);
        }

        protected override void OnSent(byte[] bytes)
        {
        }

        protected override void OnReceived(byte[] bytes)
        {
            var serializeType = BinarySerialize.From<MessageReponse>();
            var response = serializeType.Deserialize(bytes);

            var handle = _handles[response.Id];
            _handles.Remove(response.Id);
            handle.Response = response;

            handle.SentCallback(response);
        }
    }

    public class MessageHandle
    {
        public ushort Id { get; set; }
        public MessageRequest Request { get; set; }
        public MessageReponse Response { get; set; }
        public Action<MessageReponse> SentCallback { get; set; }
    }
}