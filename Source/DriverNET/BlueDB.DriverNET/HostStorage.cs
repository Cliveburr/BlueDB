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
        private static object _poolsLock;

        static HostStorage()
        {
            _pools = new List<HostPool>();
            _poolsLock = new object();
        }

        public static HostPool GetHost(IPEndPoint endPoint)
        {
            lock (_poolsLock)
            {
                var pool = _pools
                    .FirstOrDefault(p => p.EndPoint == endPoint);

                if (pool == null)
                {
                    pool = new HostPool(endPoint);
                }
                else
                {
                    _pools.Remove(pool);
                }

                return pool;
            }
        }

        public static void Release(HostPool pool)
        {
            lock (_poolsLock)
            {
                _pools.Add(pool);
            }
        }
    }
}