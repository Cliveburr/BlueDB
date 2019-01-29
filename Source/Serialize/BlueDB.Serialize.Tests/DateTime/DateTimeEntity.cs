using System;
using System.Collections.Generic;
using System.Text;
using SystemDateTime = System.DateTime;

namespace BlueDB.Serialize.Tests.DateTime
{
#pragma warning disable
    [Serializable]
    public class DateTimeEntity
    {
        public SystemDateTime DateTime { get; set; }
        public SystemDateTime MinDateTime { get; set; }
        public SystemDateTime MaxDateTime { get; set; }
        public SystemDateTime ZeroDateTime { get; set; }
        public SystemDateTime JustNewDateTime { get; set; }

        public void Populate()
        {
            DateTime = System.DateTime.Now;
            MinDateTime = System.DateTime.MinValue;
            MaxDateTime = System.DateTime.MaxValue;
            ZeroDateTime = new SystemDateTime(0);
            JustNewDateTime = new SystemDateTime();
        }
    }
#pragma warning restore
}