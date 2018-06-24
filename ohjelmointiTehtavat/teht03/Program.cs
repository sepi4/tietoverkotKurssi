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
        soketti.Connect("localhost", 25000);

        // Luo lähetettävä String tyyppinen viesti ja muuta se tavutaulukoksi
        Console.Write("syöte palvelimelle: ");
        String viesti = Console.ReadLine() + "\n";

        byte[] tavutaulukko = Encoding.ASCII.GetBytes(viesti);
        // Lähetä viesti tavuina soketin Send -metodilla
        soketti.Send(tavutaulukko);
        byte[] rec = new byte[2222]; // tavutaulukko vastaanotetulle datalle
        int paljon = soketti.Receive(rec);  // Lisää vastaanottometodi, jolle parametrina tavutaulukko
        String vastaus = System.Text.Encoding.ASCII.GetString(rec, 0, paljon);

        String palvelin = vastaus.Substring(0, vastaus.IndexOf(';'));
        String teksti = vastaus.Substring(vastaus.IndexOf(';') + 1);
        Console.WriteLine("palvelimelta: " + palvelin);
        Console.WriteLine("teksti: " + teksti);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());
      }
      soketti.Close();
    }
  }
}
