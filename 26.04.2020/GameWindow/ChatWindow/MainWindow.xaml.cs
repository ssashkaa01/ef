using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatWindow.ServiceReference1;

namespace ChatWindow
{
    public delegate void CallbackDelegateOnStartGame(string namePlayer);
    public delegate void CallbackDelegateOnEndGame(string namePlayer);
    public delegate void CallbackDelegateOnPlayerCanGo();
    public delegate void CallbackDelegateOnPlayerExit();
    public delegate void CallbackDelegateOnPlayersChange(string[] players);
    public delegate void CallbackDelegateOnSendMessage(string message);
    public delegate void CallbackDelegateOnBadAction();
    public delegate void CallbackDelegateOnEnemyGoTo(int action);

    class CallbackHandler : IGameCallback
    {
        static public event CallbackDelegateOnStartGame StartGame;
        static public event CallbackDelegateOnEndGame EndGame;
        static public event CallbackDelegateOnPlayersChange PlayersChange;
        static public event CallbackDelegateOnPlayerCanGo PlayerCanGo;
        static public event CallbackDelegateOnPlayerExit PlayerExit;
        static public event CallbackDelegateOnBadAction BadAction;
        static public event CallbackDelegateOnSendMessage SendMessage;
        static public event CallbackDelegateOnEnemyGoTo EnemyGoTo;

        // Гра розпочалася
        public void OnStartGame(string namePlayer)
        {
            StartGame(namePlayer);
        }

        // Гра завершена
        public void OnEndGame(string namePlayer)
        {
            EndGame(namePlayer);
        }

        // Гравці змінено
        public void OnPlayersChange(string[] users)
        {
            PlayersChange(users);
        }

        // Користувач може ходити
        public void OnPlayerCanGo()
        {
            PlayerCanGo();
        }

        // Користувач покинув гру
        public void OnPlayerExit()
        {
            PlayerExit();
        }

        public void CheckOnline()
        {
            return;
        }

        public void OnBadAction()
        {
            BadAction();
        }

        public void OnSendMessage(string message)
        {
            SendMessage(message);
        }

        public void OnEnemyGoTo(int action)
        {
            EnemyGoTo(action);
        }
    }
   
    public partial class MainWindow : Window
    {
        static InstanceContext instance = new InstanceContext(new CallbackHandler());
        GameClient proxy = new GameClient(instance);
        bool canGo = false;
        bool gameStarted = false;

        List<string> listMainChat = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            CallbackHandler.PlayersChange += ListPlayersChanged;
            CallbackHandler.StartGame += OnStartGame;
            CallbackHandler.EndGame += OnEndGame;
            CallbackHandler.PlayerCanGo += OnPlayerCanGo;
            CallbackHandler.PlayerExit += OnPlayerExit;
            CallbackHandler.BadAction += OnBadAction;
            CallbackHandler.SendMessage += OnSendMessage;
            CallbackHandler.EnemyGoTo += OnEnemyGoTo;

            int counter = 1;

            foreach(Button btn in gameField.Children)
            {
                btn.Content = $"{counter}";
                counter++;
            }

        }

        private void OnEnemyGoTo(int action)
        {
            int counter = 1;

            foreach (Button btn in gameField.Children)
            {
                if(counter == action)
                {
                    btn.Content = "X";
                }
               
                counter++;
            }
        }

        private void OnSendMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void OnBadAction()
        {
            MessageBox.Show("Ви не можете походити");
        }

        // Змінено список активних гравців
        private void ListPlayersChanged(string[] players)
        {
            usersList.Items.Clear();

            foreach (string name in players)
            {
                if (name != loginTb.Text)
                    usersList.Items.Add(name);
            }
        }

        private void StopGame()
        {
            canGo = false;
            gameStarted = false;
        }

        // Гравець покинув гру
        private void OnPlayerExit()
        {
            StopGame();
            MessageBox.Show("Гравець покинув гру!");
        }

        // Гравець може ходити
        private void OnPlayerCanGo()
        {
            canGo = true;
            MessageBox.Show("Ваш хід!");
        }

        // Гра завершена 
        private void OnEndGame(string namePlayer)
        {
            StopGame();

            if(loginTb.Text == namePlayer)
            {
                MessageBox.Show("Ви перемогли");
            } else
            {
                MessageBox.Show("Ви програли");
            }
        }

        // Гру розпочато
        private void OnStartGame(string namePlayer)
        {
            MessageBox.Show($"Ви граєте з: {namePlayer}");

            gameStarted = true;

            int counter = 1;

            foreach (Button btn in gameField.Children)
            {
                btn.Content = $"{counter}";

                counter++;
            }
        }

        // Авторизуватись
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (!proxy.Login(loginTb.Text))
            {
                MessageBox.Show("Користувач з таким іменем авторизований");
                return;
            }

            Login.IsEnabled = false;
            loginTb.IsEnabled = false;
            Logout.IsEnabled = true;

            bool? res = proxy.SetMeInCommand(loginTb.Text);

            if(res != null)
            {
                if (Convert.ToBoolean(res))
                {
                    gameStarted = true;
                    MessageBox.Show("Вас добавлено в команду!");
                }
                else
                {
                    MessageBox.Show("Помилка додавання в команду!");
                }
            } else
            {
                MessageBox.Show("Очікуйте суперника");
            }

            string[] users = proxy.GetPlayers();

            foreach (string name in users)
            {
                if (name != loginTb.Text)
                    usersList.Items.Add(name);
            }
        }

        // Вийти
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            proxy.LogoutAsync(loginTb.Text);
            loginTb.IsEnabled = true;
            Logout.IsEnabled = false;
            Login.IsEnabled = true;
         
            usersList.Items.Clear();
        }

        // Клік по клітинці
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!gameStarted)
            {
                MessageBox.Show("Очікуйте суперника!");
                return;
            }
            else if (!canGo)
            {
                MessageBox.Show("Зараз не ваш хід!");
                return;
            }

            Button btnClick = (sender as Button);
            int action;

            try
            {
                action = Convert.ToInt32(btnClick.Content);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка ходу!");
                return;
            }

            if(proxy.GoTo(loginTb.Text, action))
            {
                bool? res = proxy.CheckWin(loginTb.Text);

                if (res != null)
                {
                    if(Convert.ToBoolean(res))
                    {
                        MessageBox.Show("Ви перемогли");
                    }else
                    {
                        MessageBox.Show("Ви програли");
                    }
                }
                    
                btnClick.Content = "O";
                canGo = false;
            }
            else
            {
                MessageBox.Show("Помилка ходу!");
            }
        }
    }
}
