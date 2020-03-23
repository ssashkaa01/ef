using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppExam
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string PathCopy { get; set; } 
        private string PathStopWords { get; set; }
        private string Status { get; set; }
        private Thread ThreadFinder { get; set; }
        private int CountCopy { get; set; }
       
        public MainWindow()
        {
            InitializeComponent();
           
        }

        public string GetTimestamp()
        {
            return (new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()).ToString();
        }

        // Почати пошук стоп слів
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CountCopy = Convert.ToInt32(textboxCountFiles.Text);
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (IsEmpty(PathCopy))
            {
                MessageBox.Show("Empty path for copy");
                return;
            }
          
            else if (IsEmpty(PathStopWords))
            {
                MessageBox.Show("Not selected path with file for copy");
                return;
            }

            else if (CountCopy < 1)
            {
                MessageBox.Show("Copy has been > 0");
                return;
            }

            if (Status != "pause")
            {
               
                ThreadStart ts = new ThreadStart(CopyFiles);
                ThreadFinder = new Thread(ts);
                ThreadFinder.IsBackground = true;
                ThreadFinder.Start();
            }

            SetStatus("active");
        }

        // Поставити на паузу
        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            SetStatus("pause");
        }

        // Зупинити пошук
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            ThreadFinder.Abort();
            progressBar.Value = 0;
            SetStatus("stop");

            MessageBox.Show("Canceled!");
        }

        // Потік виконання пошуку стоп слів
        private void CopyFiles()
        {
            this.Dispatcher.Invoke(() =>
            {
                progressBar.Maximum = CountCopy;
                progressBar.Value = 0;
            });

            for (int i = 0; i < CountCopy; i++)
            {
                while (Status == "pause")
                {
                    Thread.Sleep(1000);
                }

                // Оновляємо прогрес бар
                this.Dispatcher.Invoke(() =>
                {
                    progressBar.Value = i + 1;
                });

                FileInfo info = new FileInfo(PathStopWords);

                // Запис файлу
                File.Copy(PathStopWords, $"{PathCopy}\\{Path.GetFileNameWithoutExtension(info.Name)}_{i}_{GetTimestamp()}{info.Extension}");

                Thread.Sleep(500);
            }

            this.Dispatcher.Invoke(() =>
            {
                SetStatus("stop");
            });

            MessageBox.Show("Success!");
        }

        // Вибір папки для копіювання
        private void ButtonSelectFolderCopy_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathCopy = folderBrowserDialog.SelectedPath;
                labelFolderCopy.Content = PathCopy;
            }
        }

        // Вибір файлу із стоп словами
        private void ButtonSelectFileStopWords_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                PathStopWords = openFileDialog.FileName;
                labelFileStopWords.Content = PathStopWords;
            }
        }

        // Змінити статус
        private void SetStatus(string status = "active")
        {
            this.Status = status;

            if(status == "active")
            {
                buttonPause.IsEnabled = true;
                buttonStart.IsEnabled = false;
                buttonStop.IsEnabled = true;
                buttonSelectFileStopWords.IsEnabled = false;
                buttonSelectFolderCopy.IsEnabled = false;
                textboxCountFiles.IsEnabled = true;
            }
            else if(status == "stop")
            {
                buttonPause.IsEnabled = false;
                buttonStart.IsEnabled = true;
                buttonStop.IsEnabled = false;
                buttonSelectFileStopWords.IsEnabled = true;
                buttonSelectFolderCopy.IsEnabled = true;
                textboxCountFiles.IsEnabled = false;
            }
            else if (status == "pause")
            {
                buttonPause.IsEnabled = false;
                buttonStart.IsEnabled = true;
                buttonStop.IsEnabled = true;
                buttonSelectFileStopWords.IsEnabled = false;
                buttonSelectFolderCopy.IsEnabled = false;
                textboxCountFiles.IsEnabled = false;
            }
        }

        // Перевірка чи значення пусте
        private bool IsEmpty(object variable)
        {
            if (variable == null) return true;

            if (variable is string && (string)variable == "") return true;
           
            return false;
        }
    }
}
