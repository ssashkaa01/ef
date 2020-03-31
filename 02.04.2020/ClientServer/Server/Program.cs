using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Database;

namespace Server
{    
    class Server
    {
        private DALClass _dal;
        public Server()
        {
            _dal = new DALClass();
        }

        public void Start()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1024);


            Socket listener = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Stream,
                                         ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(0);

                byte[] bytes = null;
                string req = "";
                int index;

                do
                {
                    Console.WriteLine("Waiting for a request...");

                    Socket handler = listener.Accept();

                    Thread.Sleep(2000);

                    req = "";
                    do
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        req += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    } while (handler.Available > 0);

                    Console.WriteLine($"Request : {req} from machine: {handler.RemoteEndPoint}");

                    try
                    {
                        index = Convert.ToInt32(req);
                    }
                    catch (Exception ex)
                    {
                        byte[] msgs = Encoding.UTF8.GetBytes(ex.Message);
                        handler.Send(msgs);

                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();

                        continue;
                        throw;
                    }

                    //Thread.Sleep(1500);
                    IQueryable<Street> res = _dal.GetStreets(index);

                    byte[] msg;

                    if (res.Count() > 0)
                    {
                        string str = "";
                        foreach(var item in res)
                        {
                            str += item.Name + " |";
                        }

                        msg = Encoding.UTF8.GetBytes(str);
                    }
                    else
                    {
                        msg = Encoding.UTF8.GetBytes("Streets not found");
                    }
                    
                    handler.Send(msg);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                } while (req != "RussiaBye");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
    }

    class Program
    {        
        static void Main(string[] args)
        {
            new Server().Start();
        }
    }
}