using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace WpfAppCrypt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string FilePath { get; set; }
        private bool IsCancel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        //генератор повторюваної послідовності
        private string GetRepeatKey(string s, int n)
        {
            var r = s;
            while (r.Length < n)
            {
                r += r;
            }

            return r.Substring(0, n);
        }

        //метод шифрування/дешифрування
        private string Cipher(string text, string secretKey)
        {
            var currentKey = GetRepeatKey(secretKey, text.Length);
            var res = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                progressBar.Maximum = text.Length;
            });

            for (var i = 0; i < text.Length; i++)
            {
                if(IsCancel)
                {
                    break;
                }

                this.Dispatcher.Invoke(() =>
                {
                    progressBar.Value = i;
                });

                res += ((char)(text[i] ^ currentKey[i])).ToString();
                System.Threading.Thread.Sleep(1000);
            }

            this.Dispatcher.Invoke(() =>
            {
                progressBar.Value = 0;
            });

            return res;
        }

        

        private async void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            string key = textBlockKey.Text;

            if (key == "")
            {
                MessageBox.Show("Empty key");
                return;
            }

            if (FilePath == "" || FilePath == null)
            {
                MessageBox.Show("File not selected");
                return;
            }

            string content = File.ReadAllText(FilePath);
            string path = System.IO.Path.GetDirectoryName(FilePath);
            string ext = System.IO.Path.GetExtension(FilePath);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(FilePath);
            IsCancel = false;

            if (radioButtonEncrypt.IsChecked == true)
            {
                Task<string> res = Crypt(content, key);
                dynamic str = await res;

                if (!IsCancel)
                {
                    File.WriteAllText(path + "/" + fileName + "_crypt" + ext, str);
                    MessageBox.Show("Success!");
                }
                else
                {
                    MessageBox.Show("Cancled!");
                }
            }
            else if (radioButtonDecrypt.IsChecked == true)
            {
                Task<string> res = Decrypt(content, key);
                dynamic str = await res;

                if (!IsCancel)
                {
                    File.WriteAllText(path + "/" + fileName + "_decrypt" + ext, str);
                    MessageBox.Show("Success!");
                }
                else
                {
                    MessageBox.Show("Cancled!");
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            IsCancel = true;
        }

        private async Task<string> Crypt(string plainText, string password)
        {
            return await Task.Run(() =>
            {
                return Cipher(plainText, password);
            });
        }

        private async Task<string> Decrypt(string plainText, string password)
        {
            return await Task.Run(() =>
            {
                return Cipher(plainText, password);
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                labelSelectFile.Content = FilePath;
            }
        }
    }
}
