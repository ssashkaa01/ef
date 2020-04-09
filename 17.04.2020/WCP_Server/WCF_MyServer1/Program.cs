using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel; // add reference System.ServiceModel
using System.IO;

namespace WCF_MyServer1
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

    public class SpaceInfo : ISpaceInfo
    {
        public string TotalSpace(string diskName)
        {
            string size = "";

            try
            {
                DriveInfo d = new DriveInfo(Convert.ToString(diskName[0]));

                size = d.TotalSize.ToString();
            }
            catch (Exception e)
            {
                return "Wrong disk!";
            }

            return size;
        }


        public string FreeSpace(string diskName)
        {
            string size = "";

            try
            {
                DriveInfo d = new DriveInfo(Convert.ToString(diskName[0]));

                size = d.TotalFreeSpace.ToString();
            }
            catch (Exception e)
            {
                return "Wrong disk!";
            }

            return size;
        }
    }

    public class DiskInfo : IDiskInfo, ISpaceInfo
    {
     
        public string[] GetDiskInfo(string path)
        {
            List<string> list = new List<string>();

            try
            {
                var dir = new DirectoryInfo(path);

                foreach (FileInfo file in dir.GetFiles())
                {
                    list.Add($"{Path.GetFileNameWithoutExtension(file.FullName)} - file");
                }

                foreach (DirectoryInfo directory in dir.GetDirectories())
                {
                    list.Add($"{directory.Name} - directory");
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return list.ToArray();
        }

        public string TotalSpace(string diskName)
        {
            string size = "";

            try
            {
                DriveInfo d = new DriveInfo(Convert.ToString(diskName[0]));

                size = d.TotalSize.ToString();
            }
            catch (Exception e)
            {
                return "Wrong disk!";
            }

            return size;
        }


        public string FreeSpace(string diskName)
        {
            string size = "";

            try
            {
                DriveInfo d = new DriveInfo(Convert.ToString(diskName[0]));

                size = d.TotalFreeSpace.ToString();
            }
            catch (Exception e)
            {
                return "Wrong disk!";
            }

            return size;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost sh = new ServiceHost(typeof(DiskInfo));
            sh.Open();

            ServiceHost sh2 = new ServiceHost(typeof(SpaceInfo));
            sh2.Open();


            Console.WriteLine("Press <ENTER> to exit...\n");
            Console.ReadLine();
            sh.Close(); // timeout (10)
        }
    }
}
