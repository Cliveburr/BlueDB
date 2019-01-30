﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Communication
{
    public class SocketClientHandler
    {
        public delegate void OnCloseDelegate(SocketClientHandler sender);
        public event OnCloseDelegate OnClose;

        public Socket Socket { get; private set; }
        public SocketServer Server { get; private set; }

        public SocketClientHandler(Socket socket, SocketServer server)
        {
            Socket = socket;
            Server = server;
        }

        public void Start()
        {
            BeginReceive(new SocketClientMessage
            {
                Buffer = new byte[Server.BufferSize]
            });
        }

        private void BeginReceive(SocketClientMessage message)
        {
            Socket.BeginReceive(message.Buffer, 0, Server.BufferSize, 0, new AsyncCallback(ReadCallback), message);
        }

        public void Stop()
        {
            OnClose?.Invoke(this);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            var message = (SocketClientMessage)ar.AsyncState;

            var bytesRead = Socket.EndReceive(ar);
            if (bytesRead > 0)
            {
                message.WriteBufferToStream(bytesRead);

                if (message.State == SocketClientMessageState.Receiving)
                {
                    BeginReceive(message);
                }
                else
                {
                    FinishReceiving(message);
                }
            }
            else
            {
                FinishReceiving(message);
            }
        }

        private void FinishReceiving(SocketClientMessage message)
        {
            BeginReceive(new SocketClientMessage
            {
                Buffer = new byte[Server.BufferSize]
            });

            message.State = SocketClientMessageState.Processing;

        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}