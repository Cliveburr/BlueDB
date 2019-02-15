using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Communication.Socket
{
    public abstract class BaseSocket : MessageProtocol
    {
        public System.Net.Sockets.Socket Socket { get; protected set; }

        public int ReceiveBufferSize { get; set; }

        public BaseSocket(System.Net.Sockets.Socket socket)
        {
            Socket = socket;

            ReceiveBufferSize = 1024;
        }

        protected void BeginReceive()
        {
            try
            {
                var buffer = new byte[ReceiveBufferSize];

                Socket.BeginReceive(buffer, 0, ReceiveBufferSize, 0, new AsyncCallback(ReceiveCallback), buffer);
            }
            catch (Exception err)
            {
                Task.Run(() => HandleReceiveError(err));
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var bytes = (byte[])ar.AsyncState;
            var bytesRead = 0;

            try
            {
                bytesRead = Socket.EndReceive(ar);

                Receive(bytes, bytesRead);
            }
            catch (Exception err)
            {
                Task.Run(() => HandleReceiveError(err));
            }

            BeginReceive();
        }

        protected virtual void HandleReceiveError(Exception err)
        {
            Console.Write(err.ToString());
        }

        protected override void ProcessSend(byte[] bytes)
        {
            try
            {
                Socket.BeginSend(bytes, 0, bytes.Length, 0, new AsyncCallback(SendCallback), bytes);
            }
            catch (Exception err)
            {
                Task.Run(() => HandleSendError(err));
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            base.EndSend();

            try
            {
                var bytes = ar.AsyncState as byte[];

                var bytesSent = Socket.EndSend(ar);
                if (bytesSent != bytes.Length)
                {
                    throw new Exception("something wrong!");
                }

                OnSent(bytes);
            }
            catch (Exception err)
            {
                Task.Run(() => HandleSendError(err));
            }
        }

        protected virtual void HandleSendError(Exception err)
        {
            Console.Write(err.ToString());
        }

        protected virtual void OnSent(byte[] bytes)
        {
        }
    }
}