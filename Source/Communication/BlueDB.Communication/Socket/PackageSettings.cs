using System;
using System.Collections.Generic;
using System.Text;

namespace BlueDB.Communication.Socket
{
    public class PackageSettings
    {
        public bool HasMore { get; private set; }
        public ushort Lenght { get; private set; }
        public ushort Settings { get; private set; }

        public PackageSettings(ushort settings)
        {
            Settings = settings;
            HasMore = (settings & 0x8000) == 0x8000;
            Lenght = (ushort)(settings & 0x7FFF);
        }

        public PackageSettings(ushort lenght, bool hasMore)
        {
            Settings = lenght |= (ushort)(hasMore ? 0x8000 : 0x0000);
            HasMore = hasMore;
            Lenght = lenght;
        }

        public static ushort MaxSettingsLenght()
        {
            return 0x7FFF;
        }
    }
}