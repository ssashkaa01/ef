using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel; // add reference System.ServiceModel
using System.ServiceModel.Description;
using System.Runtime.Serialization;

namespace WCF_MyServer1
{
    [DataContract]
    public class ConvertedUnits
    {
        [DataMember]
        public double Metr { get; set; } // метр

        [DataMember]
        public double Inch { get; set; } // дюйм

        [DataMember]
        public double Foot { get; set; } // фути

        [DataMember]
        public double Yard { get; set; } // ярд

        [DataMember]
        public double Celsius { get; set; }

        [DataMember]
        public double Fahrenheit { get; set; } 
    }

    [ServiceContract]
    public interface IConverter
    {
        [OperationContract]
        ConvertedUnits LinearMeasure(double meters);

        [OperationContract]
        ConvertedUnits CelsiusToFahrenheit(double c);

        [OperationContract]
        ConvertedUnits FahrenheitToCelsius(double f);
    }
   
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

    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost sh = new ServiceHost(typeof(Converter));

            //// стврюємо новий об'єкт поведінки
            //ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            //// дозволяємо доступ до метаданих по протоколу Http
            //behavior.HttpGetEnabled = true;

            //// додаємо нову поведінку в Description служби
            //sh.Description.Behaviors.Add(behavior);

            //sh.AddServiceEndpoint(
            //  typeof(IMyMath),              // [C]ontract
            //  new WSHttpBinding(),          // [B]inding
            //  "http://localhost/MyMath");   // [A]ddress    //[protocol]://[ipAddress, domainName]/[URI]

            // додаємо кінцеву mex-точку до служби
            //sh.AddServiceEndpoint(
            //    typeof(IMetadataExchange),
            //    MetadataExchangeBindings.CreateMexHttpBinding(),
            //    "mex");

            sh.Open();
            Console.WriteLine("Press <ENTER> to exit...\n");
            Console.ReadLine();
            sh.Close(); // timeout (10)
        }
    }
}
