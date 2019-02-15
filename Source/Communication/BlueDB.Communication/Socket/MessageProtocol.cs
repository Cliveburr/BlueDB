using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueDB.Communication.Socket
{
    public abstract class MessageProtocol
    {
        private Queue<byte[]> _toSend;
        private bool _isSendingMessage;
        private object _isSendingLock;

        private ushort _receivePackageLeft;
        private bool _receiveHasMore;
        private List<byte> _receiveBuffer;
        private object _receiveLock;

        protected abstract void ProcessSend(byte[] bytes);

        public MessageProtocol()
        {
            _toSend = new Queue<byte[]>();
            _isSendingLock = new object();
            _receiveLock = new object();
        }

        public void Send(byte[] bytes)
        {
            var fullBytes = CreatePackages(bytes);

            _toSend.Enqueue(fullBytes);

            CheckForSendMessage();
        }

        private byte[] CreatePackages(byte[] bytes)
        {
            var left = bytes.Length;
            var index = 0;
            var memory = new List<byte>();

            while (left > 0)
            {
                var lenght = Math.Min(PackageSettings.MaxSettingsLenght(), left);
                left -= lenght;
                var hasMore = left > 0;

                var settings = new PackageSettings((ushort)lenght, hasMore);
                var settingsBytes = BitConverter.GetBytes(settings.Settings);
                memory.AddRange(settingsBytes);

                memory.AddRange(SubArray(bytes, index, lenght));
                index += lenght;
            }

            return memory.ToArray();
        }

        private void CheckForSendMessage()
        {
            lock (_isSendingLock)
            {
                if (_isSendingMessage)
                {
                    return;
                }
                else
                {
                    if (_toSend.Count > 0)
                    {
                        _isSendingMessage = true;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            var bytes = _toSend.Dequeue();

            Task.Factory.StartNew(PreProcessSend, bytes);
        }

        private void PreProcessSend(object state)
        {
            ProcessSend((byte[])state);
        }

        protected virtual void EndSend()
        {
            lock (_isSendingLock)
            {
                _isSendingMessage = false;
            }

            CheckForSendMessage();
        }

        public void Receive(byte[] bytes, int count)
        {
            lock (_receiveLock)
            {
                WriteToBuffer(bytes, 0, count);
            }
        }

        private void WriteToBuffer(byte[] bytes, int index, int count)
        {
            if (_receiveBuffer == null)
            {
                _receiveBuffer = new List<byte>();
                var settingsUshort = BitConverter.ToUInt16(bytes, index);
                index += 2;
                count -= 2;
                var settings = new PackageSettings(settingsUshort);
                _receivePackageLeft = settings.Lenght;
                _receiveHasMore = settings.HasMore;
            }

            if (count <= _receivePackageLeft)
            {
                _receiveBuffer.AddRange(SubArray(bytes, index, count));
                _receivePackageLeft -= (ushort)count;
                count = 0;
            }
            else
            {
                _receiveBuffer.AddRange(SubArray(bytes, index, _receivePackageLeft));
                index += _receivePackageLeft;
                count -= _receivePackageLeft;
                _receivePackageLeft = 0;
            }

            if (_receivePackageLeft == 0 && !_receiveHasMore)
            {
                CallReceived();
            }

            if (count > 0)
            {
                WriteToBuffer(bytes, index, count);
            }
        }

        private void CallReceived()
        {
            Task.Factory.StartNew(PreOnReceived, _receiveBuffer.ToArray());
            _receiveBuffer = null;
        }

        private void PreOnReceived(object state)
        {
            OnReceived((byte[])state);
        }

        protected virtual void OnReceived(byte[] bytes)
        {
        }

        private T[] SubArray<T>(T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}