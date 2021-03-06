﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChatWindow.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IGame", CallbackContract=typeof(ChatWindow.ServiceReference1.IGameCallback))]
    public interface IGame {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/Login", ReplyAction="http://tempuri.org/IGame/LoginResponse")]
        bool Login(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/Login", ReplyAction="http://tempuri.org/IGame/LoginResponse")]
        System.Threading.Tasks.Task<bool> LoginAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/GoTo", ReplyAction="http://tempuri.org/IGame/GoToResponse")]
        bool GoTo(string name, int action);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/GoTo", ReplyAction="http://tempuri.org/IGame/GoToResponse")]
        System.Threading.Tasks.Task<bool> GoToAsync(string name, int action);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/SetMeInCommand", ReplyAction="http://tempuri.org/IGame/SetMeInCommandResponse")]
        System.Nullable<bool> SetMeInCommand(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/SetMeInCommand", ReplyAction="http://tempuri.org/IGame/SetMeInCommandResponse")]
        System.Threading.Tasks.Task<System.Nullable<bool>> SetMeInCommandAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/CheckWin", ReplyAction="http://tempuri.org/IGame/CheckWinResponse")]
        System.Nullable<bool> CheckWin(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/CheckWin", ReplyAction="http://tempuri.org/IGame/CheckWinResponse")]
        System.Threading.Tasks.Task<System.Nullable<bool>> CheckWinAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/SetWaitStatus")]
        void SetWaitStatus(string name, bool status);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/SetWaitStatus")]
        System.Threading.Tasks.Task SetWaitStatusAsync(string name, bool status);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/Logout")]
        void Logout(string name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/Logout")]
        System.Threading.Tasks.Task LogoutAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/GetPlayers", ReplyAction="http://tempuri.org/IGame/GetPlayersResponse")]
        string[] GetPlayers();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/GetPlayers", ReplyAction="http://tempuri.org/IGame/GetPlayersResponse")]
        System.Threading.Tasks.Task<string[]> GetPlayersAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGameCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/OnStartGame")]
        void OnStartGame(string namePlayer);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/OnEndGame")]
        void OnEndGame(string namePlayer);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/OnPlayerCanGo")]
        void OnPlayerCanGo();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/OnPlayerExit")]
        void OnPlayerExit();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/OnBadAction")]
        void OnBadAction();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/OnEnemyGoTo")]
        void OnEnemyGoTo(int action);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGame/CheckOnline", ReplyAction="http://tempuri.org/IGame/CheckOnlineResponse")]
        void CheckOnline();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/OnSendMessage")]
        void OnSendMessage(string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGame/OnPlayersChange")]
        void OnPlayersChange(string[] players);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGameChannel : ChatWindow.ServiceReference1.IGame, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GameClient : System.ServiceModel.DuplexClientBase<ChatWindow.ServiceReference1.IGame>, ChatWindow.ServiceReference1.IGame {
        
        public GameClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public GameClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public GameClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public GameClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public GameClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public bool Login(string name) {
            return base.Channel.Login(name);
        }
        
        public System.Threading.Tasks.Task<bool> LoginAsync(string name) {
            return base.Channel.LoginAsync(name);
        }
        
        public bool GoTo(string name, int action) {
            return base.Channel.GoTo(name, action);
        }
        
        public System.Threading.Tasks.Task<bool> GoToAsync(string name, int action) {
            return base.Channel.GoToAsync(name, action);
        }
        
        public System.Nullable<bool> SetMeInCommand(string name) {
            return base.Channel.SetMeInCommand(name);
        }
        
        public System.Threading.Tasks.Task<System.Nullable<bool>> SetMeInCommandAsync(string name) {
            return base.Channel.SetMeInCommandAsync(name);
        }
        
        public System.Nullable<bool> CheckWin(string name) {
            return base.Channel.CheckWin(name);
        }
        
        public System.Threading.Tasks.Task<System.Nullable<bool>> CheckWinAsync(string name) {
            return base.Channel.CheckWinAsync(name);
        }
        
        public void SetWaitStatus(string name, bool status) {
            base.Channel.SetWaitStatus(name, status);
        }
        
        public System.Threading.Tasks.Task SetWaitStatusAsync(string name, bool status) {
            return base.Channel.SetWaitStatusAsync(name, status);
        }
        
        public void Logout(string name) {
            base.Channel.Logout(name);
        }
        
        public System.Threading.Tasks.Task LogoutAsync(string name) {
            return base.Channel.LogoutAsync(name);
        }
        
        public string[] GetPlayers() {
            return base.Channel.GetPlayers();
        }
        
        public System.Threading.Tasks.Task<string[]> GetPlayersAsync() {
            return base.Channel.GetPlayersAsync();
        }
    }
}
