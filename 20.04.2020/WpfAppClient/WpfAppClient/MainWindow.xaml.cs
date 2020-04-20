using System;
using System.Collections.Generic;
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
using WpfAppClient.ServiceConverter;

namespace WpfAppClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            comboBoxConvertTo.Items.Add("LinearMeasure");
            comboBoxConvertTo.Items.Add("CelsiusToFahrenheit");
            comboBoxConvertTo.Items.Add("FahrenheitToCelsius");

          

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConvertedUnits cu;

                using (ConverterClient sc = new ConverterClient())
                {
                    switch (comboBoxConvertTo.SelectedItem)
                    {
                        case "LinearMeasure":
                            cu = sc.LinearMeasure(Convert.ToDouble(textBoxValue.Text));
                            MessageBox.Show($"Foot: {cu.Foot}\nInch:{cu.Inch}\nYard: {cu.Yard}");
                            break;

                        case "CelsiusToFahrenheit":
                            cu = sc.CelsiusToFahrenheit(Convert.ToDouble(textBoxValue.Text));
                            MessageBox.Show($"Fahrenheit: {cu.Fahrenheit}");
                            break;

                        case "FahrenheitToCelsius":
                            cu = sc.FahrenheitToCelsius(Convert.ToDouble(textBoxValue.Text));
                            MessageBox.Show($"Celsius: {cu.Celsius}");
                            break;
                    }

                 
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
