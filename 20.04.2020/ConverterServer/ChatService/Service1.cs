using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
{
    public class Converter : IConverter
    {
        public ConvertedUnits LinearMeasure(double m)
        {
            ConvertedUnits cu = new ConvertedUnits();

            cu.Metr = m;
            cu.Inch = m * 39.3700787;
            cu.Foot = m * 3.2808399;
            cu.Yard = m * 1.0936133;

            return cu;
        }

        public ConvertedUnits CelsiusToFahrenheit(double c)
        {
            ConvertedUnits cu = new ConvertedUnits();

            cu.Celsius = c;
            cu.Fahrenheit = c * 9 / 5 + 32;

            return cu;
        }

        public ConvertedUnits FahrenheitToCelsius(double f)
        {
            ConvertedUnits cu = new ConvertedUnits();

            cu.Fahrenheit = f;
            cu.Celsius = (f - 32.0) / 1.8;

            return cu;
        }
    }
}
