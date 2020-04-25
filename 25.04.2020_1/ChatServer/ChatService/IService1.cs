using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
{
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IChat
    {
        [OperationContract]
        bool Login(string name);

        [OperationContract]
        string[] GetUsers();

        [OperationContract(IsOneWay = true)]
        void SendMessasge(string userName, string msg);

        [OperationContract(IsOneWay = true)]
        void SendPrivateMessasge(string NameFrom, string msg, string NameTo);

        [OperationContract]
        void Logout(string name);
    }

    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void RecieveMessage(string user, string msg);

        [OperationContract(IsOneWay = true)]
        void UsersChange(string[] users);

        [OperationContract(IsOneWay = true)]
        void ReceivePrivateMessage(string user, string msg);
    }
}
