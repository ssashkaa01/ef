using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace _22._03._2020
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string PathSearch { get; set; }
        private string Status { get; set; }
      
        public MainWindow()
        {
            InitializeComponent();
            SetStatus("stop");
        }

        private async Task LoadFilesAsync()
        {
            await Task.Run(() =>
            {

                IEnumerable<FileInfo> results = Directory.GetFiles(PathSearch).Select(file => new FileInfo(file));

                foreach (var item in results)
                {

                    if (Status == "stop") { 
                        MessageBox.Show("Cancled!");
                        break;
                    }

                    Thread.Sleep(1000);

                    this.Dispatcher.Invoke(() =>
                    {
                        ListFiles.Items.Add($"{item.Name} | Size: {item.Length} | Ext: {item.Extension} | Changed: {item.LastWriteTimeUtc}");
                    });
                }
            });
        }
        // Змінити статус
        private void SetStatus(string status = "active")
        {
            this.Status = status;

            if (status == "active")
            {
                Load.IsEnabled = false;
                Cancel.IsEnabled = true;
                Select.IsEnabled = false;
            }
            else if (status == "stop")
            {
                Load.IsEnabled = true;
                Cancel.IsEnabled = false;
                Select.IsEnabled = true;
            }
        }


        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            if (IsEmpty(PathSearch))
            {
                MessageBox.Show("Empty path for search");
                return;
            }

            SetStatus("active");

            ListFiles.Items.Clear();

            Task res = LoadFilesAsync();
            await res;

            if(Status == "active")
            {
                MessageBox.Show("Success!");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

            SetStatus("stop");
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathSearch = folderBrowserDialog.SelectedPath;
                labelPath.Content = PathSearch;
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
                i += directoryInfo.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly).Length;
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
