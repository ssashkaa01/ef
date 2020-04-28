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

        [OperationContract]
        bool GoTo(string name, int action);

        [OperationContract]
        bool? SetMeInCommand(string name);

        [OperationContract]
        bool? CheckWin(string name);

        [OperationContract(IsOneWay = true)]
        void SetWaitStatus(string name, bool status = true);

        [OperationContract(IsOneWay = true)]
        void Logout(string name);

        [OperationContract]
        string[] GetPlayers();
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
        void OnBadAction();

        [OperationContract(IsOneWay = true)]
        void OnEnemyGoTo(int action);

        [OperationContract]
        void CheckOnline();

        [OperationContract(IsOneWay = true)]
        void OnSendMessage(string message);

        [OperationContract(IsOneWay = true)]
        void OnPlayersChange(string[] players);
    }
}
