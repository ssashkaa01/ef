using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace WpfAppDownloadFile
{

    public class DownloadFile
    {
        public WebClient webClient { get; set; }
        public string downloadPath { get; set; }
        public int percentDownload { get; set; }

        public DownloadFile(string pathFolderDownloads, string nameFile)
        {
            downloadPath = $"{pathFolderDownloads}\\download_file_{GetTimestamp(DateTime.Now)}{System.IO.Path.GetExtension(nameFile)}";

            webClient = new WebClient();
            percentDownload = 0;
        }

        public string GetFileName()
        {
            return System.IO.Path.GetFileName(downloadPath);
        }

        public override string ToString()
        {
            return $"{GetFileName()} | Downloaded: {percentDownload}%";
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }

    public partial class MainWindow : Window
    {
        public string pathFolderDownloads { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        // Стартуємо завантаження
        private async void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            if(pathFolderDownloads == "" || pathFolderDownloads == null)
            {
                MessageBox.Show("Please select folder for save!");
                return;
            }

            //textUrl.Text = "http://212.183.159.230/5MB.zip";

            try
            {
                DownloadFile df = new DownloadFile(pathFolderDownloads, textUrl.Text);

                Uri uri = new Uri(textUrl.Text);

                int index = listDownloads.Items.Add(df);

                // Подія на завершення завантаження
                df.webClient.DownloadFileCompleted += (s, ev) =>
                {
                    DownloadFile tmpDf = (DownloadFile)listDownloads.Items[index];

                    MessageBox.Show($"File \"{tmpDf.GetFileName()}\" downloaded!");
                };

                // Подія на зміну відсоткового завантаження
                df.webClient.DownloadProgressChanged += (s, ev) =>
                {
                    DownloadFile tmpDf = (DownloadFile)listDownloads.Items[index];

                    tmpDf.percentDownload = ev.ProgressPercentage;

                    listDownloads.Items.Refresh();
                };

                await df.webClient.DownloadFileTaskAsync(uri, df.downloadPath);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pathFolderDownloads = folderBrowserDialog.SelectedPath;
                labelPathFolderDownloads.Content = pathFolderDownloads;
            }
        }
    }
}
