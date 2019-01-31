using BlueDB.Communication;
using BlueDB.Communication.Messages;
using BlueDB.Communication.Socket;
using BlueDB.Serialize;
using System;

namespace BlueDB.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new SocketServer();

            server.BeginMessageProcess = MessageProcess;

            server.Start(8011);

            Console.ReadKey();
        }

        private static void MessageProcess(SocketServerConnection connection, ReceiveMessage message, Action<SendMessage> messageProcessCallback)
        {
            var serialize = BinarySerialize.From<MessageData>();
            var requestData = serialize.Deserialize(message.GetBytes);

            var responseData = new MessageData
            {
                Id = requestData.Id,
                TextView = "resposta do server!",
                Bytes = requestData.Bytes
            };

            var responseBytes = serialize.Serialize(responseData);

            var response = new SendMessage(responseBytes);

            messageProcessCallback(response);
        }
    }
}
