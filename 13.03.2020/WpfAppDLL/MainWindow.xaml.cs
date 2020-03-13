using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace WpfAppDLL
{
    public class ComboBoxClasses
    {
        public string _Key { get; set; }
        public string _Value { get; set; }

        public Type  _Original { get; set; }

        public ComboBoxClasses(string _key, string _value, Type _original)
        {
            _Key = _key;
            _Value = _value;
            _Original = _original;
        }
    }

    public class ComboBoxMethods
    {
        public string _Key { get; set; }
        public string _Value { get; set; }
        public MemberInfo _Original { get; set; }

        public ComboBoxMethods(string _key, string _value, MemberInfo _original)
        {
            _Key = _key;
            _Value = _value;
             _Original = _original;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string FilePath { get; set; }
        private Assembly DLL;
        List<ComboBoxClasses> Classes = new List<ComboBoxClasses>();
        List<ComboBoxMethods> Methods = new List<ComboBoxMethods>();

        public MainWindow()
        {
            InitializeComponent();

            ComboBoxClasses.DisplayMemberPath = "_Value";
            ComboBoxClasses.SelectedValuePath = "_Key";

            ComboBoxMethods.DisplayMemberPath = "_Value";
            ComboBoxMethods.SelectedValuePath = "_Key";

        }

        // Відкрити файл
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                TextPathFile.Text = FilePath;

                UpdateComboBoxClasses();
            }
        }

        // Оновити комбобокс класів
        private void UpdateComboBoxClasses()
        {

            DLL = Assembly.LoadFile(FilePath);
            string className = "";

            Classes.Clear();

            foreach (Type type in DLL.GetExportedTypes())
            {
                if (type.IsAbstract)
                {
                    className = "Abstract Class : " + type.Name;
                }
                else if (type.IsPublic)
                {
                    className = "Public Class : " + type.Name;
                }
                else if (type.IsSealed)
                {
                    className = "Sealed Class : " + type.Name;
                }

                Classes.Add(new ComboBoxClasses(type.FullName, className, type));
            }

            ComboBoxClasses.ItemsSource = Classes;

        }


        // При виборі класу
        private void ComboBoxClasses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string methodName = "";
           
            Type t = Classes[ComboBoxClasses.SelectedIndex]._Original;

            foreach (MemberInfo method in t.GetMethods())
            {
                if (method.ReflectedType.IsPublic)
                {
                    methodName = "Public Method : " + method.Name.ToString();
                }
                else
                {
                    methodName = "Non-Public Method : " + method.Name.ToString();
                }

                Methods.Add(new ComboBoxMethods(method.Name, methodName, method));
            }

            ComboBoxMethods.ItemsSource = Methods;
        }

        private void ComboBoxMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
