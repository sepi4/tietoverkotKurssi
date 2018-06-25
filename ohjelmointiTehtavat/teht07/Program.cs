using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace teht07
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            int port = 9999;

            IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, port);
            byte[] rec = new byte[256];

            EndPoint ep = (EndPoint)iep;
            s.ReceiveTimeout = 100;
            String msg;
            Boolean on = true;
            do
            {
                Console.Write(">");
                msg = Console.ReadLine();
                if (msg.Equals("q"))
                    on = false;
                else
                {
                    s.SendTo(Encoding.ASCII.GetBytes(msg), ep);
                    while(!Console.KeyAvailable) // ikuinen 
                    {
                        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                        EndPoint Palvelinep = (EndPoint)remote;
                        // int paljon = 0;

                        try
                        {
                            int montakoMerkkia = s.ReceiveFrom(rec, ref Palvelinep);
                            String recString = Encoding.ASCII.GetString(rec, 0, montakoMerkkia);
                            String[] osat = recString.Split(';', 2);
                            if (osat.Length < 2) {
                                Console.WriteLine("Virheelinen formaatti (nimi;teksti)");
                            }
                            else 
                            {
                                Console.WriteLine("{0}: {1}", osat[0], osat[1]);
                            }
                        }
                        catch (System.Exception) // jos sekunnin sis
                        {
                            // timeout
                        }
                    }

                }
            } while (on);
            s.Close();
        }
    }
}
