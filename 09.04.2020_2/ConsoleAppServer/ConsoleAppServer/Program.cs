using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleAppServer
{
    public class Program
    {
        static TcpListener server { get; set; }
        static int randomNumber { get; set; }
        static Random rnd = new Random();
        static IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        static TcpClient client { get; set; }
        static Regex rgx = new Regex(@"^[0-9-]{1,3}$");
        static int port = 11001;
        
        static void GenerateNumber()
        {
            randomNumber = rnd.Next(1, 100);
            Console.WriteLine($"Загадане число - {randomNumber}");
        }

        static void Main(string[] args)
        {
            GenerateNumber();

            try
            {
                server = new TcpListener(localAddr, port);
                server.Start();

                ThreadGame();

               // Thread thread = new Thread(new ThreadStart(ThreadGame));
               // thread.IsBackground = true;              
               // thread.Start();

            }
            catch (SocketException sockEx)
            {
                Console.WriteLine("Socket exception: " + sockEx.Message);
            }
            catch (Exception Ex)
            {
                Console.WriteLine("Exception : " + Ex.Message);
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }

            Console.ReadKey();
        }

        static void ThreadGame()
        { 
            while (true)
            {
                try
                {
                    Console.WriteLine("Ожидание подключений... ");

                    // получаем входящее подключение
                    client = server.AcceptTcpClient();
                    Console.WriteLine("Подключен клиент. Выполнение запроса...");

                    NetworkStream stream = client.GetStream();

                    byte[] Data = new Byte[256]; ;

                    Int32 bytes = stream.Read(Data, 0, Data.Length);
                    string message = System.Text.Encoding.Unicode.GetString(Data, 0, bytes).Trim();

                    Console.WriteLine($"Получено: {message}");

                    string response = "";

                     if (!rgx.IsMatch(message))
                    {
                            response = "Неправильне значення!"; 
                    }
                     else
                    {
                        int result = Convert.ToInt32(message);

                        if (result == randomNumber)
                        {
                            response = "Ви вгадали число!";
                            GenerateNumber();
                        }
                        else if (result > randomNumber)
                        {
                            response = "Загадане число менше";
                        }
                        else if (result < randomNumber)
                        {
                            response = "Загадане число  більше";
                        }
                    }
             
                    // преобразуем сообщение в массив байтов
                    byte[] data = Encoding.Unicode.GetBytes(response);

                    // отправка сообщения
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Отправлено сообщение: {0}", response);

                    // закрываем поток
                    stream.Close();

                    // закрываем подключение
                    client.Close();
                }catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                
            }
        }
        
    }
}
