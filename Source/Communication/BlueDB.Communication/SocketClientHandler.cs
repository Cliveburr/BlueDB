using System;
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

        private class MessageState : IDisposable
        {
            public byte[] Buffer { get; set; }

            private MemoryStream _memoryStream;
            private BinaryWriter _binaryWriter;

            public MessageState()
            {
                _memoryStream = new MemoryStream();
                _binaryWriter = new BinaryWriter(_memoryStream);
            }

            public void Dispose()
            {
                _memoryStream?.Dispose();
                _binaryWriter?.Dispose();
            }

            public void WriteBufferToStream(int count)
            {
                _binaryWriter.Write(Buffer, 0, count);
            }
        }

        public SocketClientHandler(Socket socket, SocketServer server)
        {
            Socket = socket;
            Server = server;
        }

        public void Start()
        {
            var state = new MessageState
            {
                Buffer = new byte[Server.BufferSize]
            };

            Socket.BeginReceive(state.Buffer, 0, Server.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public void Stop()
        {
            OnClose?.Invoke(this);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            var state = (MessageState)ar.AsyncState;

            var bytesRead = Socket.EndReceive(ar);
            if (bytesRead > 0)
            {
                state.WriteBufferToStream(bytesRead);

                Socket.BeginReceive(state.Buffer, 0, Server.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            else
            {
                // process the state

                var newState = new MessageState
                {
                    Buffer = new byte[Server.BufferSize]
                };

                Socket.BeginReceive(newState.Buffer, 0, Server.BufferSize, 0, new AsyncCallback(ReadCallback), newState);
            }
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