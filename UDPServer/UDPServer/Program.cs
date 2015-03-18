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
            Console.WriteLine("Starting server...");

            //Opsætning af server til modtagelse af clienter
            var ipep = new IPEndPoint(IPAddress.Any, 11000);
            var newsock = new UdpClient(ipep);

            //Venter på forbindelse til client
            Console.WriteLine("Waiting for a client...");
            var sender = new IPEndPoint(IPAddress.Any, 0);

            //Indlæsning og udskrivning af besked fra client
            var data = newsock.Receive(ref sender);
            Console.WriteLine("Message received from {0}:", sender.ToString());
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

            //Svar til client om forbindelse
            var connectedMessage = "You are connected to the UDPServer";
            data = Encoding.ASCII.GetBytes(connectedMessage);
            newsock.Send(data, data.Length, sender);

            //Start server routinen
            while (true)
            {
               Console.WriteLine("Waiting for client...");
               data = newsock.Receive(ref sender);
               if (Encoding.ASCII.GetString(data, 0, data.Length) == "u"|| Encoding.ASCII.GetString(data, 0, data.Length) == "U")
               {
                 Console.WriteLine("Client requested server uptime information...");
                 string text = System.IO.File.ReadAllText(@"/proc/uptime");
                 Console.WriteLine("Server uptime is: {0}", text);
                 newsock.Send(text, text.Length, sender);
               }
               if (Encoding.ASCII.GetString(data, 0, data.Length) == "l"|| Encoding.ASCII.GetString(data, 0, data.Length) == "L")
               {
                 Console.WriteLine("Client requested CPU-load information...");
                 string text = System.IO.File.ReadAllText(@"/proc/loadavg");
                 Console.WriteLine("Server cpu load is: {0}", text);
                 newsock.Send(text, text.Length, sender);
               }
            }
        }
    }
}
