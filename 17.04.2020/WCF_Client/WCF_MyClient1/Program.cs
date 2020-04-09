using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WCF_MyClient1
{
    [ServiceContract]
    public interface IDiskInfo
    {
        [OperationContract]
        string[] GetDiskInfo(string path);
    }

    [ServiceContract]
    public interface ISpaceInfo
    {
        [OperationContract]
        string TotalSpace(string diskName);

        [OperationContract]
        string FreeSpace(string diskName);
    }

    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IDiskInfo> factory = new ChannelFactory<IDiskInfo>(
                new WSHttpBinding(),
                new EndpointAddress("http://localhost:1011/DiskInfo"));

            ChannelFactory<ISpaceInfo> factory2 = new ChannelFactory<ISpaceInfo>(
                new WSHttpBinding(),
                new EndpointAddress("http://localhost:1011/SpaceInfo"));

            IDiskInfo channel = factory.CreateChannel();
            ISpaceInfo channel2 = factory2.CreateChannel();

            Console.WriteLine($"{channel2.FreeSpace("disk")}");
            Console.WriteLine($"{channel2.TotalSpace("disk")}");
            Console.WriteLine($"{channel2.TotalSpace("uoriir")}");

            string[] result = channel.GetDiskInfo(@"C:\Users\SSASHKOO\source\repos");

            if(result != null)
            {
                foreach(string str in result)
                {
                    Console.WriteLine(str);
                }
            }

            //Console.WriteLine("result: {0}", result);
            Console.WriteLine("Press <ENTER> to exit...\n");
            Console.ReadLine();

            factory.Close();
        }
    }
}
