using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleAppScreenshotClient
{
    class Program
    {
        private static int IntervalSend { get; set; }
        private static string ServerIp { get; set; }
        private static int ServerPort { get; set; }

        static void Main()
        {
            ServerIp = "127.0.0.1";
            ServerPort = 12345;
            IntervalSend = 5000;

            StartSender();
        }

        private static void StartSender()
        {

            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort);

                bool done = false;
                while (!done)
                {
                    Thread.Sleep(IntervalSend);

                    Bitmap bmp = new Bitmap(200, 200);
                    //Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                    }

                    ImageConverter converter = new ImageConverter();
                    byte []image = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                    Console.WriteLine("Take screenshot!");

                    try
                    {
                        s.SendTo(image, ep);
                        Console.WriteLine("Screenshot sent!");
                    } catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    

                    

                }
            }
        }
    }
}
