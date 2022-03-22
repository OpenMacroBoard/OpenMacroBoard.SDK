using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace BeaconLib
{
    internal static class UdpClientNatTraversalExtension
    {
        public static void EnableNatTraversal(this UdpClient client)
        {
            try
            {
#if NET5_0_OR_GREATER
                if (OperatingSystem.IsWindows())
                {
                    client.AllowNatTraversal(true);
                }
#else
                client.AllowNatTraversal(true);
#endif
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error switching on NAT traversal: " + ex.Message);
            }
        }
    }
}
