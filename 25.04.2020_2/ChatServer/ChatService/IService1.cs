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

        [OperationContract(IsOneWay = true)]
        void SendText(string userName, string msg);

        [OperationContract]
        void Logout(string name);
    }

    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void TextForUsers(string user, string msg);
    }
}
