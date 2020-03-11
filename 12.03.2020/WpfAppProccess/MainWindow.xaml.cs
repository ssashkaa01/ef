using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppProccess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Timer aTimer;
        public class ProcessItem
        {
            private int ram;

            public string NAME { get; set; }
            public int PID { get; set; }
            public string RAM {
                get {
                    return $"{ram} MB";
                }
                set {
                    ram =  Convert.ToInt32(value) / (int)(1024*1024);
                    
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            UpdateTable();
           
            // Create a timer and set a two second interval.
            aTimer = new Timer();
            aTimer.Interval = 2000;
         
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;
            aTimer.Start();

            comboTimeout.Items.Add(1000);
            comboTimeout.Items.Add(2000);
            comboTimeout.Items.Add(5000);
            comboTimeout.SelectedValue = 2000;
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            UpdateTable();
        }

        private void UpdateTable()
        {
           
            List<ProcessItem> list = new List<ProcessItem>();

            foreach (Process p in Process.GetProcesses())
            {
                list.Add(new ProcessItem()
                {
                    NAME = p.ProcessName,
                    PID = p.Id,
                    RAM = Convert.ToString(p.PrivateMemorySize64)
                });
            }

            Dispatcher.Invoke(() => dataGrid.ItemsSource = null);
            Dispatcher.Invoke(() => dataGrid.ItemsSource = list);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.GetProcessById((dataGrid.SelectedItem as ProcessItem).PID).Kill();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process p = Process.GetProcessById((dataGrid.SelectedItem as ProcessItem).PID);

            MessageBox.Show($"PID: {p.Id}\nMachine Name: {p.MachineName}\nProcess Name: {p.ProcessName}\nRAM: {p.PrivateMemorySize64}");
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(path.Text);
            } catch (Exception error)
            {
                MessageBox.Show("Bad path to proccess");
            }
           
        }

        private void ComboTimeout_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void ComboTimeout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            try
            {
                aTimer.Interval = Convert.ToInt32(comboTimeout.SelectedValue);
            }
            catch (Exception error)
            {
                ///MessageBox.Show("Bad timeout");
            }
        }
    }
}
