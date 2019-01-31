using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Communication.Socket
{
    public class ReceiveMessage : IDisposable
    {
        public bool IsReceived { get; set; }
        public byte[] Buffer { get; set; }

        private MemoryStream _messageStream;
        private BinaryWriter _binaryWriter;

        private uint _packageLeft;
        private bool _hasMore;

        public ReceiveMessage()
        {
            IsReceived = false;
            _messageStream = new MemoryStream();
            _binaryWriter = new BinaryWriter(_messageStream);
        }

        public byte[] GetBytes
        {
            get
            {
                return _messageStream.ToArray();
            }
        }

        public void Dispose()
        {
            _messageStream?.Dispose();
            _binaryWriter?.Dispose();
        }

        public void WriteBufferToStream(int index, int count)
        {
            if (_packageLeft == 0)
            {
                var settingsUshort = BitConverter.ToUInt16(Buffer, index);
                index += 2;
                count -= 2;
                var settings = PackageMessage.GetPackageSettings(settingsUshort);
                _packageLeft = settings.Item1;
                _hasMore = settings.Item2;
            }

            if (count <= _packageLeft)
            {
                _binaryWriter.Write(Buffer, index, count);
                _packageLeft -= (uint)count;
                count = 0;
            }
            else
            {
                _binaryWriter.Write(Buffer, index, (int)_packageLeft);
                index += (int)_packageLeft;
                count -= (int)_packageLeft;
                _packageLeft = 0;
            }

            if (_packageLeft == 0 && !_hasMore)
            {
                IsReceived = true;

                if (count > 0)
                {
                    //TODO:
                    throw new Exception("this messages end but already receive data from the next message!");
                }
            }
            else
            {
                if (count > 0)
                {
                    WriteBufferToStream(index, count);
                }
            }
        }
    }
}