using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppDownloadFile
{

    /*
     
     кількість потоків для скачування,
     Користувач може призупинити завантаження,
     */

    public partial class MainWindow : Window
    {
        public string pathFolderDownloads { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DisableAllButton();

            pathFolderDownloads = @"C:\Users\SSASHKOO\Downloads";

            textUrl.Text = "http://212.183.159.230/20MB.zip";
            labelPathFolderDownloads.Content = pathFolderDownloads;
        }

        // Стартуємо завантаження
        private async void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            if(pathFolderDownloads == "" || pathFolderDownloads == null)
            {
                MessageBox.Show("Please select folder for save!");
                return;
            }
            
            try
            {
                DownloadFile df = new DownloadFile(pathFolderDownloads, textUrl.Text);

                Uri uri = new Uri(textUrl.Text);

                int index = listDownloads.Items.Add(df);

                // Подія на завершення завантаження
                df.webClient.DownloadFileCompleted += (s, ev) =>
                {
                    try
                    {

                        DownloadFile tmpDf = (DownloadFile)listDownloads.Items[index];

                        this.Dispatcher.Invoke(() =>
                        {
                            if (listDownloads.SelectedIndex == index)
                            {
                                UpdateButtonForListItem(tmpDf);
                            }
                        });

                        MessageBox.Show($"File \"{tmpDf.GetFileName()}\" downloaded!");

                    }
                    catch (Exception) {}

                };

                // Подія на зміну відсоткового завантаження
                df.webClient.DownloadProgressChanged += (s, ev) =>
                {
                    try
                    {
                        DownloadFile tmpDf = (DownloadFile)listDownloads.Items[index];

                        tmpDf.percentDownload = ev.ProgressPercentage;

                        this.Dispatcher.Invoke(() =>
                        {
                            listDownloads.Items.Refresh();
                        });
                    }
                    catch (Exception) {}
                };

                df.Download();     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Вибрати папку для збереження завантажень
        private void BtnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pathFolderDownloads = folderBrowserDialog.SelectedPath;
                labelPathFolderDownloads.Content = pathFolderDownloads;
            }
        }

        // Поставити на паузу
        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if(listDownloads.SelectedIndex < 0)
            {
                MessageBox.Show("Not selected item");
                return;
            }

            DownloadFile tmpDf = (DownloadFile)listDownloads.Items[listDownloads.SelectedIndex];

            if(!tmpDf.Pause())
            {
                MessageBox.Show("You can`t set pause on this item");
                return;
            }

            listDownloads.Items.Refresh();
        }

        // Скасувати
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (listDownloads.SelectedIndex < 0)
            {
                MessageBox.Show("Not selected item");
                return;
            }

            DownloadFile tmpDf = (DownloadFile)listDownloads.Items[listDownloads.SelectedIndex];

            if (!tmpDf.Cancel())
            {
                MessageBox.Show("You can`t cancel download for this item");
                return;
            }

            listDownloads.Items.Refresh();

        }

        // Видалити файл
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listDownloads.SelectedIndex < 0)
            {
                MessageBox.Show("Not selected item");
                return;
            }

            DownloadFile tmpDf = (DownloadFile)listDownloads.Items[listDownloads.SelectedIndex];

            listDownloads.Items.RemoveAt(listDownloads.SelectedIndex);
            listDownloads.Items.Refresh();
        }

        // Переміщення файлу
        private void BtnReplace_Click(object sender, RoutedEventArgs e)
        {
            if (listDownloads.SelectedIndex < 0)
            {
                MessageBox.Show("Not selected item");
                return;
            }

            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    DownloadFile tmpDf = (DownloadFile)listDownloads.Items[listDownloads.SelectedIndex];
                    tmpDf.Replace(folderBrowserDialog.SelectedPath);    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        // Перейменувати файл
        private void BtnRename_Click(object sender, RoutedEventArgs e)
        {
            if (listDownloads.SelectedIndex < 0)
            {
                MessageBox.Show("Not selected item");
                return;
            }

            RenameFile rf = new RenameFile();
            rf.ShowDialog();

            try
            {
                DownloadFile tmpDf = (DownloadFile)listDownloads.Items[listDownloads.SelectedIndex];
                
                if(rf.newName != null && rf.newName != "")
                {
                    tmpDf.Rename(rf.newName);
                    listDownloads.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Оновити інтерфейс для елемента із списку
        private void UpdateButtonForListItem(DownloadFile df)
        {
            if(df.isCanceled)
            {
                btnCancel.IsEnabled = false;
                btnPause.IsEnabled = false;
                btnReplace.IsEnabled = false;
                btnDelete.IsEnabled = true;
                btnRename.IsEnabled = false;
            }
            else if (df.isPaused)
            {
                btnCancel.IsEnabled = true;
                btnPause.IsEnabled = true;
                btnReplace.IsEnabled = false;
                btnDelete.IsEnabled = true;
                btnRename.IsEnabled = false;
            }
            else if (df.percentDownload != 100)
            {
                btnCancel.IsEnabled = true;
                btnPause.IsEnabled = true;
                btnReplace.IsEnabled = false;
                btnDelete.IsEnabled = true;
                btnRename.IsEnabled = false;
            }
            else if (df.percentDownload == 100)
            {
                btnCancel.IsEnabled = false;
                btnPause.IsEnabled = false;
                btnReplace.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnRename.IsEnabled = true;
            }

            if (df.isPaused)
            {
                btnPause.Content = "Resume";
            }
            else
            {
                btnPause.Content = "Pause";
            }
        }

        // Встановити активними всі кнопки
        private void SetActiveAllButton()
        {
            btnCancel.IsEnabled = true;
            btnPause.IsEnabled = true;
            btnReplace.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnRename.IsEnabled = false;
        }

        // Встановити пасивними всі кнопки
        private void DisableAllButton()
        {
            btnCancel.IsEnabled = false;
            btnPause.IsEnabled = false;
            btnReplace.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnRename.IsEnabled = false;
        }

        // Вибрано елемент із списку
        private void ListDownloads_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DownloadFile tmpDf = (DownloadFile)listDownloads.Items[listDownloads.SelectedIndex];
                UpdateButtonForListItem(tmpDf);
            }
            catch (Exception)
            {
                DisableAllButton();
            }
        }
    }
}
