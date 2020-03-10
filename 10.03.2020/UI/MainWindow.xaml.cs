using BLL;
using System;
using System.Collections.Generic;
using System.Data;
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


namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Logics logics = new Logics();

        public MainWindow()
        {
            InitializeComponent();
           

            table.ItemsSource = logics.GetDirectors();

            SelectedTable.Items.Add("Directors");
            SelectedTable.Items.Add("Workers");
            SelectedTable.Items.Add("Shops");
            SelectedTable.Items.Add("Products");

            //DataTableCollection = logics.GetShops();
            //MessageBox.Show("qq");

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

         
            // Load data by setting the CollectionViewSource.Source property:
            // logicsViewSource.Source = [generic data source]
        }

        private void SelectedTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            ChangeTable(SelectedTable.SelectedValue.ToString());
        }

        private void ChangeTable(string tableName)
        {
            switch (tableName)
            {
                case "Directors":
                    table.ItemsSource = logics.GetDirectors();
                    break;

                case "Workers":
                    table.ItemsSource = logics.GetWorkers();
                    break;

                case "Shops":
                    table.ItemsSource = logics.GetShops();
                    break;

                case "Products":
                    table.ItemsSource = logics.GetProducts();
                    break;
            }
        }
    }
}
