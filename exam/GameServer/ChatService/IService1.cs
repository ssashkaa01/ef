using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
{
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IGame
    {
        [OperationContract]
        bool Login(string name);

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
        void OnStartGame(string namePlayer);

        [OperationContract(IsOneWay = true)]
        void OnEndGame(string namePlayer);

        [OperationContract(IsOneWay = true)]
        void OnPlayerCanGo();

        [OperationContract(IsOneWay = true)]
        void OnPlayerExit();

        [OperationContract(IsOneWay = true)]
        void OnPlayersChange(string[] players);
    }
}
