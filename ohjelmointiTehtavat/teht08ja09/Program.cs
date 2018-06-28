using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace teht08ja09
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket palvelin = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, 9999);
            EndPoint Pep = (IPEndPoint)iep;
            Console.Write("Anna nimesi: ");
            String nimi = Console.ReadLine();
            Laheta(palvelin, Pep, "JOIN " + nimi);

            Boolean on = true;
            String TILA = "JOIN";

            while (on)
            {
                String[] palat = Vastaanota(palvelin); // palauttaa splitatut palat
                switch (TILA)
                {
                    case "JOIN":
                        if (palat[0] == "ACK")
                        {
                            if (palat[1] == "201")
                                Console.WriteLine("Odotetaan toista pelajaa");
                            else if (palat[1] == "202")
                            {
                                TILA = "GAME";
                                Console.WriteLine("Vastustajasi on " + palat[2]);
                                KysyNum(palvelin, Pep);
                            }
                            else if (palat[1] == "203")
                            {
                                TILA = "GAME";
                                Console.WriteLine("Vastustaja {0} saa aloittaa ", palat[2]);
                            }
                            else
                            {
                                System.Console.WriteLine("Virhe, palat[1]: " + palat[1]);
                            }
                        }
                        else 
                        {
                            System.Console.WriteLine("Virhe, palat[0]: " + palat[0]);
                        } // if (palat[0] == "ACK")
                        break;
                    case "GAME":
                        if (palat[0] == "ACK")
                        {
                            if (palat[1] == "300")
                                Console.WriteLine("Väärä numero. Vastustajan vuoro.");
                            else if (palat[1] == "407")
                                Console.WriteLine("Virhe: arvauksesi ei ollut numero. Kokeile uudestaan.");
                                KysyNum(palvelin, Pep);
                            
                        }
                        else if (palat[0] == "DATA")
                        {
                            System.Console.WriteLine("Vastustaja veikkasi numeroa " + palat[1]);
                            Laheta(palvelin, Pep, "ACK 300");
                            KysyNum(palvelin, Pep);
                        }
                        else if (palat[0] == "QUIT")
                        {
                            if (palat[1] == "501")
                            {
                                System.Console.WriteLine(palat[2]);
                                Laheta(palvelin, Pep, "ACK 500");
                                return;
                            }
                            else if (palat[1] == "502")
                            {
                                System.Console.WriteLine(palat[2]);
                                Laheta(palvelin, Pep, "ACK 500");
                                return;
                            }
                        }
                        break;
                } 

            } // while
        }
        public static void KysyNum(Socket s, EndPoint ep)
        {
            Console.Write("Anna numero: ");
            String luku = Console.ReadLine();
            Laheta(s, ep, "DATA " + luku);
        }
        public static void Laheta(Socket s, EndPoint ep, String msg)
        {
            s.SendTo(Encoding.ASCII.GetBytes(msg), ep);
        }

        public static String[] Vastaanota(Socket s) 
        {
            byte[] rec = new byte[256];
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Palvelinep = (EndPoint)remote;
            int montakoMerkkia = s.ReceiveFrom(rec, ref Palvelinep);
            String recString = Encoding.ASCII.GetString(rec, 0, montakoMerkkia);
            // String[] osat = recString.Split(' ', 2);
            String[] osat = recString.Split(' ', 3);
            return osat;
        }
    }
}

