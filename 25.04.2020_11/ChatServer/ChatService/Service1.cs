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

            try
            {
                string[] users = clients.Select(i => i.Key).ToArray();

                foreach (var item in clients)
                {
                    if (item.Key == name)
                        continue;

                    try
                    {
                        item.Value.UsersChange(users);
                    }
                    catch (Exception ex)
                    {
                        clients.Remove(item.Key);
                    }
                }
            }
            catch (Exception ex)
            {
               // clients.Remove(name);
            }

            return true;
        }

        // Відправка повідомлення
        public void SendMessasge(string userName, string msg)
        {
            foreach (var item in clients)
            {
                if (item.Key == userName)
                    continue;

                try
                {
                    item.Value.RecieveMessage(userName, msg);
                }
                catch (Exception ex) {
                    clients.Remove(userName);
                }
            }
        }

        public string[] GetUsers()
        {
            return clients.Select(i => i.Key).ToArray();
        }

        public void Logout(string name)
        {
            clients.Remove(name);

            string[] users = clients.Select(i => i.Key).ToArray();

            foreach (var item in clients)
            {
                if (item.Key == name)
                    continue;

                try
                {
                    item.Value.UsersChange(users);
                }
                catch (Exception ex)
                {
                    clients.Remove(item.Key);
                }
            }
        }

        public void SendPrivateMessasge(string NameFrom, string msg, string NameTo)
        {
            foreach (var item in clients)
            {
                if (item.Key == NameTo)
                {
                    try
                    {
                        item.Value.ReceivePrivateMessage(NameFrom, msg);
                    }
                    catch (Exception ex)
                    {
                        clients.Remove(NameTo);
                    }
                }
            }
        }
    }
}
