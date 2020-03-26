using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleAppServer
{
    public class Program
    {
        [Serializable]
        public class FileDetails
        {
            public string FILETYPE = "";
            public long FILESIZE = 0;

            public override string ToString() => $"FileType {FILETYPE} size {FILESIZE}";


        }

        private static FileDetails fileInfo;
        private static int localPort = 11001;
        private static UdpClient client = new UdpClient(localPort);
        private static IPEndPoint RemoteIpEndPoint = null;

        private static FileStream fs;
        private static byte[] receiveBytes = null;
        static void Main(string[] args)
        {
            while(true)
            {
                if(fs == null)
                {

                    GetFileDetails();

                    ReceiveFile();

                }

              
            }

            client.Close();
        }
        private static void GetFileDetails()
        {
            try
            {
                Console.WriteLine("waiting for info...");
                receiveBytes = client.Receive(ref RemoteIpEndPoint);
                Console.WriteLine("info has come!");

                XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));
                using (MemoryStream stream1 = new MemoryStream())
                {

                    stream1.Write(receiveBytes, 0, receiveBytes.Length);
                    stream1.Position = 0;
                    fileInfo = (FileDetails)fileSerializer.Deserialize(stream1);
                    Console.WriteLine($"File {fileInfo}");

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void ReceiveFile()
        {
            try
            {
                Console.WriteLine("waition for a file ...");
                receiveBytes = client.Receive(ref RemoteIpEndPoint);
                Console.WriteLine("I have a file. Save it!");
                fs = new FileStream(@"C:\Users\SSASHKOO\Desktop\file_from_server_" + GetTimestamp(DateTime.Now) + "." + fileInfo.FILETYPE, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Write(receiveBytes, 0, receiveBytes.Length);
                Console.WriteLine("File is saved");
                //Process.Start(fs.Name);
            }
            catch (Exception eR)
            {
                Console.WriteLine(eR.ToString());
            }
            finally
            {
                fs.Close();
                fs = null;
            }
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
