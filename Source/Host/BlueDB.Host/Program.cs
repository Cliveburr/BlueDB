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





            var test0 = (ushort)0x0000;

            var test0SettingsFalse = SocketMessage.SetPackageSettings(test0, false);
            var test0SettingsBackFalse = SocketMessage.GetPackageSettings(test0SettingsFalse);


            var test0SettingsTrue = SocketMessage.SetPackageSettings(test0, true);





            Console.ReadKey();
        }
    }
}
