using BlueDB.Communication.Messages;
using BlueDB.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueDB.Communication.Socket
{
    public class SendMessage
    {
        public bool IsSent { get; set; }
        public byte[] Bytes { get; set; }

        public SendMessage(byte[] bytes)
        {
            SetBytes(bytes);
        }

        private void SetBytes(byte[] bytes)
        {
            var left = bytes.Length;
            var index = 0;

            using (var memoryStream = new MemoryStream())
            using (var writer = new BinaryWriter(memoryStream))
            {
                while (left > 0)
                {
                    var lenght = Math.Min(PackageMessage.MaxSettingsLenght(), left);
                    left -= lenght;
                    var hasMore = left > 0;

                    var settings = PackageMessage.SetPackageSettings((ushort)lenght, hasMore);

                    writer.Write(settings);

                    writer.Write(bytes, index, lenght);
                    index += lenght;
                }

                Bytes = memoryStream.ToArray();
            }

        }
    }
}