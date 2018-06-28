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
        public static String[] Vastaanota(Socket s, ref EndPoint remote) 
        {
            byte[] rec = new byte[256];
            // IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            // EndPoint Palvelinep = (EndPoint)remote;
            // int montakoMerkkia = s.ReceiveFrom(rec, ref Palvelinep);
            int montakoMerkkia = s.ReceiveFrom(rec, ref remote);
            String recString = Encoding.ASCII.GetString(rec, 0, montakoMerkkia);
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
            Boolean on = true;
            int vuoro = -1;
            int Pelaajat = 0;
            int Quit_ACK = 0;
            EndPoint[] pelaajat = new EndPoint[2];
            String[] nimet = new String[2];

            while (on)
            {
                IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint)client;
                String[] kehys = Vastaanota(palvelin, ref remote);
                switch (STATE)
                {
                    case "WAIT":
                        break;
                    case "GAME":
                        break;
                    case "WAIT_ACK":
                        break;
                    case "END":
                        break;
                    default:
                        System.Console.WriteLine("VIRHE");
                        break;
                }
            }
        }
    }
}
