using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Socket
{
    public class PackageMessage
    {
        public static ushort MaxSettingsLenght()
        {
            return 0x7FFF;
        }

        public static Tuple<ushort, bool> GetPackageSettings(ushort settings)
        {
            var hasMore = (settings & 0x8000) == 0x8000;
            var lenght = settings & 0x7FFF;
            return new Tuple<ushort, bool>((ushort)lenght, hasMore);
        }

        public static ushort SetPackageSettings(ushort lenght, bool hasMore)
        {
            return lenght |= (ushort)(hasMore ? 0x8000 : 0x0000);
        }
    }
}