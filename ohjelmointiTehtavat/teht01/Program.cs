using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace teht01
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork,
                                  SocketType.Stream,
                                  ProtocolType.Tcp);

            // s.Connect("localhost", 25000);
            // String snd = "GET / HTTP/1.1\nHost: localhost\n\n";
            s.Connect("www.example.com", 80);
            String snd = "GET / HTTP/1.1\nHost: www.example.com\nConnection:Close\n\n";
            
            byte[] buffer = Encoding.ASCII.GetBytes(snd);
            s.Send(buffer);
            String sivu = "";

            int count = 0;

            do
            {
                try
                {
                    byte[] rec = new byte[1024];
                    count = s.Receive(rec);
                    // System.Console.WriteLine("Tavuja vastaanotettu: " + count);
                    sivu += Encoding.ASCII.GetString(rec, 0, count);
                }
                catch
                {
                    Console.WriteLine(sivu);
                    throw;
                }
            } while (count > 0);

            //leikataan otsikkorivit pois
            sivu = sivu.Substring(sivu.IndexOf("\r\n\r\n"));

            Console.WriteLine(sivu);
            // Console.ReadKey();
            s.Close();
        }
    }
}

