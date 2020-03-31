using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

namespace WpfAppScreenshot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool IsStarted { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            IsStarted = false;
            btnStop.IsEnabled = false;
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            int PORT;

            try
            {
                PORT = Convert.ToInt32(textPORT.Text);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            IsStarted = true;
            textPORT.IsEnabled = false;
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;

            await Task.Run(() =>
            {
                StartListener(PORT);
            });
           
        }

        private void StartListener(int port)
        {
           
            UdpClient listener = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);

            try
            {
                while (IsStarted)
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                  
                    MemoryStream byteStream = new MemoryStream(bytes);

                    this.Dispatcher.Invoke(() =>
                    {

                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = byteStream;
                        image.EndInit();
                        
                        imageBox.Source = image;
                    });
                   
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            textPORT.IsEnabled = true;
            IsStarted = false;
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }
    }
}
