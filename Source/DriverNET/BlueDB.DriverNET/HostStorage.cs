using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BlueDB.DriverNET
{
    public static class HostStorage
    {
        private static List<HostPool> _pools;

        public static HostPool GetHost(IPEndPoint endPoint)
        {
            var pool = _pools
                .FirstOrDefault(p => p.EndPoint == endPoint);

            if (pool == null)
            {
                pool = new HostPool(endPoint);
            }

            return pool;
        }
    }
}