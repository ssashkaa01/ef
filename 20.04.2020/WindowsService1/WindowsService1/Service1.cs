using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ChatService;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        // Add ServiceModel Reference
        static ServiceHost host = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (host != null)
            {
                host.Close();
            }

            host = new ServiceHost(typeof(ChatService.IConverter));
            host.Open();
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                host.Close();
                host = null;
            }
        }
    }
}
