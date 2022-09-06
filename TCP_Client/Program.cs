using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;

namespace TCP_Cient
{
    class Program
    {
        static void send()
        {
            bool exitclient = false;
            while (!exitclient)
            {
                TcpClient client =null;
                NetworkStream stream = null;
                try
                {
                    client = new TcpClient("192.168.2.58", 1302);
                    stream = client.GetStream();
                    while (!exitclient)
                    {
                        Console.WriteLine("Write a message: ");
                        string messageToSend = Console.ReadLine();
                        int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
                        byte[] sendData = Encoding.ASCII.GetBytes(messageToSend);


                        stream.Write(sendData, 0, sendData.Length);
                        if(messageToSend == "exit")
                        {
                            exitclient = true;                           
                        }
                    }
                    //StreamReader sr = new StreamReader(stream);
                    //string response = sr.ReadLine();
                    //Console.WriteLine(response);
                   
                    //Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine("failed to connect...");
                }
                finally
                {
                    if(stream != null)
                    {
                        stream.Close();
                        
                    }
                    if(client !=null)
                    {
                        client.Close();
                    }
                }
            }
        }

        static void recieve()
        {
            while (true)
            {
                try
                {
                    TcpListener listener = new TcpListener(IPAddress.Parse("192.168.2.37"), 1301);
                    listener.Start();
                    //Console.WriteLine("Waiting for a connection.");
                    TcpClient client = listener.AcceptTcpClient();
                    //Console.WriteLine("User2 accepted.....\n");
                    NetworkStream stream = client.GetStream();
                    StreamReader sr = new StreamReader(client.GetStream());
                    StreamWriter sw = new StreamWriter(client.GetStream());
                    byte[] buffer = new byte[1024];
                    int recv=stream.Read(buffer, 0, buffer.Length);

                    if (recv > 0)
                    {
                        string request = Encoding.UTF8.GetString(buffer, 0, recv);
                        Console.WriteLine("User2 >> " + request);
                    }
                    stream.Close();
                    client.Close();
                    listener.Stop();
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong in stream.");
                    //sw.WriteLine(e.ToString());
                }
            }
        }

        static void Main(string[] args)
        {
            Thread sendingThread = new Thread(send);
            Thread recievingThread = new Thread(recieve);
            sendingThread.Start();
            recievingThread.Start();
        }

    }
}