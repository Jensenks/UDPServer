using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var ipep = new IPEndPoint(IPAddress.Any, 9050);
            var newsock = new UdpClient(ipep);

            Console.WriteLine("Waiting for a client...");

            var sender = new IPEndPoint(IPAddress.Any, 0);

            var data = newsock.Receive(ref sender);

            Console.WriteLine("Message received from {0}:", sender.ToString());
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

            var welcome = "Welcome to my test server";
            data = Encoding.ASCII.GetBytes(welcome);
            newsock.Send(data, data.Length, sender);

            //while (true)
            //{
            //    data = newsock.Receive(ref sender);

            //    Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
            //    newsock.Send(data, data.Length, sender);
            //}
        }
    }
}
