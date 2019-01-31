using BlueDB.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BlueDB.DriverNET.ClientTest
{
    class Program
    {
        private static Socket client;

        static void Main(string[] args)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8011);

            client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), null);

            Console.ReadKey();
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                client.EndConnect(ar);

                var message = GetTestMessage();
                var package = message.Packages[message.Index];

                client.BeginSend(package, 0, package.Length, 0, new AsyncCallback(SendCallback), message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Message message = (Message)ar.AsyncState;

                int bytesSent = client.EndSend(ar);

                message.Index++;
                if (message.Index < message.Packages.Count)
                {
                    var package = message.Packages[message.Index];

                    client.BeginSend(package, 0, package.Length, 0, new AsyncCallback(SendCallback), message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static byte[] GenTestMsg()
        {
            using (var memoryStream = new MemoryStream())
            using (var writeStream = new BinaryWriter(memoryStream))
            {
                var length = (ushort)1000;
                var hasMore = false;

                writeStream.Write((ushort)22); // message id

                writeStream.Write(SocketMessage.SetPackageSettings(length, hasMore)); // package settings

                var rnd = new Random((int)DateTime.Now.Ticks);

                writeStream.Write(Enumerable.Range(0, length)
                    .Select(n => (byte)rnd.Next(0, 256))
                    .ToArray());

                return memoryStream.ToArray();
            }
        }

        private static Message GetTestMessage()
        {
            var rnd = new Random((int)DateTime.Now.Ticks);

            var bytes = Enumerable.Range(0, 100000)
                    .Select(n => (byte)rnd.Next(0, 256))
                    .ToArray();

            return new Message(bytes);
        }
    }

    public class Message
    {
        private static int _idCount;

        public List<byte[]> Packages { get; set; }
        public int Index { get; set; }
        public int Id { get; private set; }

        public Message(byte[] bytes)
        {
            Index = 0;
            Id = ++_idCount;

            SplitInPackages(bytes);
        }

        private void SplitInPackages(byte[] bytes)
        {
            Packages = new List<byte[]>();
            var left = bytes.Length;
            var index = 0;

            using (var memoryStream = new MemoryStream())
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write((ushort)Id);

                while (left > 0)
                {
                    var lenght = Math.Min(SocketMessage.MaxSettingsLenght(), left);
                    left -= lenght;
                    var hasMore = left > 0;

                    var settings = SocketMessage.SetPackageSettings((ushort)lenght, hasMore);

                    writer.Write(settings);

                    writer.Write(bytes, index, lenght);
                    index += lenght;

                    Packages.Add(memoryStream.ToArray());
                    memoryStream.Position = 0;
                    memoryStream.SetLength(0);
                }
            }
        }
    }
}
