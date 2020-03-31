using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

namespace WpfAppClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                LoadStreets(); 
            });
        }

        private void LoadStreets()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1"); //Dns.GetHostAddresses("microsoft.com")[0];
            IPEndPoint ep = new IPEndPoint(ip, 1024);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                s.Connect(ep);
                if (s.Connected)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        textResult.Text = "";

                        String strSend = textMsg.Text;
                        s.Send(System.Text.Encoding.UTF8.GetBytes(strSend));
                    });
                    
                    byte[] buffer = new byte[1024];
                    int l;
                    do
                    {
                        l = s.Receive(buffer);
                        this.Dispatcher.Invoke(() =>
                        {
                            textResult.Text += System.Text.Encoding.UTF8.GetString(buffer, 0, l);
                        });
                    } while (l > 0);
                }
                else
                    MessageBox.Show("Connection Error");
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                s.Shutdown(SocketShutdown.Both);
                s.Close();
            }
        }
    }
   
}
