﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfAppClient.ServiceConverter {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ConvertedUnits", Namespace="http://schemas.datacontract.org/2004/07/WCF_MyServer1")]
    [System.SerializableAttribute()]
    public partial class ConvertedUnits : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double CelsiusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double FahrenheitField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double FootField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double InchField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double MetrField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double YardField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Celsius {
            get {
                return this.CelsiusField;
            }
            set {
                if ((this.CelsiusField.Equals(value) != true)) {
                    this.CelsiusField = value;
                    this.RaisePropertyChanged("Celsius");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Fahrenheit {
            get {
                return this.FahrenheitField;
            }
            set {
                if ((this.FahrenheitField.Equals(value) != true)) {
                    this.FahrenheitField = value;
                    this.RaisePropertyChanged("Fahrenheit");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Foot {
            get {
                return this.FootField;
            }
            set {
                if ((this.FootField.Equals(value) != true)) {
                    this.FootField = value;
                    this.RaisePropertyChanged("Foot");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Inch {
            get {
                return this.InchField;
            }
            set {
                if ((this.InchField.Equals(value) != true)) {
                    this.InchField = value;
                    this.RaisePropertyChanged("Inch");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Metr {
            get {
                return this.MetrField;
            }
            set {
                if ((this.MetrField.Equals(value) != true)) {
                    this.MetrField = value;
                    this.RaisePropertyChanged("Metr");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Yard {
            get {
                return this.YardField;
            }
            set {
                if ((this.YardField.Equals(value) != true)) {
                    this.YardField = value;
                    this.RaisePropertyChanged("Yard");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceConverter.IConverter")]
    public interface IConverter {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConverter/LinearMeasure", ReplyAction="http://tempuri.org/IConverter/LinearMeasureResponse")]
        WpfAppClient.ServiceConverter.ConvertedUnits LinearMeasure(double meters);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConverter/LinearMeasure", ReplyAction="http://tempuri.org/IConverter/LinearMeasureResponse")]
        System.Threading.Tasks.Task<WpfAppClient.ServiceConverter.ConvertedUnits> LinearMeasureAsync(double meters);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConverter/CelsiusToFahrenheit", ReplyAction="http://tempuri.org/IConverter/CelsiusToFahrenheitResponse")]
        WpfAppClient.ServiceConverter.ConvertedUnits CelsiusToFahrenheit(double c);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConverter/CelsiusToFahrenheit", ReplyAction="http://tempuri.org/IConverter/CelsiusToFahrenheitResponse")]
        System.Threading.Tasks.Task<WpfAppClient.ServiceConverter.ConvertedUnits> CelsiusToFahrenheitAsync(double c);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConverter/FahrenheitToCelsius", ReplyAction="http://tempuri.org/IConverter/FahrenheitToCelsiusResponse")]
        WpfAppClient.ServiceConverter.ConvertedUnits FahrenheitToCelsius(double f);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IConverter/FahrenheitToCelsius", ReplyAction="http://tempuri.org/IConverter/FahrenheitToCelsiusResponse")]
        System.Threading.Tasks.Task<WpfAppClient.ServiceConverter.ConvertedUnits> FahrenheitToCelsiusAsync(double f);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IConverterChannel : WpfAppClient.ServiceConverter.IConverter, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ConverterClient : System.ServiceModel.ClientBase<WpfAppClient.ServiceConverter.IConverter>, WpfAppClient.ServiceConverter.IConverter {
        
        public ConverterClient() {
        }
        
        public ConverterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ConverterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConverterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConverterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public WpfAppClient.ServiceConverter.ConvertedUnits LinearMeasure(double meters) {
            return base.Channel.LinearMeasure(meters);
        }
        
        public System.Threading.Tasks.Task<WpfAppClient.ServiceConverter.ConvertedUnits> LinearMeasureAsync(double meters) {
            return base.Channel.LinearMeasureAsync(meters);
        }
        
        public WpfAppClient.ServiceConverter.ConvertedUnits CelsiusToFahrenheit(double c) {
            return base.Channel.CelsiusToFahrenheit(c);
        }
        
        public System.Threading.Tasks.Task<WpfAppClient.ServiceConverter.ConvertedUnits> CelsiusToFahrenheitAsync(double c) {
            return base.Channel.CelsiusToFahrenheitAsync(c);
        }
        
        public WpfAppClient.ServiceConverter.ConvertedUnits FahrenheitToCelsius(double f) {
            return base.Channel.FahrenheitToCelsius(f);
        }
        
        public System.Threading.Tasks.Task<WpfAppClient.ServiceConverter.ConvertedUnits> FahrenheitToCelsiusAsync(double f) {
            return base.Channel.FahrenheitToCelsiusAsync(f);
        }
    }
}