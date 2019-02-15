using BlueDB.Communication.Socket;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BlueDB.Communication.Tests
{
    [TestClass]
    public class SocketMessageSettingsTest
    {
        [TestMethod]
        public void TestValues()
        {
            var values = new List<ushort>
            {
                0x0000, 0x0001, 0x0010, 0x0100, 0x1000,
                0x0002, 0x0020, 0x0200, 0x2000, 0x2001, 0x2011, 0x2111,
                0x0003, 0x0030, 0x0300, 0x3000, 0x3001, 0x3011, 0x3111,
                0x0005, 0x0050, 0x0500, 0x5000, 0x5002, 0x5022, 0x5222,
                0x0006, 0x0060, 0x0600, 0x6000, 0x6002, 0x6022, 0x6222,
                0x0008, 0x0080, 0x0800, 0x000A, 0x00A2, 0x0A22, 0x0AAA,
                0x0009, 0x0080, 0x0800, 0x700F, 0x70FF, 0x7FFF
            };
            values.ForEach(v => TestValue(v));
        }

        private void TestValue(ushort value)
        {
            var settingsFalse = new PackageSettings(value, false);
            var settingsBackFalse = new PackageSettings(settingsFalse.Settings);
            Assert.AreEqual(value, settingsBackFalse.Lenght);
            Assert.IsFalse(settingsBackFalse.HasMore);

            var settingsTrue = new PackageSettings(value, true);
            var settingsBackTrue = new PackageSettings(settingsTrue.Settings);
            Assert.AreEqual(value, settingsBackTrue.Lenght);
            Assert.IsTrue(settingsBackTrue.HasMore);
        }
    }
}
