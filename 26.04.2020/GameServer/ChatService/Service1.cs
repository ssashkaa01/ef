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
        public List<int> players { get; set; } // ids
        private List<int> gameField { get; set; }
        private List<int> winCombination { get; set; }
        private List<int> player1 { get; set; }
        private List<int> player2 { get; set; }


        public bool isStarted { get; set; }
        private int goPlayer { get; set; }
       
        public Command()
        {
            players = new List<int>();
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
        public bool HasPlayer(int pId)
        {
            foreach (int id in players)
            {
                if (id == pId) return true;
            }

            return false;
        }

        // Видалити гравця з команди
        public bool RemovePlayer(int pId)
        {
            if (!HasPlayer(pId)) return false;

            players.Remove(pId);

            return true;
        }

        // Добавити гравця в команду
        public bool AddPlayer(int pId)
        {
            if (HasPlayer(pId)) return false;

            players.Add(pId);

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

            Thread t = new Thread(new ThreadStart(PlayerSortProc));
            t.IsBackground = true;
            t.Start();
        }

        // Розприділення гравців
        public void PlayerSortProc()
        {
            while(true)
            {
                Thread.Sleep(100);

                lock(players)
                {
                    // Перевіряємо всіх користувачів на очікування гри
                    for (int i = 0; i < players.Count; i++)
                    {
                        try
                        {
                            players[i].callback.CheckOnline();
                        }
                        catch (Exception ex)
                        {
                            RemovePlayer(players[i].name);
                            continue;
                        }

                        // Якщо користувач очікує, то підбираємо команду
                        if (players[i].statusWait == true)
                        {
                            bool setted = false;

                            if (commands.Count > 0)
                            {
                                
                                lock(commands)
                                {
                                    for (int c = 0; c < commands.Count; c++)
                                    {
                                        if (commands[c].IsWaitingPlayer())
                                        {
                                            players[i].statusWait = false;
                                            commands[c].AddPlayer(players[i].id);
                                            setted = true;

                                            if (commands[c].CanPlay())
                                            {
                                                commands[c].Start();

                                                players[commands[c].players[0]].callback.OnStartGame(players[commands[c].players[0]].name);
                                                players[commands[1].players[1]].callback.OnStartGame(players[commands[c].players[1]].name);

                                                players[commands[c].players[0]].callback.OnPlayerCanGo();
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            
                            if(!setted)
                            {
                                Command c = new Command();
                                players[i].statusWait = false;
                                c.AddPlayer(players[i].id);

                                lock (commands)
                                {
                                    commands.Add(c);
                                }

                            }
                        }
                    }
                }
            }
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

        // Сповістити всіх про оновленн списку
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
            throw new NotImplementedException();
        }

        // Видалити гравця із списку
        private void RemovePlayer(string name)
        {
            lock (commands)
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

                        foreach (int pId in commands[c].players)
                        {
                            players[pId].callback.OnPlayerExit();
                            players[pId].statusWait = true;
                        }

                        commands.Remove(commands[c]);
                        break;
                    }
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
