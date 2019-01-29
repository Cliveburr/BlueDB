using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BlueDB.Serialize.Tests
{
    public class DNETSerializeHelper
    {
        public static Tuple<TimeSpan, int, object> TestDNETSerialize(object values)
        {
            var sw = Stopwatch.StartNew();

            using (var serializeStream = new MemoryStream())
            {
                var serializeFormatter = new BinaryFormatter();
                serializeFormatter.Serialize(serializeStream, values);
                serializeStream.Flush();
                serializeStream.Position = 0;
                var bytes = serializeStream.ToArray();

                using (var deserializeStream = new MemoryStream(bytes))
                {
                    var deserializeFormatter = new BinaryFormatter();
                    deserializeStream.Seek(0, SeekOrigin.Begin);
                    var returnedValues = deserializeFormatter.Deserialize(deserializeStream);

                    sw.Stop();

                    return new Tuple<TimeSpan, int, object>(sw.Elapsed, bytes.Length, returnedValues);
                }
            }
        }
    }
}