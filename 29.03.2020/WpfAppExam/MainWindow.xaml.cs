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
    public class BadWord
    {
        public string Word { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return $"{Word} [{Count}]";
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string PathCopy { get; set; } 
        private string PathSearch { get; set; } 
        private string PathStopWords { get; set; }
        private string[] StopWords { get; set; }
        private string Status { get; set; }
        private Thread ThreadFinder { get; set; }
        private List<string> BadWords { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            BadWords = new List<string>();

            PathSearch = "C:\\Users\\SSASHKOO\\source\\repos\\WpfAppExam\\StopWordsFolder";
            PathCopy = "C:\\Users\\SSASHKOO\\source\\repos\\WpfAppExam\\CopyFolder";
            PathStopWords = "C:\\Users\\SSASHKOO\\source\\repos\\WpfAppExam\\stop.txt";
            LoadStopWords();

            listBoxResult.ItemsSource = BadWords;
        }

        // Завантажити стоп слова
        private void LoadStopWords()
        {
            StopWords = File.ReadAllLines(PathStopWords);
        }

        public string GetTimestamp()
        {
            return (new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()).ToString();
        }

        // Почати пошук стоп слів
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (IsEmpty(PathCopy))
            {
                MessageBox.Show("Empty path for copy");
            }
            else if (IsEmpty(PathSearch))
            {
                MessageBox.Show("Empty path for search");
            }
            else if (IsEmpty(PathStopWords))
            {
                MessageBox.Show("Not selected path with stop words");
            }

            if (Status != "pause")
            {
                listBoxResult.ItemsSource = null;
                listBoxResult.Items.Refresh();
                BadWords.Clear();

                ThreadStart ts = new ThreadStart(FindStopWords);
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
        private void FindStopWords()
        {
            string PathResult = $"{PathCopy}\\Result_{GetTimestamp()}.txt";
            int countFiles = GetCountFiles();
            int countBadWord = 0;
            int i = 0;

            File.AppendAllText(PathResult, "Files:\n");

            this.Dispatcher.Invoke(() =>
            {
                progressBar.Maximum = GetCountFiles();
                progressBar.Value = i;
            });

            string[] allfiles = Directory.GetFiles(PathSearch, "*.txt", SearchOption.AllDirectories);

            foreach (var file in allfiles)
            {
                while (Status == "pause")
                {
                    Thread.Sleep(1000);
                }

                countFiles--;
                i++;
                countBadWord = 0;

                // Оновляємо прогрес бар
                this.Dispatcher.Invoke(() =>
                {
                    progressBar.Value = i;
                });

                if(HasStopWords(file))
                {
                    FileInfo info = new FileInfo(file);
                    string contents = File.ReadAllText(file);

                    foreach (string word in StopWords)
                    {
                        if (contents.Contains(word))
                        {

                            int count = Regex.Matches(contents, word).Count;

                            for (int j = 0; j < count; j++)
                            {
                                BadWords.Add(word);
                            }

                            countBadWord += count;
                            string pattern = String.Format(@"\b{0}\b", word);
                            contents = Regex.Replace(contents, pattern, "*******");
                           

                        }
                    }

                    File.AppendAllText(PathResult, $"Path: {file} | Count bad words: {countBadWord} | Size: {info.Length}\n");

                    // Запис файлу
                   
                    File.WriteAllText($"{PathCopy}\\{info.Name}_{GetTimestamp()}.{info.Extension}", contents);



                }

                Thread.Sleep(500);
            }

            IEnumerable<BadWord> listBad = BadWords.GroupBy(w => w)
                        .Select(group => new BadWord
                        {
                            Word = group.Key,
                            Count = group.Count()
                        })
                        .OrderByDescending(x => x.Count).Take(10);

            // 
            File.AppendAllText(PathResult, $"TOP 10 BAD WORDS:\n");

            foreach(BadWord w in listBad)
            {
                File.AppendAllText(PathResult, $"{w.Word} | Count: {w.Count}\n");
            }


            this.Dispatcher.Invoke(() =>
            {
                listBoxResult.ItemsSource = listBad;

                listBoxResult.Items.Refresh();


                SetStatus("stop");
            });

            MessageBox.Show("Success!");
        }

        // Перевірити чи є стоп слова
        private bool HasStopWords(string path)
        {
            string contents = File.ReadAllText(path);

            foreach(string word in StopWords)
            {
                if(contents.Contains(word))
                {
                    return true;
                }
            }

            return false;
        }

     
        // Вибір папки, в якій потрібно шукати
        private void ButtonSelectFolderSearch_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathSearch = folderBrowserDialog.SelectedPath;
                labelFolderCopy.Content = PathSearch;
            }
        }

        // Вибір папки для копіювання із стоп словами
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
                LoadStopWords();
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
                buttonSelectFolderSearch.IsEnabled = false;
            }
            else if(status == "stop")
            {
                buttonPause.IsEnabled = false;
                buttonStart.IsEnabled = true;
                buttonStop.IsEnabled = false;
                buttonSelectFileStopWords.IsEnabled = true;
                buttonSelectFolderCopy.IsEnabled = true;
                buttonSelectFolderSearch.IsEnabled = true;
            }
            else if (status == "pause")
            {
                buttonPause.IsEnabled = false;
                buttonStart.IsEnabled = true;
                buttonStop.IsEnabled = true;
                buttonSelectFileStopWords.IsEnabled = false;
                buttonSelectFolderCopy.IsEnabled = false;
                buttonSelectFolderSearch.IsEnabled = false;
            }
        }

        // Порахувати кількість файлів
        private int GetCountFiles()
        {
            int i = 0;

            DirectoryInfo directoryInfo = new DirectoryInfo(PathSearch);

            if (directoryInfo.Exists)
            {
                // Шукаємо у вкладених папках 
                i += directoryInfo.GetFiles("*.*", System.IO.SearchOption.AllDirectories).Length;
            }

            return i;
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
