using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace teht04ja05
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket s = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            try
            {
                s.Connect("localhost", 25000);
            }
            catch (Exception ex)
            {
                Console.Write("Virhe: " + ex.Message);
                Console.ReadKey();
                return;
            }
            

            NetworkStream ns = new NetworkStream(s);
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);

            // String email = "testi posti";

            Boolean on = true;
            String viesti = "";
            while (on)
            {
                // System.Console.WriteLine(viesti);
                // luetaan 
                viesti = sr.ReadLine();
                String[] status = viesti.Split(' ');

                switch (status[0])
                {
                    case "220":
                        sw.WriteLine("HELO jyu.fi");
                        break;
                    case "250":
                        switch (status[1])
                        {
                            case "2.0.0":
                                // System.Console.WriteLine("2.0.0");
                                sw.WriteLine("QUIT");
                                break;
                            case "2.1.0":
                                // System.Console.WriteLine("2.1.0");
                                Console.Write("Vastaanottajan osoite: ");
                                String vastaanottajanMail = Console.ReadLine();
                                sw.WriteLine("RCPT TO: " + vastaanottajanMail);
                                break;
                            case "2.1.5":
                                // System.Console.WriteLine("2.1.5");
                                sw.WriteLine("DATA:");
                                break;
                            default: //
                                // MAIL FROM 
                                Console.Write("Lähettäjän osoite: ");
                                String lahettajanMail = Console.ReadLine();
                                sw.WriteLine("MAIL FROM: " + lahettajanMail);
                                // sw.WriteLine("QUIT");
                                break;
                        } // switch
                        break;
                    case "221":
                        on = false;
                        break;
                    case "354":
                        // System.Console.WriteLine("354");
                        Console.WriteLine("Mailin sisältö (päätä '.' merkkiin, yksin rivillä): ");
                        String uusiRivi = "";
                        String sisalto = "";
                        while (uusiRivi != ".") {
                            uusiRivi = Console.ReadLine();
                            if (uusiRivi != ".")
                                sisalto += uusiRivi + "\n";
                            else 
                                sisalto += uusiRivi;
                        }
                        sw.WriteLine(sisalto);
                        break;
                    default:
                        System.Console.WriteLine("Virhe...");
                        sw.WriteLine("QUIT");
                        break;
                } // switch
                sw.Flush();
            } // while

            // Console.ReadKey();

            sw.Close();
            sr.Close();
            ns.Close();
            s.Close();
        }
    }
}

