using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace teht10ja11ja12
{
    class Program
    {
        public static int Flip(int i){
            return 1 - i;
        }
        public static void Laheta(Socket s, EndPoint ep, String msg)
        {
            s.SendTo(Encoding.ASCII.GetBytes(msg), ep);
        }
        public static String[] Vastaanota(Socket s, ref EndPoint remote) 
        {
            byte[] rec = new byte[256];
            int montakoMerkkia = s.ReceiveFrom(rec, ref remote);
            String recString = Encoding.ASCII.GetString(rec, 0, montakoMerkkia);
            System.Console.WriteLine("Viesti portista {0}: \"{1}\"", remote.ToString(), recString);
            String[] osat = recString.Split(' ', 3);
            return osat;
        }
        static void Main(string[] args)
        {
            Socket palvelin;
            IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, 9999);
            try
            {
                palvelin = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                palvelin.Bind(iep);
            }
            catch
            {
                return;
            }
            String STATE = "WAIT";
            System.Console.WriteLine("Tila: WAIT");

            Boolean on = true;
            int vuoro = -1;
            int luku = -1;
            int pelaajaMaara = 0;
            int quit_ACK = 0;
            EndPoint[] pelaajat = new EndPoint[2];
            String[] nimet = new String[2];

            while (on)
            {
                // paketin vastaan otto
                IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint)client;
                String[] kehys = Vastaanota(palvelin, ref remote); 

                switch (STATE)
                {
                    case "WAIT":
                        switch (kehys[0])
                        {
                            case "JOIN":
                                if (pelaajaMaara >= 2)
                                {
                                    Laheta(palvelin, remote, "ACK 400 pelissa on jo 2 pelajaa");
                                    break;
                                }
                                pelaajat[pelaajaMaara] = remote;
                                nimet[pelaajaMaara] = kehys[1];
                                pelaajaMaara++;
                                if (pelaajaMaara == 1)
                                    Laheta(palvelin, remote, "ACK 201 JOIN OK");
                                else if (pelaajaMaara == 2)
                                {
                                    Random rand = new Random();
                                    int aloittaja = rand.Next(0,1);
                                    vuoro = aloittaja;
                                    luku = rand.Next(0, 10);
                                    System.Console.WriteLine("Oikea numero: " + luku);
                                    Laheta(palvelin, pelaajat[aloittaja], "ACK 202 " + nimet[Flip(aloittaja)]);
                                    Laheta(palvelin, pelaajat[Flip(aloittaja)], "ACK 203 " + nimet[aloittaja]);
                                    STATE = "GAME";
                                    System.Console.WriteLine("Tila: GAME");
                                }
                                break;
                            default:
                                Laheta(palvelin, remote, "ACK 404 Vaara kehysrakenne");
                                break;
                        } // switch (kehys[0])
                            
                        break;
                    case "GAME":
                        switch (kehys[0])
                        {
                            case "DATA":
                                int arvaus;
                                try
                                {
                                    arvaus = Int32.Parse(kehys[1]);
                                }
                                catch
                                {
                                    Laheta(palvelin, remote, "ACK 407 arvauksesi ei ollut numero");
                                    break;
                                }
                                
                                // oikein arvaus
                                if (arvaus == luku)
                                {
                                    Laheta(palvelin, remote, "QUIT 501 "+luku+" oli oikea numero. Voitit!");
                                    Laheta(palvelin, pelaajat[Flip(vuoro)], "QUIT 502 "+luku+" oli oikea numero. Vastustaja arvasi oikein. Hävisit!");
                                    STATE = "END";
                                    System.Console.WriteLine("Tila: " + STATE);
                                }
                                else // vaarin arvaus
                                {
                                    Laheta(palvelin, pelaajat[vuoro], "ACK 300 DATA OK");
                                    Laheta(palvelin, pelaajat[Flip(vuoro)], "DATA " + arvaus);
                                    vuoro = Flip(vuoro);
                                    STATE = "WAIT_ACK";
                                    System.Console.WriteLine("Tila: " + STATE);
                                }
                                break;
                            default:
                                Laheta(palvelin, remote, "ACK 400 VIRHE");
                                break;
                        } // "GAME" switch (kehys[0])
                        break;
                    case "WAIT_ACK":
                        if (kehys[1] == "300")
                        {
                            STATE = "GAME";                            
                            System.Console.WriteLine("Tila: " + STATE);
                        }
                        else {
                            Laheta(palvelin, remote, "ACK 404 Vaara kehysrakenne");
                        }
                        break;
                    case "END":
                        if (kehys[0] == "ACK" && kehys[1] == "500")
                        {
                            quit_ACK++;
                            if (quit_ACK == 2)
                            {
                                STATE = "CLOSED";
                                System.Console.WriteLine("Tila: CLOSED");
                                return;
                            }
                        }
                        break;
                    default:
                        System.Console.WriteLine("ACK 400 VIRHE");
                        break;
                }
            }
        }
    }
}
