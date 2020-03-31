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

    class AsyncServer
    {
        IPEndPoint endP;
        Socket socket;
        private DALClass _dal;

        public AsyncServer(string strAddr, int port)
        {
            _dal = new DALClass();
            endP = new IPEndPoint(IPAddress.Parse(strAddr), port);
        }

        void MyAcceptCallbakFunction(IAsyncResult ia)
        {
            //get a link to the listening socket
            Socket socket = (Socket)ia.AsyncState;

            //get a socket to exchange data with the client
            Socket ns = socket.EndAccept(ia);

            //output the connection information to the console
            Console.WriteLine(ns.RemoteEndPoint.ToString());

            //send the client the current time asynchronously, 
            //by the end of the sending operation, the  
            //MySendCallbackFunction method will be called.


            string req = "";
            byte[] bytes;
            do
            {
                bytes = new byte[1024];
                int bytesRec = ns.Receive(bytes);
                req += Encoding.UTF8.GetString(bytes, 0, bytesRec);

            } while (ns.Available > 0);

            Console.WriteLine($"Request : {req} from machine: {ns.RemoteEndPoint}");

            byte[] msg;

            try
            {
                int index = Convert.ToInt32(req);
                IQueryable<Street> res = _dal.GetStreets(35000);
                
                if (res.Count() > 0)
                {
                    string str = "";
                    foreach (var item in res)
                    {
                        str += item.Name + " |";
                    }

                    msg = Encoding.UTF8.GetBytes(str);
                }
                else
                {
                    msg = Encoding.UTF8.GetBytes("Streets not found");
                }
            }
            catch (Exception ex)
            {
                msg = Encoding.UTF8.GetBytes(ex.Message);
               

   
            }

            //Thread.Sleep(1500);
            

            ns.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(MySendCallbackFunction), ns);

            

            //resume asynchronous Accept
            socket.BeginAccept(new AsyncCallback(MyAcceptCallbakFunction), socket);
        }
        void MySendCallbackFunction(IAsyncResult ia)
        {
            //after sending data to the client,
            //close the socket (if we would need to continue
            //data exchange, we could have arranged it here)
            Socket ns = (Socket)ia.AsyncState;
            int n = ((Socket)ia.AsyncState).EndSend(ia);
            //ns.Shutdown(SocketShutdown.Send);
            ns.Close();
        }

        public void StartServer()
        {
            if (socket != null)
                return;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            socket.Bind(endP);
            socket.Listen(10);

            Console.WriteLine("Waiting for connection...");
            //start asynchronous Accept, when the client 
            //is connected, the MyAcceptCallbakFunction handler is called.
            socket.BeginAccept(new AsyncCallback(MyAcceptCallbakFunction), socket);
        }
    }

    class Program
    {        
        static void Main(string[] args)
        {
            AsyncServer server = new AsyncServer("127.0.0.1", 1024);
            server.StartServer();
            Console.Read();
        }
    }
}