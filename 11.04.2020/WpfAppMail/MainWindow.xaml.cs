using EAGetMail;
using EASendMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


namespace WpfAppMail
{
   
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog1 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeOpenFileDialog();

            textServer.Text = "mail.storym.pro";
            textPort.Text = "465";
            textEmail.Text = "demo@storym.pro";
            textPassword.Password = "R3n9H8x1";

            textTo.Text = "ssashkaa01@gmail.com";
            textTheme.Text = "Bla bla text";
            textMessage.Text = "Test Message from C#";


        }

        private void InitializeOpenFileDialog()
        {
            this.openFileDialog1 = new OpenFileDialog();
            // Set the file dialog to filter for graphics files.
            //this.openFileDialog1.Filter =
            //    "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|" +
            //    "All files (*.*)|*.*";

            // Allow the user to select multiple images.
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Select files";
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            SmtpServer server = new SmtpServer(textServer.Text)
            {
                Port = Convert.ToInt32(textPort.Text),
                ConnectType = SmtpConnectType.ConnectSSLAuto,
                User = textEmail.Text,
                Password = textPassword.Password
            };

            SmtpMail message = new SmtpMail("TryIt") // trial licence
            {
                From = textEmail.Text,
                To = textTo.Text,
                Subject = textTheme.Text,
                TextBody = textMessage.Text,
                Priority = EASendMail.MailPriority.High
            };

            if(listBoxFiles.Items.Count > 0)
            {
                foreach(string file in listBoxFiles.Items)
                {
                    message.AddAttachment(file);
                }
            }

            SmtpClient client = new SmtpClient();
            //client.Connect(server);
            try
            {
                //Console.WriteLine("Try to send mail...");

                client.SendMail(server, message);

                System.Windows.MessageBox.Show("Message is sent!");

                //Console.WriteLine("Message is sent");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                //Console.WriteLine(ex.Message);
            }
        }

        private void BtnAttachFiles_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dr = this.openFileDialog1.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    listBoxFiles.Items.Add(file);
                }
            }
        }

        private void BtnClearAttachFiles_Click(object sender, RoutedEventArgs e)
        {
            listBoxFiles.Items.Clear();
        }

        private void BtnUpdateMessages_Click(object sender, RoutedEventArgs e)
        {
            listBoxMessages.Items.Clear();

            MailServer server = new MailServer(
                textServer.Text,
                textEmail.Text,
                textPassword.Password,
                EAGetMail.ServerProtocol.Imap4)
            {
                SSLConnection = true,
                Port = 993
            };

            MailClient client = new MailClient("TryIt");

            try
            {
                client.Connect(server);

                var messages = client.GetMailInfos();

                foreach (var m in messages)
                {
                    Mail message = client.GetMail(m);

                    listBoxMessages.Items.Add($"From: {message.From}\n\n\t{message.Subject}\n");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }




        }
    }
}
