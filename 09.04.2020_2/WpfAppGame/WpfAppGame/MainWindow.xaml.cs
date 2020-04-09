using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPAddress server = IPAddress.Parse("127.0.0.1");
        int port = 11001;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(server, port);
                TcpClient client = new TcpClient();

                client.Connect(endPoint);

                using (NetworkStream stream = client.GetStream())
                {

                    Byte[] Data = System.Text.Encoding.Unicode.GetBytes(textValue.Text);
                    stream.Write(Data, 0, Data.Length);

                    Data = new Byte[256];
                    Int32 bytes = stream.Read(Data, 0, Data.Length);
                    string response = System.Text.Encoding.Unicode.GetString(Data, 0, bytes).Trim();

                    MessageBox.Show(response);
                }

                client.Close();

            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show($"{ex}");
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"{ex}");
            }
        }
    }
}
