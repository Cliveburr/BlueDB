using BlueDB.Communication;
using System;

namespace BlueDB.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new SocketServer();
            server.Start(8011);

            Console.ReadKey();
        }
    }
}
