using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Data.DataBlock
{
    // Data split in block
    //
    // PPPPPPPP = ulong - block connection, position to next block
    //
    // DDDDD...DDDDD<end of block>PPPPPPPP
    //
    /*
     * 0                             2                             0
     * 000000000000000000000000000000111111111111111111111111111111222222222222222222222222222222
     * 
     * 
     * 
     * 
     * 
     */

    public class DataBlockFile<T> where T : class
    {
        public DataBlockFile(string path, int blockSize)
        {

        }

        //public T Read(ulong pos)
        //{

        //}

        public void Write(ulong pos, T data)
        {
            //using (var block = new DataBlockStream())
            //{
            //    block.SetBlocks(0, 2);

            //    block.Write()
            //}
        }
    }


    public abstract class BlockStream : Stream
    {
        public override bool CanRead => _ioStream.CanRead;
        public override bool CanSeek => false;
        public override bool CanWrite => _ioStream.CanWrite;
        public override long Length => _ioStream.Length;

        private Stream _ioStream;
        private int _blockSize;
        private int _blockBodySize;
        private int _blockleft;
        private long _blockPosition;

        public abstract long GetFreeBlockPosition();

        public BlockStream(Stream ioStream, int blockSize)
        {
            _ioStream = ioStream;
            _blockPosition = -1;
            _blockleft = 0;

            SetBlockSize(blockSize);
        }

        private void SetBlockSize(int blockSize)
        {
            _blockSize = blockSize;
            _blockBodySize = blockSize - 4;
        }

        public override long Position
        {
            get
            {
                return _blockPosition;
            }
            set
            {
                _blockPosition = value;
                if (value > -1)
                {
                    _ioStream.Position = _blockSize * value;
                    _blockleft = _blockBodySize;
                }
                else
                {
                    _blockleft = 0;
                }
            }
        }

        public override void Flush()
        {
            _ioStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_blockPosition == -1)
            {
                throw new Exception("Invalid position!");
            }

            var totalRead = 0;
            while (totalRead < count)
            {
                var toRead = Math.Min(count - totalRead, _blockleft);
                var read = _ioStream.Read(buffer, offset, toRead);
                totalRead += read;
                _blockleft -= read;
                offset += read;

                if (_blockleft == 0)
                {
                    var blockConnectionBytes = new byte[4];
                    _ioStream.Read(blockConnectionBytes, 0, 4);
                    var ioPosition = BitConverter.ToUInt32(blockConnectionBytes, 0);

                    _blockPosition = ioPosition / _blockSize;
                    _ioStream.Position = ioPosition;
                    _blockleft = _blockBodySize;
                }
            }
            return totalRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            _ioStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var needToWriteConnection = false;
            while (count > 0)
            {
                if (_blockleft == 0)
                {
                    _blockPosition = GetFreeBlockPosition();
                    var ioPosition = _blockSize * _blockPosition;
                    var fileBorder = ioPosition + _blockSize;

                    if (_ioStream.Length < fileBorder)
                    {
                        _ioStream.SetLength(fileBorder);
                    }

                    if (needToWriteConnection)
                    {
                        var ioPositionBytes = BitConverter.GetBytes(ioPosition);
                        _ioStream.Write(ioPositionBytes, 0, 4);
                    }

                    _ioStream.Position = ioPosition;
                    _blockleft = _blockBodySize;
                }

                var toWrite = Math.Min(count, _blockleft);
                _ioStream.Write(buffer, offset, toWrite);
                _blockleft -= toWrite;
                count -= toWrite;
                offset += toWrite;
                needToWriteConnection = true;
            }
        }
    }
}