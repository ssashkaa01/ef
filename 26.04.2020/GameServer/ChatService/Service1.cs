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
        public string name { get; set; }
        public bool statusWait { get; set; }
        public ICallback callback { get; set; }

        public Player(string name, ICallback callback)
        {
            this.name = name;
            this.callback = callback;
            this.statusWait = true;
        }
    }

    // Команда
    public class Command
    {
        public List<Player> players { get; set; }
        private List<int> gameField { get; set; }
        private List<int> winCombination { get; set; }
        private List<int> player1 { get; set; }
        private List<int> player2 { get; set; }


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

        public Chat()
        {
            Thread t = new Thread(new ThreadStart(PlayerSortProc));
            t.IsBackground = true;
            t.Start();

            Thread t2 = new Thread(new ThreadStart(GameProc));
            t2.IsBackground = true;
            t2.Start();

            Thread t3 = new Thread(new ThreadStart(PlayerCheckProc));
            t3.IsBackground = true;
            t3.Start();
        }

        // Розприділення гравців
        public void PlayerSortProc()
        {
            while(true)
            {
                Thread.Sleep(70);

                lock(players)
                {
                    // Перевіряємо всіх користувачів на очікування гри
                    foreach (Player p in players)
                    {
                        // Якщо користувач очікує, то підбираємо команду
                        if (p.statusWait == true)
                        {
                            bool setted = false;

                            if (commands.Count > 0)
                            {
                                
                                lock(commands)
                                {
                                    foreach (Command comm in commands)
                                    {
                                        if (comm.IsWaitingPlayer())
                                        {
                                            comm.AddPlayer(p);
                                            setted = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            
                            if(!setted)
                            {
                                Command c = new Command();
                                p.statusWait = false;
                                c.AddPlayer(p);

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

        // Ігровий процес
        public void GameProc()
        {
            while (true)
            {
                Thread.Sleep(100);

                lock(commands)
                {
                    foreach (Command c in commands)
                    {
                        // Гравці відстуні
                      //  if (!c.HasPlayers())
                        {
                         //   commands.Remove(c);
                        }
                        // Можна грати
                       // else 
                        if (c.CanPlay())
                        {
                            c.Start();
                            c.players[0].callback.OnStartGame(c.players[1].name);
                            c.players[1].callback.OnStartGame(c.players[0].name);

                            c.players[0].callback.OnPlayerCanGo();
                        }
                    }
                }
            }
        }

        // Ігровий процес
        public void PlayerCheckProc()
        {
            while (true)
            {
                Thread.Sleep(20);

                lock(players)
                {
                    // Перевіряємо всіх користувачів
                    foreach (Player p in players)
                    {
                        try
                        {
                            p.callback.CheckOnline();
                        }
                        catch (Exception ex)
                        {
                            RemovePlayer(p.name);
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
                players.Add(new Player(name, OperationContext.Current.GetCallbackChannel<ICallback>()));
            }

            return true;
        }

        public string[] GetPlayers()
        {
            return players.Select(i => i.name).ToArray();
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

        // Вийти
        public void Logout(string name)
        {
            RemovePlayer(name);
            NotifPlayersChanged();
        }

        // Сповістити всіх про оновленн списку
        private void NotifPlayersChanged()
        {
            lock(players)
            {
                string[] playersOnline = players.Select(p => p.name).ToArray();

                foreach (Player p in players)
                {
                    try
                    {
                        p.callback.OnPlayersChange(playersOnline);
                    }
                    catch (Exception ex)
                    {
                        RemovePlayer(p.name);
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

                foreach (Command c in commands)
                {
                    if (c.HasPlayer(p))
                    {
                        c.RemovePlayer(p);

                        foreach (Player pl in c.players)
                        {
                            pl.callback.OnPlayerExit();
                            pl.statusWait = true;
                        }

                        commands.Remove(c);
                        break;
                    }
                }
            }
            
            lock(players)
            {
                foreach (Player pl in players)
                {
                    if (pl.name == name)
                    {
                        players.Remove(pl);
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
                foreach (Command c in commands)
                {
                    Player p = GetPlayerByName(name);

                    if (p == null) continue;

                    if (c.HasPlayer(p))
                    {
                        return c;
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
                foreach (Player p in players)
                {
                    if (p.name == name)
                    {
                        return p;
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
                foreach (Player p in players)
                {
                    if (p.name == name)
                    {
                        p.statusWait = status;
                    }
                }
            }
        }
    }
}
