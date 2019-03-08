using System;
using System.IO;
using System.Threading.Tasks;

namespace BlueDB.Data
{
    public class SystemFile : IDisposable
    {
        public string Path { get; private set; }

        protected FileStream _file;

        public SystemFile(string path)
        {
            Path = path;

            Open();
        }

        private void Open()
        {
            _file = File.Open(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public void Dispose()
        {
            _file?.Flush();
            _file?.Close();
            _file?.Dispose();
            _file = null;
        }

        public long Position
        {
            set
            {
                _file.Position = value;
            }
        }

        public async Task<byte[]> Read(long pos, int count)
        {
            var buffer = new byte[count];
            _file.Position = pos;
            if (await _file.ReadAsync(buffer, 0, count) == count)
            {
                return buffer;
            }
            else
            {
                throw new FileLoadException();
            }
        }

        public async Task<byte[]> Read(int count)
        {
            var buffer = new byte[count];
            if (await _file.ReadAsync(buffer, 0, count) == count)
            {
                return buffer;
            }
            else
            {
                throw new FileLoadException();
            }
        }

        public async Task Write(int pos, byte[] data)
        {
            CheckFileLength(pos + data.Length);

            _file.Position = pos;
            await _file.WriteAsync(data, 0, data.Length);
        }

        public async Task Write(byte[] data)
        {
            CheckFileLength(_file.Position + data.Length);

            await _file.WriteAsync(data, 0, data.Length);
        }

        protected void CheckFileLength(long length)
        {
            if (_file.Length < length)
            {
                _file.SetLength(length);
            }
        }
    }
}