using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;


namespace teht03
{
  class Program
  {
    static void Main(string[] args)
    {
      Socket soketti = null;
      try
      {

        soketti = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // Luo soketti
        // Yhdistä soketti
        // soketti.Connect("www.example.com", 80);
        soketti.Connect("localhost", 25000);

        // Luo lähetettävä String tyyppinen viesti ja muuta se tavutaulukoksi
        // String viesti = "GET / HTTP/1.1\r\nHost: www.example.com\r\nConnection:Close\r\n\r\n";
        String viesti = "kissakissakissa\n";

        byte[] tavutaulukko = Encoding.ASCII.GetBytes(viesti);
        // Lähetä viesti tavuina soketin Send -metodilla
        soketti.Send(tavutaulukko);
        byte[] rec = new byte[2222]; // tavutaulukko vastaanotetulle datalle
        int paljon = soketti.Receive(rec);  // Lisää vastaanottometodi, jolle parametrina tavutaulukko
        String vastaus = System.Text.Encoding.ASCII.GetString(rec, 0, paljon);

        Console.WriteLine("Soketti luotiin onnistuneesti seuraavin ominaisuuksin:\r\n"
        + "AddressFamily = " + soketti.AddressFamily.ToString() + "\r\nSocketType = "
        + soketti.SocketType.ToString() + "\r\nProtocolType = " + soketti.ProtocolType.ToString());
        IPEndPoint Aiep = (IPEndPoint)soketti.RemoteEndPoint;
        Console.WriteLine("Soketti yhdistettiin palvelimeen: {0} porttiin {1}", Aiep.Address, Aiep.Port);
        Console.WriteLine("Vastaus palvelimelta:\r\n" + vastaus);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      finally
      {
        if (soketti != null)
        {
          // pyrkii varmistamaan että molemmat osapuolet
          // ehtivät lähettää kaiken datan
          if (soketti.Connected) soketti.Shutdown(SocketShutdown.Both);
          // suljetaan soketti ja vapautetaan resurssit
          soketti.Close();
          Console.WriteLine("Soketti suljettiin onnistuneesti");
        }
      }




      // Console.Write("syöte: ");
      // String syote = Console.ReadLine();
      // Console.WriteLine();
      // Console.WriteLine("palvelin: " + syote);
      // Console.WriteLine("teksti: " + syote);

    }
  }
}
