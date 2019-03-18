using System;
using System.Net;
using System.Net.Sockets;

namespace IpAddressPrinter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Your IP address is \"{GetLocalIpAddress()}\"");
        }

        private static string GetLocalIpAddress()
        {
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    return endPoint?.Address.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
