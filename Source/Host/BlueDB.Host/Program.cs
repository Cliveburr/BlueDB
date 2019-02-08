using BlueDB.Communication;
using BlueDB.Communication.Messages;
using BlueDB.Communication.Socket;
using BlueDB.Host.Context;
using BlueDB.Host.Process;
using BlueDB.Serialize;
using System;
using System.Linq;

namespace BlueDB.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new SocketServer();

            server.BeginMessageProcess = MessageProcess;

            server.Start("127.0.0.1", 8011);

            Console.ReadKey();
        }

        private static void MessageProcess(SocketServerConnection connection, ReceiveMessage message, Action<SendMessage> messageProcessCallback)
        {
            var connectionContext = GetConnectionContext(connection);
            var context = connectionContext.Clone();

            context.Connection = connection;
            context.Callback = messageProcessCallback;

            var request = DeserializeRequest(context, message);
            if (request == null)
            {
                return;
            }

            context.Request = request;

            var processRequest = PreProcess.Instance.Compute(context);

            ExecutorController.Instance.ScheduleToProcess(processRequest);
        }

        private static ConnectionContext GetConnectionContext(SocketServerConnection connection)
        {
            var context = connection.GetItemOfBag<ConnectionContext>("Context");
            if (context == null)
            {
                context = new ConnectionContext();
                connection.SetItemOnBag("Context", context);
            }
            return context;
        }

        private static MessageRequest DeserializeRequest(ConnectionContext context, ReceiveMessage message)
        {
            try
            {
                var serialize = BinarySerialize.From<MessageRequest>();
                return serialize.Deserialize(message.GetBytes);
            }
            catch (Exception err)
            {
                ResponseErrors(context, err);
                return null;
            }
        }

        private static void ResponseErrors(ConnectionContext context, Exception error)
        {
            var response = new MessageReponse
            {
                Error = error.Message
            };

            var sendMessage = SerializeResponse(response);

            context.Callback(sendMessage);
        }

        private static SendMessage SerializeResponse(MessageReponse response)
        {
            var serialize = BinarySerialize.From<MessageReponse>();
            var bytes = serialize.Serialize(response);
            return new SendMessage(bytes);
        }
    }
}
