using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace ChatService
{
    // Гравець
    public class Player
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool statusWait { get; set; }
        public ICallback callback { get; set; }

        public Player(int id, string name, ICallback callback)
        {
            this.name = name;
            this.callback = callback;
            this.statusWait = true;
            this.id = id;
        }
    }

    // Команда
    public class Command
    {
        public List<int> playersId { get; set; } // ids
        private List<List<int>> winCombinations { get; set; }
        private List<int> gameField { get; set; }
        private List<int> player1 { get; set; }
        private List<int> player2 { get; set; }

        public bool isStarted { get; set; }
        public bool isEnded { get; set; }
        private int goPlayer { get; set; }
       
        public Command()
        {
            playersId = new List<int>();
            player1 = new List<int>();
            player2 = new List<int>();
            isStarted = false;
            isEnded = false;

            // Виграшні комбінації
            winCombinations = new List<List<int>>()
            {
                new List<int>{ 1,2,3 },
                new List<int>{ 4,5,6 },
                new List<int>{ 7,8,9 },
                new List<int>{ 1,4,7 },
                new List<int>{ 2,5,8 },
                new List<int>{ 3,6,9 },
                new List<int>{ 1,5,9 },
                new List<int>{ 3,5,7 }
            };
        }

        // Походити до клітинки
        public bool GoTo(int idPlayer, int number)
        {
            if (goPlayer != idPlayer) return false;

            int idx = gameField.IndexOf(number);

            if (idx == -1) return false;

            gameField.RemoveAt(idx);

            // Перший гравець
            if (playersId[0] == idPlayer)
            {
                player1.Add(number);
            }
            // Другий гравець
            else if (playersId[1] == idPlayer)
            {
                player2.Add(number);
            }


            // Даємо хід іншому гравцеві
            goPlayer = (goPlayer == playersId[0]) ? playersId[1] : playersId[0];
            
            return true;
        }

        // Перевірити комбінацію
        private bool CheckCombo(int n1, int n2, int n3)
        {
            foreach (List<int> winCombo in winCombinations)
            {
                if(winCombo[0] == n1 && winCombo[1] == n2 && winCombo[2] == n3)
                {
                    return true;
                }
            }

            return false;
        }

        // Отримати id противника
        public int? GetIdEnemy(int idPlayer)
        {
            if (playersId[0] == idPlayer)
            {
                return playersId[1];
            }
            else if (playersId[1] == idPlayer)
            {
                return playersId[0];
            }

            return null;
        }

        // Перевірити виграш
        public bool CheckWin(int idPlayer)
        {
            List<int> nums = new List<int>();

            if (playersId[0] == idPlayer)
            {
                nums = player1?.OrderBy(n => n)?.ToList();
            }
            // Другий гравець
            else if (playersId[1] == idPlayer)
            {
                nums = player2?.OrderBy(n => n)?.ToList();
            }

            if (nums == null || nums.Count < 3) return false;

            int n1, n2, n3;

            for (int i = 0; i < nums.Count; i++)
            {
                n1 = nums[i];

                if (nums.FindIndex(delegate (int idx) { return idx == i + 1; }) == -1) {
                    break;
                }
                n2 = nums[i + 1];

                if (nums.FindIndex(delegate (int idx) { return idx == i + 2; }) == -1)
                {
                    break;
                }
                n3 = nums[i + 2];


                if (CheckCombo(n1,n2,n3))
                {
                    return true;
                }
            }

            return false;
        }

        // Розпочати гру
        public bool Start()
        {
            if (isStarted || isEnded) return false;

            // Ігрове поле
            gameField = new List<int>{
                1, 2, 3,
                4, 5, 6,
                7, 8, 9
            };

            isStarted = true;

            goPlayer = playersId[0];

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
            if(playersId.Count > 1)
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
            if (isStarted || isEnded) return false;
            
            return (playersId.Count == 2);
        }

        // Перевірити чи команда не пуста
        public bool HasPlayers()
        {
            return (playersId.Count == 0);    
        }

        // Перевірити чи команда не пуста
        public bool HasPlayer(int pId)
        {
            foreach (int id in playersId)
            {
                if (id == pId) return true;
            }

            return false;
        }

        // Видалити гравця з команди
        public bool RemovePlayer(int pId)
        {
            if (!HasPlayer(pId)) return false;

            playersId.Remove(pId);

            return true;
        }

        // Добавити гравця в команду
        public bool AddPlayer(int pId)
        {
            if (HasPlayer(pId)) return false;

            playersId.Add(pId);

            return true;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Chat : IGame
    {
        private List<Player> players = new List<Player>();
        private List<Command> commands = new List<Command>();
        private int counterId { get; set; }

        public Chat()
        {
            counterId = 0;
        }

        // Перевірка перемоги
        public bool? CheckWin(string name)
        {
            if (!PlayerExists(name))
                return null;

            int? idxPlayer = GetIdxPlayerByName(name);
            int? idxCommand = GetCommandIdxByPlayer(name);

            if (idxCommand == null || idxPlayer == null) return null;

            if (!commands[Convert.ToInt32(idxCommand)].isStarted) return null;

            for (int p = 0; p < 2; p++)
            {
                // Перевіряємо гравця на перемогу
                if (commands[Convert.ToInt32(idxCommand)].CheckWin(commands[Convert.ToInt32(idxCommand)].playersId[p]))
                {
                    int? idEnemy = commands[Convert.ToInt32(idxCommand)].GetIdEnemy(commands[Convert.ToInt32(idxCommand)].playersId[p]);
                    GetPlayerById(Convert.ToInt32(idEnemy)).statusWait = true;
                    GetPlayerById(commands[Convert.ToInt32(idxCommand)].playersId[p]).statusWait = true;
                    commands[Convert.ToInt32(idxCommand)].isStarted = false;
                    commands[Convert.ToInt32(idxCommand)].isEnded = true;

                    // Сповіщаємо переможця
                    if (GetPlayerById(commands[Convert.ToInt32(idxCommand)].playersId[p]).name == name)
                    {
                        // Сповіщаємо суперника
                        if (idEnemy != null)
                        {
                            
                            GetPlayerById(Convert.ToInt32(idEnemy)).callback.OnEndGame(GetPlayerById(commands[Convert.ToInt32(idxCommand)].playersId[p]).name);
                        }
                        return true;
                    }
                    else
                    {
                        GetPlayerById(commands[Convert.ToInt32(idxCommand)].playersId[p]).callback.OnEndGame(GetPlayerById(commands[Convert.ToInt32(idxCommand)].playersId[p]).name);
                        return false;
                    }
                }
            }

            return null;
        }

        // Розприділення гравців
        public bool? SetMeInCommand(string name)
        {
            if (!PlayerExists(name))
            {
                return false;
            }
                
            int? idxPlayer = GetIdxPlayerByName(name);
            int? idxCommand = GetCommandIdxByPlayer(name);

            if (idxCommand != null || idxPlayer == null) return false;

            bool setted = false;

            lock (commands)
            {

                // Якщо користувач очікує, то підбираємо команду
                if (players[Convert.ToInt32(idxPlayer)].statusWait == true)
                {
                    // Якщо команди є
                    if (commands.Count > 0)
                    {
                        lock (commands)
                        {
                            for (int c = 0; c < commands.Count; c++)
                            {
                                if (commands[c].IsWaitingPlayer())
                                {
                                    players[Convert.ToInt32(idxPlayer)].statusWait = false;
                                    commands[c].AddPlayer(players[Convert.ToInt32(idxPlayer)].id);
                                    setted = true;

                                    if (commands[c].CanPlay())
                                    {
                                        commands[c].Start();

                                        // Сповіщаємо 
                                        if (GetPlayerById(commands[Convert.ToInt32(idxCommand)].playersId[1]).name == name)
                                        {
                                            // Сповіщаємо суперника
                                            GetPlayerById(commands[c].playersId[0]).callback.OnStartGame(GetPlayerById(commands[c].playersId[1]).name);
                                            GetPlayerById(commands[c].playersId[0]).callback.OnPlayerCanGo();
                                           
                                        }
                                       
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                            
                if(!setted)
                {
                    Command c = new Command();
                    players[Convert.ToInt32(idxPlayer)].statusWait = false;
                    c.AddPlayer(players[Convert.ToInt32(idxPlayer)].id);

                    lock (commands)
                    {
                        commands.Add(c);
                        return null;
                    }

                }
            }

            return true;
        }  

        // Авторизація
        public bool Login(string name)
        {
            if (PlayerExists(name))
                return false;

            lock(players)
            {
                players.Add(new Player(++counterId, name, OperationContext.Current.GetCallbackChannel<ICallback>()));
            }

            NotifPlayersChanged(name);

            return true;
        }

        // Отримати активних гравців
        public string[] GetPlayers()
        {
            return players.Select(i => i.name).ToArray();
        }

        // Вийти
        public void Logout(string name)
        {
            RemovePlayer(name);
            NotifPlayersChanged();
        }

        // Сповістити всіх про оновлення списку
        private void NotifPlayersChanged(string name = null)
        {
            lock(players)
            {
                string[] playersOnline = players.Select(p => p.name).ToArray();

                for (int p = 0; p < players.Count; p++)
                {
                    if(name != null)
                    {
                        if (players[p].name == name) continue;
                    }

                    try
                    {
                        players[p].callback.OnPlayersChange(playersOnline);
                    }
                    catch (Exception ex)
                    {
                        RemovePlayer(players[p].name);
                    }
                }
            }
        }

        // Походити до
        public bool GoTo(string name, int action)
        {
            int? idxPlayer = GetIdxPlayerByName(name);
            int? idxCommand = GetCommandIdxByPlayer(name);

            if (idxCommand == null || idxPlayer == null) return false;

            // Пробуємо зробити хід
            if(commands[Convert.ToInt32(idxCommand)].GoTo(players[Convert.ToInt32(idxPlayer)].id, action))
            {
                int? idEnemy = commands[Convert.ToInt32(idxCommand)].GetIdEnemy(players[Convert.ToInt32(idxPlayer)].id);

                if(idEnemy != null)
                {
                    int? idxEnemy = GetIdxPlayerById(Convert.ToInt32(idEnemy));

                    // Сповіщаємо суперника, що його хід
                    players[Convert.ToInt32(idxEnemy)].callback.OnEnemyGoTo(action);
                    players[Convert.ToInt32(idxEnemy)].callback.OnPlayerCanGo();
                }
                else
                {
                    // Якщо суперника не знайдено, то сповіщаємо і ставимо в режим очікування
                   // players[Convert.ToInt32(idxPlayer)].callback.OnPlayerExit();
                    players[Convert.ToInt32(idxPlayer)].statusWait = true;
                }
                return true;
            }else
            {
              //  players[Convert.ToInt32(idxPlayer)].callback.OnBadAction();
                return false;
            } 
        }

        // Видалити гравця із списку
        private void RemovePlayer(string name)
        {
            lock (commands)
            {
                try
                {
                    Player p = GetPlayerByName(name);

                    for (int c = 0; c < commands.Count; c++)
                    {
                        if (commands[c].HasPlayer(p.id))
                        {
                            commands[c].RemovePlayer(p.id);

                            lock (players)
                            {
                                players.Remove(p);
                            }

                            foreach (int pId in commands[c].playersId)
                            {
                                players[pId].callback.OnPlayerExit();
                                players[pId].statusWait = true;
                            }

                            commands.Remove(commands[c]);
                            break;
                        }
                    }
                }
                catch(Exception ex)
                {

                }
                
            }
        }


        // Отримати команду гравця
        private Command GetCommandPlayer(string name)
        {
            lock(commands)
            {
                for (int c = 0; c < commands.Count; c++)
                {
                    Player p = GetPlayerByName(name);

                    if (p == null) continue;

                    if (commands[c].HasPlayer(p.id))
                    {
                        return commands[c];
                    }
                }
            }

            return null;
        }

        // Отримати команду гравця
        private int? GetCommandIdxByPlayer(string name)
        {
            lock (commands)
            {
                for (int c = 0; c < commands.Count; c++)
                {
                    Player p = GetPlayerByName(name);

                    if (p == null) continue;

                    if (commands[c].HasPlayer(p.id))
                    {
                        return c;
                    }
                }
            }

            return null;
        }

        // Отримати id гравця
        private int? GetIdxPlayerByName(string name)
        {

            lock (players)
            {
                for (int p = 0; p < players.Count; p++)
                {
                    if (players[p].name == name)
                    {
                        return p;
                    }
                }
                return null;
            }
        }

        // Отримати idx гравця
        private int? GetIdxPlayerById(int id)
        {

            lock (players)
            {
                for (int p = 0; p < players.Count; p++)
                {
                    if (players[p].id == id)
                    {
                        return p;
                    }
                }
                return null;
            }
        }

        // Отримати гравця
        private Player GetPlayerByName(string name)
        {
            lock(players)
            {
                for (int p = 0; p < players.Count; p++)
                {
                    if (players[p].name == name)
                    {
                        return players[p];
                    }
                }
            }

            return null;
        }

        // Отримати гравця по Id
        private Player GetPlayerById(int id)
        {
            lock (players)
            {
                for (int p = 0; p < players.Count; p++)
                {
                    if (players[p].id == id)
                    {
                        return players[p];
                    }
                }
            }

            return null;
        }

        // Встановити статус очікування на гру
        public void SetWaitStatus(string name, bool status = true)
        {
            lock(players)
            {
                for (int p = 0; p < players.Count; p++)
                {
                    if (players[p].name == name)
                    {
                        players[p].statusWait = status;
                    }
                }
            }
        }

        // Перевірити існування гравця
        private bool PlayerExists(string name)
        {
            lock (players)
            {
                for (int p = 0; p < players.Count; p++)
                {
                    if (players[p].name == name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
