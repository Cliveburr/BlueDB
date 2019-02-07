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

                BeginReceive(new ReceiveMessage
                {
                    Buffer = new byte[2048]
                });

                connectedCallback();
            }
            catch (Exception e)
            {
                //TODO:

                Console.WriteLine(e.ToString());
            }
        }

        public void SendMessage(MessageData data, Action<MessageData> sentCallback)
        {
            data.Id = ++_messageIdIndex;

            var serialize = BinarySerialize.From<MessageData>();
            var bytes = serialize.Serialize(data);
            var request = new SendMessage(bytes);

            var handle = new MessageHandle
            {
                Id = data.Id,
                Request = data,
                SentCallback = sentCallback
            };
            _handles.Add(data.Id, handle);

            SendMessage(request);
        }

        protected override void MessageSent(SendMessage message)
        {
        }

        protected override void FinishReceiving(ReceiveMessage message)
        {
            var serialize = BinarySerialize.From<MessageData>();
            var response = serialize.Deserialize(message.GetBytes);

            var handle = _handles[response.Id];
            _handles.Remove(response.Id);
            handle.Response = response;

            handle.SentCallback(response);
        }
    }

    public class MessageHandle
    {
        public ushort Id { get; set; }
        public MessageData Request { get; set; }
        public MessageData Response { get; set; }
        public Action<MessageData> SentCallback { get; set; }
    }
}