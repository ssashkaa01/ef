using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace WpfAppRegEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            RegistryKey[] regs = new[]
            {
                Registry.ClassesRoot,
                Registry.CurrentUser,
                Registry.LocalMachine,
                Registry.Users,
                Registry.CurrentConfig
            };

            foreach(RegistryKey reg in regs)
            {
                var item = new TreeViewItem()
                {
                    Header = reg,
                    //IsExpanded = false
                };

                LoadSubKeys(item);


                //item.ItemsSource = reg.GetSubKeyNames();
                TreeView.Items.Add(item);

               // LoadSubKeys(item);
            }


        }

        private async void Item_Expanded(object sender, RoutedEventArgs e)
        {
            //ParameterizedThreadStart ts1 = new ParameterizedThreadStart(UpdateExpandItems);
            //Thread t1 = new Thread(ts1);
            //t1.IsBackground = true;

            //t1.Start((object)(sender));


            //UpdateExpandItems(sender);

            await Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    TreeViewItem item = sender as TreeViewItem;

                    foreach (TreeViewItem i in item.Items)
                    {
                   
                        LoadSubKeys(i);
                    

                    }
                });
            });

        }

        private async Task UpdateExpandItems(object sender)
        {
            await Task.Run(() =>
            {
                TreeViewItem item = sender as TreeViewItem;

                foreach (TreeViewItem i in item.Items)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        LoadSubKeys(i);
                    });

                }
            });
            
            
        }

        private void LoadSubKeys(TreeViewItem item)
        {
            item.Items.Clear();

            RegistryKey key = (item.Header as RegistryKey);

            if (key.SubKeyCount <= 0) return;

            foreach(var name in key.GetSubKeyNames())
            {
                try
                {
                    var subitem = new TreeViewItem()
                    {
                        IsExpanded = false,
                        Header = key.OpenSubKey(name)
                    };
                   
                    item.Items.Add(subitem);
                   
                    item.Expanded += Item_Expanded;
                    item.Selected += Item_Selected;
                }
                catch (Exception e)
                {

                }
            }
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;

            RegistryKey key = (item.Header as RegistryKey);

            table.Items.Clear();

            foreach (string name in key.GetSubKeyNames())
            {
               
                //table.Items.Add();
            }

           
        }
    }
}
