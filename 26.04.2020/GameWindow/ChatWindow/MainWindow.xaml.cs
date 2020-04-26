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

    class CallbackHandler : IGameCallback
    {
        static public event CallbackDelegateOnStartGame StartGame;
        static public event CallbackDelegateOnEndGame EndGame;
        static public event CallbackDelegateOnPlayersChange PlayersChange;
        static public event CallbackDelegateOnPlayerCanGo PlayerCanGo;
        static public event CallbackDelegateOnPlayerExit PlayerExit;

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
    }
   
    public partial class MainWindow : Window
    {
        static InstanceContext instance = new InstanceContext(new CallbackHandler());
        GameClient proxy = new GameClient(instance);
        bool canGo = true;
        bool gameStarted = true;

        List<string> listMainChat = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            CallbackHandler.PlayersChange += ListPlayersChanged;
            CallbackHandler.StartGame += OnStartGame;
            CallbackHandler.EndGame += OnEndGame;
            CallbackHandler.PlayerCanGo += OnPlayerCanGo;
            CallbackHandler.PlayerExit += OnPlayerExit;

            int counter = 1;

            foreach(Button btn in gameField.Children)
            {
                btn.Content = $"{counter}";
                counter++;
            }

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

        // Гравець покинув гру
        private void OnPlayerExit()
        {
            MessageBox.Show("Гравець покинув гру!");
        }

        // Гравець може ходити
        private void OnPlayerCanGo()
        {
            MessageBox.Show("Ваш хід!");
        }

        // Гра завершена 
        private void OnEndGame(string namePlayer)
        {
            throw new NotImplementedException();
        }

        // Гру розпочато
        private void OnStartGame(string namePlayer)
        {
            MessageBox.Show($"You play with: {namePlayer}");

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
                MessageBox.Show("User is already exist.");
                return;
            }

            Login.IsEnabled = false;
            loginTb.IsEnabled = false;
            Logout.IsEnabled = true;
           
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
            Button btnClick = (sender as Button);

            if (!canGo)
            {
                MessageBox.Show("Going other player!");
                return;
            }

            try
            {
                int numberBtn = Convert.ToInt32(btnClick.Content);
            }
            catch (Exception ex)
            {
                MessageBox.Show("You can't go to this.");
                return;
            }

            btnClick.Content = "O";
        }
    }
}
