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

                udpClient.Connect("10.0.0.1", 11000);

                Console.WriteLine("Connected to server. Sending message: This is the client connection");

                // Sends a message to the host to which you have connected.
                Byte[] sendBytes = Encoding.ASCII.GetBytes("This is the client connection");
                udpClient.Send(sendBytes, sendBytes.Length);

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("This is the message you received: " +
                                             returnData.ToString());
                Console.WriteLine("This message was sent from " +
                                            RemoteIpEndPoint.Address.ToString() +
                                            " on their port number " +
                                            RemoteIpEndPoint.Port.ToString());
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
                    Console.WriteLine("Input error. Input was:{0}:", input);
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
