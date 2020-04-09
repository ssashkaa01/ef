using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfAppDownloadFile
{
    public class DownloadFile
    {
        public WebClient webClient { get; set; }
        public int percentDownload { get; set; }
        public string downloadPath { get; set; }
        public bool isCanceled { get; set; }
        public bool isPaused { get; set; }

        private ThreadStart ts { get; set; }
        private Thread t { get; set; }
        private Uri uri { get; set; }

        public DownloadFile(string pathFolderDownloads, string urlFile)
        {
            uri = new Uri(urlFile);
            downloadPath = $"{pathFolderDownloads}\\{System.IO.Path.GetFileNameWithoutExtension(urlFile)}_{GetTimestamp(DateTime.Now)}{System.IO.Path.GetExtension(urlFile)}";
            webClient = new WebClient();
            percentDownload = 0;
            isCanceled = false;
        }

        public string GetFileName()
        {
            return System.IO.Path.GetFileName(downloadPath);
        }

        public override string ToString()
        {
            if (percentDownload == 100)
            {
                return $"{GetFileName()} | Downloaded!";
            }
            else if (isCanceled)
            {
                return $"{GetFileName()} | Canceled!";
            }
            else if (isPaused)
            {
                return $"{GetFileName()} | Paused!";
            }
            else
            {
                return $"{GetFileName()} | Download: {percentDownload}%";
            }
        }

        private void Downloader()
        {
            webClient.DownloadFileTaskAsync(uri, downloadPath);


        }

        // Завантажити
        public bool Download()
        {
            if (isCanceled || percentDownload == 100 || isPaused)
            {
                return false;
            }

            ts = new ThreadStart(Downloader);
            Thread t = new Thread(ts);
            t.IsBackground = true;
            t.Start();

            return true;
        }

        // Скасувати завантаження
        public bool Cancel()
        {
            if (!isCanceled && percentDownload != 100)
            {
                webClient.CancelAsync();
                isCanceled = true;
                DeleteFile();
                return true;
            }

            return false;
        }

        // Поставити на паузу
        public bool Pause()
        {
            if (!isCanceled && percentDownload != 100)
            {
                isPaused = true;
                return true;
            }

            return false;
        }

        // Перейменувати
        public bool Rename(string newName)
        {
            string newPath = System.IO.Path.GetDirectoryName(downloadPath) + "\\" + newName + System.IO.Path.GetExtension(downloadPath);

            if (!isCanceled && percentDownload == 100 && File.Exists(downloadPath) && !File.Exists(newPath))
            {
                System.IO.File.Move(downloadPath, newPath);

                downloadPath = newPath;
                return true;
            }

            return false;
        }

        // Перемістити
        public bool Replace(string selectedPath)
        {
            string newPath = selectedPath + "\\" + System.IO.Path.GetFileName(downloadPath);

            if (!isCanceled && percentDownload == 100 && File.Exists(downloadPath) && !File.Exists(newPath))
            {
                System.IO.File.Move(downloadPath, newPath);

                downloadPath = newPath;
                return true;
            }

            return false;
        }

        // Видалити завантажуваний файл
        public void DeleteFile()
        {
            webClient.CancelAsync();

            if (File.Exists(downloadPath))
            {
                bool tryDelete = true;

                while (tryDelete)
                {
                    try
                    {
                        File.Delete(downloadPath);
                        tryDelete = false;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        // Отримати часову мітку
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
