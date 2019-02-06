using System;
using System.Net;

namespace BlueDB.DriverNET
{
    public class ClientConnection : IDisposable
    {

        public ClientConnection(IPEndPoint endPoint)
        {

        }

        public ClientConnection(IPAddress address, int port)
            : this(new IPEndPoint(address, port))
        {
        }

        public ClientConnection(string address, int port)
            : this(IPAddress.Parse(address), port)
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}