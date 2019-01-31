using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Communication
{
    public class SocketMessage : IDisposable
    {
        public SocketMessageState State { get; set; }
        public ushort Id { get; set; }
        public byte[] Buffer { get; set; }

        private MemoryStream _messageStream;
        private BinaryWriter _binaryWriter;

        private uint _packageLeft;
        private bool _hasMore;

        public SocketMessage()
        {
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

        public void WriteBufferToStream(int count)
        {
            if (State == SocketMessageState.Pristine)
            {
                State = SocketMessageState.Receiving;
                Id = BitConverter.ToUInt16(Buffer, 0);

                ComputePackage(2, count - 2);
            }
            else if (State == SocketMessageState.Receiving)
            {
                ComputePackage(0, count);
            }
            else
            {
                throw new Exception();
            }
        }

        private void ComputePackage(int index, int count)
        {
            if (_packageLeft == 0)
            {
                var settingsUshort = BitConverter.ToUInt16(Buffer, index);
                index += 2;
                count -= 2;
                var settings = GetPackageSettings(settingsUshort);
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
                State = SocketMessageState.Received;
            }
            else
            {
                if (count > 0)
                {
                    ComputePackage(index, count);
                }
            }
        }

        public static ushort MaxSettingsLenght()
        {
            return 0x7FFF;
        }

        public static Tuple<ushort, bool> GetPackageSettings(ushort settings)
        {
            var hasMore = (settings & 0x8000) == 0x8000;
            var lenght = settings & 0x7FFF;
            return new Tuple<ushort, bool>((ushort)lenght, hasMore);
        }

        public static ushort SetPackageSettings(ushort lenght, bool hasMore)
        {
            return lenght |= (ushort)(hasMore ? 0x8000 : 0x0000);
        }
    }

    public enum SocketMessageState
    {
        Pristine = 0,
        Receiving = 1,
        Received = 2,
        Processing = 3,
        Answering = 4,
        Completed = 5
    }
}