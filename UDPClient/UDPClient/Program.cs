using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting client...");
            UdpClient udpClient = new UdpClient(11000);
            try
            {
                Console.WriteLine("Connecting to server...");
                // Forsøg at oprette forbindelse til serveren med ip 10.0.0.1 og port 11000
                udpClient.Connect("10.0.0.1", 11000);

                Console.WriteLine("Connected to server. Sending message: This is the client connection");

                // Send en testbesked til serveren for at tjekke forbindelsen
                Byte[] sendBytes = Encoding.ASCII.GetBytes("This is the client connection");
                udpClient.Send(sendBytes, sendBytes.Length);

                // Tjek efter beskeder fra en hvilken som helst ip
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Aflæs bytes fra afsenderen (I dette tilfælde vores UDP server)
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                // Udskriv svaret fra afsenderen (Serveren)
                Console.WriteLine("This is the message you received: " +
                                             returnData.ToString());

                while(true) {
                  // Send kommand til server
                  Console.WriteLine("Choose command for server. u/U for uptime. l/L for cpu load info:");
                  string input = Console.ReadLine();
                  sendBytes = Encoding.ASCII.GetBytes(input);
                  udpClient.Send(sendBytes, sendBytes.Length);

                  //Vent på at modtage data
                  RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                  // Indlæs data
                  receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                  returnData = Encoding.ASCII.GetString(receiveBytes);

                  //Udskriv relevent data
                  if (input == "u"|| input == "U") {
                    Console.WriteLine("The server uptime is: " + returnData.ToString());
                  }
                  else if (input == "l"|| input == "L") {
                    Console.WriteLine("The server cpu load is: " + returnData.ToString());
                  }
                  else {
                    Console.WriteLine("" + returnData.ToString());
                  }
                }

                udpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
