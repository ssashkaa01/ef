using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace _05._04._2020
{
    [Serializable]
    public class FileDetails
    {
        public string FILETYPE = "";
        public long FILESIZE = 0;
    }

    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog1 { get; set; }
        private List<string> files { get; set; }

        private const int remotePort = 11001;
        private static FileDetails fileInfo = new FileDetails();
        private static IPAddress remoteIPAddress = null;
        private static UdpClient server = new UdpClient();
        private static IPEndPoint endPoint = null;
        //private static FileStream fs;

        public MainWindow()
        {
            InitializeComponent();
            InitializeOpenFileDialog();

            files = new List<string>();
        }

        private void InitializeOpenFileDialog()
        {
            this.openFileDialog1 = new OpenFileDialog();

            // Allow the user to select multiple images.
            this.openFileDialog1.Multiselect = true;

            this.openFileDialog1.Title = "Files";
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    FileInfo fi = new FileInfo(file);

                    if (fi.Length > 8192)
                    {
                        System.Windows.MessageBox.Show($"{file} is big! size has been < 8 kb");
                        continue;
                    }

                    files.Add(file);
                }

                listBoxFiles.ItemsSource = files;
                listBoxFiles.Items.Refresh();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
              
                remoteIPAddress = IPAddress.Parse("127.0.0.1");
                endPoint = new IPEndPoint(remoteIPAddress, remotePort);

                foreach(string file in files)
                {
                    Thread.Sleep(3000);
                    FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    
                    SendFileInfo(fs);
                    Thread.Sleep(2000);
                    SendFile(fs);

                    fs.Close();

                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        public static void SendFileInfo(FileStream fs)
        {
            fileInfo.FILETYPE = System.IO.Path.GetExtension(fs.Name).Substring(1);
            fileInfo.FILESIZE = fs.Length;

            XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));

            using (MemoryStream stream = new MemoryStream())
            {
                fileSerializer.Serialize(stream, fileInfo);
                stream.Position = 0;
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
                
                server.Send(bytes, bytes.Length, endPoint);
            }

        }
        private static bool SendFile(FileStream fs)
        {

            Byte[] bytes = new Byte[fs.Length];

            fs.Read(bytes, 0, bytes.Length);
            
            try
            {
                server.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                return false;
            }
           

            return true;
        }
    }
}
