using BlueDB.Communication.Messages;
using BlueDB.Communication.Messages.Commands;
using BlueDB.DriverNET.Messages;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BlueDB.DriverNET
{
    public class ClientConnection : IDisposable
    {
        private HostPool _host;
        private ushort _idsIndex;

        public ClientConnection(IPEndPoint endPoint)
        {
            _host = HostStorage.GetHost(endPoint);
            _idsIndex = 0;
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
            var request = new MessageRequest
            {
                Commands = new ICommand[]
                {
                    new ReleaseConnectionCommand()
                }
            };

            var response = SendMessage(request).Result;
            if (!string.IsNullOrEmpty(response.Error))
            {
                //TODO: tratar erro ao soltar a connection
            }

            _host.Release();
            _host = null;
        }

        public void SendMessage(MessageRequest request, Action<MessageReponse> callBack)
        {
            request.Id = ++_idsIndex;

            _host.SendMessage(request, callBack);
        }

        public async Task<MessageReponse> SendMessage(MessageRequest request)
        {
            request.Id = ++_idsIndex;

            var t = new TaskCompletionSource<MessageReponse>();

            _host.SendMessage(request, s => t.TrySetResult(s));

            return await t.Task;
        }

        public RequestBuilder CreateRequest()
        {
            return new RequestBuilder(this);
        }
    }
}