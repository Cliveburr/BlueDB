using BlueDB.Communication;
using BlueDB.Communication.Messages;
using BlueDB.Communication.Socket;
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
        private static SocketClient client;

        static void Main(string[] args)
        {
            using (var connection = new ClientConnection("127.0.0.1", 8011))
            {
                var data = GetTestMessage();

                connection.Client.SendMessage(data, (response) =>
                {

                    for (var i = 0; i < data.Bytes.Length; i++)
                    {
                        if (data.Bytes[i] != response.Bytes[i])
                        {
                            throw new Exception();
                        }
                    }
                });

            }



            //var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //var ipAddress = ipHostInfo.AddressList[0];
            //var remoteEP = new IPEndPoint(ipAddress, 8011);

            //client = new SocketClient(remoteEP);

            //client.Connect(ConnectCallback);

            Console.ReadKey();
        }

        private static void ConnectCallback()
        {
            var data = GetTestMessage();

            client.SendMessage(data, (response) =>
            {

                for (var i = 0; i < data.Bytes.Length; i++)
                {
                    if (data.Bytes[i] != response.Bytes[i])
                    {
                        throw new Exception();
                    }
                }
            });
        }

        private static MessageData GetTestMessage()
        {
            var rnd = new Random((int)DateTime.Now.Ticks);

            var bytes = Enumerable.Range(0, 100000)
                    .Select(n => (byte)rnd.Next(0, 256))
                    .ToArray();

            return new MessageData
            {
                TextView = "um textinho teste!",
                Bytes = bytes
            };
        }
    }
}