using System.Collections.Generic;
using System.Net;

namespace BeaconLib
{
    internal class IPEndPointComparer : IComparer<IPEndPoint>
    {
        public static readonly IPEndPointComparer Instance = new();

        public int Compare(IPEndPoint? x, IPEndPoint? y)
        {
            if (x is null && y is null)
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            var c = string.CompareOrdinal(
                x.Address.ToString(),
                y.Address.ToString()
            );

            if (c != 0)
            {
                return c;
            }

            return y.Port - x.Port;
        }
    }
}
