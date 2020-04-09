using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppDownloadFile
{
    /// <summary>
    /// Interaction logic for RenameFile.xaml
    /// </summary>
    public partial class RenameFile : Window
    {
        public string newName { get; set; }

        public RenameFile()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            Regex rgx = new Regex(@"^[a-zA-Z0-9\s\-]{1,30}$");

            if(!rgx.IsMatch(textBoxNewName.Text))
            {
                MessageBox.Show("Only a-zA-Z0-9, space, from 1 to 30 symbols!");
                return;
            }

            newName = textBoxNewName.Text;

            this.Close();
        }
    }
}
