using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Communication
{
    public class SocketClientMessage : IDisposable
    {
        public SocketClientMessageState State { get; set; }
        public ushort Id { get; set; }
        public byte[] Buffer { get; set; }

        private MemoryStream _messageStream;
        private BinaryWriter _binaryWriter;

        private MemoryStream _packageStream;
        private BinaryReader _packageReader;

        public SocketClientMessage()
        {
            _messageStream = new MemoryStream();
            _binaryWriter = new BinaryWriter(_messageStream);
        }

        public void Dispose()
        {
            _messageStream?.Dispose();
            _binaryWriter?.Dispose();
        }

        public void WriteBufferToStream(int count)
        {
            if (State == SocketClientMessageState.Pristine)
            {
                State = SocketClientMessageState.Receiving;
                Id = BitConverter.ToUInt16(Buffer, 0);

                
            }
            else if (message.State == SocketClientMessageState.Receiving)
            {
                message.WriteBufferToStream(bytesRead);
            }
            else
            {
                throw new Exception();
            }

            _binaryWriter.Write(Buffer, 0, count);
        }
    }

    public enum SocketClientMessageState
    {
        Pristine = 0,
        Receiving = 1,
        Received = 2,
        Processing = 3,
        Answering = 4,
        Answered = 5
    }
}