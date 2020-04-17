using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatService
{
    // Гравець
    public class Player
    {
        public string name { get; set; }
        public ICallback callback { get; set; }

        public Player(string name, ICallback callback)
        {
            this.name = name;
            this.callback = callback;
        }
    }

    // Команда
    public class Command
    {
        private List<Player> players { get; set; }
        private List<int> gameField { get; set; }
        private List<int> winCombination { get; set; }
        public bool isStarted { get; set; }
        private int goPlayer { get; set; }
       
        public Command()
        {
            players = new List<Player>();
            isStarted = false;
            goPlayer = 1;

            // Виграшні комбінації
            winCombination = new List<int>()
            {
                123, 345, 678, 136, 247, 358, 148, 346
            };
        }

        // Походити до клітинки
        public bool GoTo(int number)
        {



            return true;
        }

        // Розпочати гру
        public bool Start()
        {
            if (isStarted) return false;

            // Ігрове поле
            gameField = new List<int>{
                1, 2, 3,
                4, 5, 6,
                7, 8, 9
            };

            isStarted = true;

            return true;
        }

        // Зупинити
        public bool Stop()
        {
            if (!isStarted) return false;

            isStarted = false;

            return true;
        }

        // Перевірити чи гравець очікує
        public bool IsWaitingPlayer()
        {
            if(players.Count > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Перевірити чи команда може грати
        public bool CanPlay()
        {
            if (isStarted) return false;
            
            return (players.Count == 2);
        }

        // Перевірити чи команда не пуста
        public bool HasPlayers()
        {
            return (players.Count == 0);    
        }

        // Перевірити чи команда не пуста
        public bool HasPlayer(Player player)
        {
            if (!HasPlayers()) return false;

            foreach (Player p in players)
            {
                if (p.name == player.name) return true;
            }

            return true;
        }

        // Видалити гравця з команди
        public bool RemovePlayer(Player player)
        {
            if (!HasPlayer(player)) return false;

            players.Remove(player);

            return true;
        }

        // Добавити гравця в команду
        public bool AddPlayer(Player player)
        {
            if (HasPlayer(player)) return false;

            players.Add(player);

            return true;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Chat : IGame
    {
        private List<Player> players = new List<Player>();
        private List<Command> commands = new List<Command>();

        // Авторизація
        public bool Login(string name)
        {
            if (PlayerExists(name))
                return false;

            players.Add(new Player(name, OperationContext.Current.GetCallbackChannel<ICallback>()));

            return true;
        }

        // Перевірити існування гравця
        private bool PlayerExists(string name)
        {
            foreach(Player player in players)
            {
                if(player.name == name)
                {
                    return true;
                }
            }

            return false;
        }

        // Отримати команду гравця
        private Command GetCommandPlayer(Player player)
        {

        }

        public void Logout(string name)
        {
            throw new NotImplementedException();
        }

        public void SendPrivateMessasge(string NameFrom, string msg, string NameTo)
        {
            throw new NotImplementedException();
        }
    }
}
