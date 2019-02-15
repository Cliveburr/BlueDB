using BlueDB.Communication.Socket;
using BlueDB.Serialize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlueDB.Communication.Tests
{
    [TestClass]
    public class MessageProtocolTest
    {

        [TestMethod]
        public void SingleCommunication()
        {
            var client = new DirectTest();
            var server = new DirectTest();
            var waitReceive = new ManualResetEvent(false);

            var message = DirectTestMessage.Generate();

            server.OnMessage += delegate(DirectTestMessage messageReceived)
            {
                message.Compare(messageReceived);

                waitReceive.Set();
            };

            client.OnSend += delegate (byte[] bytes)
            {
                server.Receive(bytes, bytes.Length);
            };

            client.Send(message);

            waitReceive.WaitOne();
        }

        private struct MultipleCommunicationStruct
        {
            public DirectTest Client;
            public DirectTestMessage Message;
            public bool IsCompleted;
        }

        [TestMethod]
        public void MultipleCommunication()
        {
            var server = new DirectTest();
            var waitReceiveAll = new ManualResetEvent(false);
            var checkReciveLock = new object();

            var clients = Enumerable.Range(0, 2000)
                .Select(i => new MultipleCommunicationStruct
                {
                    Client = new DirectTest(),
                    Message = DirectTestMessage.Generate(),
                    IsCompleted = false
                })
                .ToList();

            server.OnMessage += delegate (DirectTestMessage messageReceived)
            {
                var client = clients.First(c => c.Message.Index == messageReceived.Index);
                client.IsCompleted = true;
                client.Message.Compare(messageReceived);

                lock (checkReciveLock)
                {
                    var notCompleted = clients.Where(c => c.IsCompleted);
                    if (!notCompleted.Any())
                    {
                        waitReceiveAll.Set();
                    }
                }
            };

            clients.ForEach(m => m.Client.OnSend += delegate (byte[] bytes)
            {
                server.Receive(bytes, bytes.Length);
            });

            Parallel.ForEach(clients, (client) =>
            {
                client.Client.Send(client.Message);
            });

            waitReceiveAll.WaitOne();
        }
    }

    public class DirectTest : MessageProtocol
    {
        private static Serialize.Types.SerializeType<DirectTestMessage> _serializeType;

        public delegate void OnMessageDelegate(DirectTestMessage message);
        public event OnMessageDelegate OnMessage;
        public delegate void OnSendDelegate(byte[] bytes);
        public event OnSendDelegate OnSend;

        static DirectTest()
        {
            _serializeType = BinarySerialize.From<DirectTestMessage>();
        }

        public void Send(DirectTestMessage message)
        {
            var bytes = _serializeType.Serialize(message);
            Send(bytes);
        }

        protected override void ProcessSend(byte[] bytes)
        {
            base.EndSend();

            OnSend?.Invoke(bytes);
        }

        protected override void OnReceived(byte[] bytes)
        {
            var message = _serializeType.Deserialize(bytes);
            OnMessage?.Invoke(message);
        }
    }

    public class DirectTestMessage
    {
        public static int IndexCount { get; set; }

        public int Index { get; set; }
        public int SomeValue { get; set; }
        public string Data { get; set; }

        public static DirectTestMessage Generate()
        {
            var rnd = new Random((int)DateTime.Now.Ticks);

            var someValue = rnd.Next(int.MinValue, int.MaxValue);
            var dataLength = rnd.Next(10, 1000);
            var dataBytes = Enumerable.Range(0, dataLength)
                .Select(i => (byte)rnd.Next(byte.MinValue, byte.MaxValue))
                .ToArray();
            var data = Convert.ToBase64String(dataBytes);

            return new DirectTestMessage
            {
                Index = IndexCount++,
                SomeValue = someValue,
                Data = data
            };
        }

        public void Compare(DirectTestMessage from)
        {
            Assert.AreEqual(SomeValue, from.SomeValue);
            Assert.AreEqual(Data.Length, from.Data.Length);

            for (var i = 0; i < Data.Length; i++)
            {
                Assert.AreEqual(Data[i], from.Data[i]);
            }
        }
    }
}