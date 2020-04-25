using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Chat : IChat
    {
        private Dictionary<string, ICallback> clients = new Dictionary<string, ICallback>();

        public bool Login(string name)
        {
            if (clients.ContainsKey(name))
                return false;

            clients.Add(name, OperationContext.Current.GetCallbackChannel<ICallback>());

            return true;
        }

        // Відправка повідомлення
        public void SendText(string userName, string msg)
        {
            foreach (var item in clients)
            {
                if (item.Key == userName)
                    continue;

                try
                {
                    item.Value.TextForUsers(userName, msg);
                }
                catch (Exception ex) {
                    clients.Remove(userName);
                }
            }
        }

        public void Logout(string name)
        {
            clients.Remove(name);
        }
    }
}
