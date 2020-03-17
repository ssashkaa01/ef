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
using TanksLib;

namespace WpfAppTanksDLL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread thread;

        public MainWindow()
        {
            InitializeComponent();

            Tanks1.Items.Add(new Tank("T-34"));
            Tanks1.Items.Add(new Tank("T-34"));
            Tanks1.Items.Add(new Tank("T-34"));
            Tanks1.Items.Add(new Tank("T-34"));
            Tanks1.Items.Add(new Tank("T-34"));

            Tanks2.Items.Add(new Tank("Panterra"));
            Tanks2.Items.Add(new Tank("Panterra"));
            Tanks2.Items.Add(new Tank("Panterra"));
            Tanks2.Items.Add(new Tank("Panterra"));
            Tanks2.Items.Add(new Tank("Panterra"));

            thread = new Thread(War);
            thread.Start();
        }

        public void War()
        {
            while(true)
            {
                Thread.Sleep(1000);

                if(Tanks1.Items.Count == 0)
                {
                    MessageBox.Show("Tanks 2 WIN!!!");
                    thread.Abort();
                    break;
                }
                else if(Tanks2.Items.Count == 0)
                {
                    MessageBox.Show("Tanks 1 WIN!!!");
                    thread.Abort();
                    break;
                }

                Tank t1 = (Tank)Tanks1.Items[0];
                Tank t2 = (Tank)Tanks2.Items[0];

                this.Dispatcher.Invoke(() =>
                {
                    if (t1 ^ t2)
                    {
                        Tanks2.Items.RemoveAt(0);
                    }
                    else
                    {
                        Tanks1.Items.RemoveAt(0);
                    }
                });
            }
        }
    }
}
