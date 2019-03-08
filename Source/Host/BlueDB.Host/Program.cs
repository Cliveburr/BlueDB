using BlueDB.Communication;
using BlueDB.Communication.Messages;
using BlueDB.Communication.Socket;
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
            var server = new SocketServer
            {
                BeginMessageProcess = MessageProcess
            };

            server.Start("127.0.0.1", 8011);

            Console.ReadKey();
        }

        private static void MessageProcess(SocketServerConnection connection, MessageRequest request, Action<MessageReponse> messageProcessCallback)
        {
            var context = new ProcessContext
            {
                Connection = connection,
                Callback = messageProcessCallback,
                Request = request
            };

            PreProcess.Instance.Compute(context);

            ExecutorController.Instance.ScheduleToProcess(context);
        }

        //private static void ResponseErrors(ProcessContext context, Exception error)
        //{
        //    var response = new MessageReponse
        //    {
        //        Error = error.Message
        //    };

        //    context.Callback(response);
        //}
    }
}
