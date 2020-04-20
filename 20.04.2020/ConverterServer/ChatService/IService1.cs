using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
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
}
