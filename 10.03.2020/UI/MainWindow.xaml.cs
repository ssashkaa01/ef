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
           
            SelectedTable.Items.Add("Directors");
            SelectedTable.Items.Add("Workers");
            SelectedTable.Items.Add("Shops");
            SelectedTable.Items.Add("Products");

            SelectedTable.SelectedItem = "Directors";

            //DataTableCollection = logics.GetShops();
            //MessageBox.Show("qq");

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show((table.SelectedItem as DirectorDTO).Education);
            //logics.UpdateDirector(table.SelectedItem as DirectorDTO);
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

        // Змінити таблицю
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

        // Збереження даних
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(!logics.SaveAll())
            {
                MessageBox.Show("Bad data in table!");
            }
            else
            {
                MessageBox.Show("Data saved!");
            }
        }

        private void Table_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {

            

        }

        private void Table_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            
        }

        private void Table_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            
        }

        private void Table_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            
        }

        private void Table_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
         
           // logics.UpdateDirector(table.SelectedItem as DirectorDTO);
            // MessageBox.Show((table.SelectedItem as DirectorDTO).Education);
            // logics.UpdateDirector(table.SelectedItem as DirectorDTO);

        }

        private void Table_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void Table_CurrentCellChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(e.);
        }
    }
}
