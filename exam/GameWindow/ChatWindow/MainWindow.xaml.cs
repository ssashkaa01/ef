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

                // Клік по клітинці
                btn.Click += Game_Btn_Click;

                counter++;
            }

        }

        // Змінено список активних гравців
        private void ListPlayersChanged(string[] players)
        {
            throw new NotImplementedException();
        }

        // Гравець покинув гру
        private void OnPlayerExit()
        {
            throw new NotImplementedException();
        }

        // Гравець може ходити
        private void OnPlayerCanGo()
        {
            throw new NotImplementedException();
        }

        // Гра завершена 
        private void OnEndGame(string namePlayer)
        {
            throw new NotImplementedException();
        }

        // Гру розпочато
        private void OnStartGame(string namePlayer)
        {
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
                MessageBox.Show("Player is already exist.");
        }

        // Вийти
        private void Logout_Click(object sender, RoutedEventArgs e)
        {

        }

        // Клік по клітинці
        private void Game_Btn_Click(object sender, RoutedEventArgs e)
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
