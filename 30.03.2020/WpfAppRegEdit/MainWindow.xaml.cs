using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

    public class Reg
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }

 
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            ReloadRegistry();
            Instance = this;






            // ПОДІЯ НА КЛАВІШУ 1







            hook = SetHook(proc);
            //Application.Run();
            //UnhookWindowsHookEx(hook);
           
        }

        private void ReloadRegistry()
        {
            TreeView.Items.Clear();

            RegistryKey[] regs = new[]
            {
                Registry.ClassesRoot,
                Registry.CurrentUser,
                Registry.LocalMachine,
                Registry.Users,
                Registry.CurrentConfig
            };

            foreach (RegistryKey reg in regs)
            {
                var item = new TreeViewItem()
                {
                    Header = reg,
                    IsExpanded = false
                };

                LoadSubKeys(item);

                TreeView.Items.Add(item);

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

            foreach (var name in key.GetSubKeyNames())
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
            // ЦЕЙ БЛОК ВИКЛИКАЄТЬСЯ БАГАТО РАЗ... ЯКЩО НЕ ВИВЕСТИ СПОВІЩЕННЯ, ТО ТАБЛИЦЯ НЕ ОНОВЛЮЄТЬСЯ!!!! 
            // ЙМОВІРНО ЦЕ ЧЕРЕЗ ВЕЛИКУ КЫЛЬКІСТЬ ЕЛЕМЕНТІВ В ТАБЛИЦІ
            // ЧОМУ ТАК ВІДБУВАЄТЬСЯ НА ЖАЛЬ НЕ ЗНАЙШОВ

            TreeViewItem item = sender as TreeViewItem;

            RegistryKey key = (item.Header as RegistryKey);

            //MessageBox.Show(item.Header?.ToString());

            table.ItemsSource = null;

            List<Reg> regs = new List<Reg>();

            foreach (string name in key.GetValueNames())
            {
                regs.Add(new Reg()
                {
                    Data = key.GetValue(name)?.ToString(),
                    Name = name,
                    Type = key.GetValueKind(name).ToString(),
                });

            }

            table.ItemsSource = regs;
            table.Items.Refresh();

            MessageBox.Show("1");

        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static HookProc proc = HookCallback;
        private static IntPtr hook = IntPtr.Zero;

        private static IntPtr SetHook(HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if ((nCode >= 0) && (wParam == (IntPtr)WM_KEYDOWN))
            {
                // ПОДІЯ НА КЛАВІШУ 1
                int vkCode = Marshal.ReadInt32(lParam);

                // ПОДІЯ НА КЛАВІШУ 1
                if (vkCode == 49)
                {
                    Instance.ReloadRegistry();
                    MessageBox.Show("Registry reload!");
                }
                return (IntPtr)1;
            }
            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }

}
