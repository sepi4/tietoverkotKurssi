using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace teht06
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket s = null;
            int port = 9999;
            IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, port);
            List<EndPoint> asiakkaat = new List<EndPoint>();

            try
            {
                s = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Udp);
                s.Bind(iep);
            }
            catch (Exception ex)
            {
                Console.Write("Virhe: " + ex.Message);
                Console.ReadKey();
                return;
            }
            System.Console.WriteLine("Odotetaan asiakasta...");

            while (!Console.KeyAvailable)
            {
                byte[] rec = new byte[256];
                IPEndPoint asiakas = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (EndPoint)asiakas;
                int received = s.ReceiveFrom(rec, ref remote);

                String recString = Encoding.ASCII.GetString(rec, 0, received);
                char[] delim = {';'};
                String[] palat = recString.Split(delim, 2);
                if (palat.Length < 2)
                {
                    //laheta virheviesti
                    System.Console.WriteLine("Virheellinen viesti asiakkaalta: [{0}:{1}]", ((IPEndPoint)remote).Address, ((IPEndPoint)remote).Port);
                    s.SendTo(Encoding.ASCII.GetBytes("virhe"), remote);
                }
                else
                {
                    if (!asiakkaat.Contains(remote))
                    {
                        asiakkaat.Add(remote);
                        System.Console.WriteLine("Uusi asiakas: [{0}:{1}]", ((IPEndPoint)remote).Address, ((IPEndPoint)remote).Port);
                    }
                    System.Console.WriteLine("{0}: {1}", palat[0], palat[1]);

                    foreach (var client in asiakkaat)
                    {
                       //String kaikille = palat[0] + ": " + palat[1];
                       s.SendTo(Encoding.ASCII.GetBytes(recString), client);
                    }
                }
            }
            Console.ReadKey();
            s.Close();
        }
    }
}
