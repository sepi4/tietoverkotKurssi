using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;


namespace teht02
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket Palvelin = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            
            // IPEndPoint iep = new IPEndPoint(IPAddress.Any, 25000); // kukatahansa
            IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, 25000); // vain oma kone
            Palvelin.Bind(iep);

            //kunnellaan asiakkaita
            Palvelin.Listen(5); // jonossa saa olla

            while(true) {
                // koodi pysahtyy, palvelin odottaa yhteytta, palautaa asiakkaan socketti
                Socket Asiakas = Palvelin.Accept(); 

                IPEndPoint iap = (IPEndPoint)Asiakas.RemoteEndPoint;

                System.Console.WriteLine("Yhteys osoitteesta: {0}, portista {1}", iap.Address, iap.Port);

                NetworkStream ns = new NetworkStream(Asiakas);
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);

                String rec = sr.ReadLine();

                sw.WriteLine("Sergein Palvelin;" + rec);
                sw.Flush(); // laittaa datan eteenpain
                Asiakas.Close();
            }


            // Console.ReadKey();
            // Palvelin.Close();

        }
    }
}
