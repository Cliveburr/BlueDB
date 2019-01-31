using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Socket
{
    public abstract class BaseSocket
    {
        public System.Net.Sockets.Socket Socket { get; protected set; }

        private Queue<SendMessage> _sendMessages;
        private bool _isSendingMessage;
        private object _isSendingLock;

        public BaseSocket(System.Net.Sockets.Socket socket)
        {
            Socket = socket;
            _sendMessages = new Queue<SendMessage>();
            _isSendingMessage = false;
            _isSendingLock = new object();
        }

        protected void BeginReceive(ReceiveMessage message)
        {
            try
            {
                Socket.BeginReceive(message.Buffer, 0, message.Buffer.Length, 0, new AsyncCallback(ReceiveCallback), message);
            }
            catch (Exception err)
            {
                HandleReceiveError(err);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var message = (ReceiveMessage)ar.AsyncState;
            var bytesRead = 0;

            try
            {
                bytesRead = Socket.EndReceive(ar);
            }
            catch (Exception err)
            {
                HandleReceiveError(err);
            }

            if (bytesRead > 0)
            {
                message.WriteBufferToStream(0, bytesRead);

                if (message.IsReceived)
                {
                    FinishReceiving(message);
                }
                else
                {
                    BeginReceive(message);
                }
            }
            else
            {
                FinishReceiving(message);
            }
        }

        protected virtual void FinishReceiving(ReceiveMessage message)
        {
        }

        protected virtual void HandleReceiveError(Exception err)
        {
        }

        public void SendMessage(SendMessage message)
        {
            _sendMessages.Enqueue(message);

            CheckForSendMessage();
        }

        private void CheckForSendMessage()
        {
            lock (_isSendingLock)
            {
                if (_isSendingMessage)
                {
                    return;
                }
                _isSendingMessage = true;
            }

            if (_sendMessages.Count > 0)
            {
                var message = _sendMessages.Dequeue();

                Socket.BeginSend(message.Bytes, 0, message.Bytes.Length, 0, new AsyncCallback(SendCallback), message);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            lock (_isSendingLock)
            {
                _isSendingLock = false;
            }

            try
            {
                var message = ar.AsyncState as SendMessage;

                var bytesSent = Socket.EndSend(ar);
                if (bytesSent != message.Bytes.Length)
                {
                    throw new Exception("something wrong!");
                }

                message.IsSent = true;
                MessageSent(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        protected virtual void MessageSent(SendMessage message)
        {
        }
    }
}