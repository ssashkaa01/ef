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
    public delegate void CallbackDelegateOnRecieve(string user, string msg);
    public delegate void CallbackDelegateOnUsersChange(string[] users);
    public delegate void CallbackDelegateOnReceivePrivateMessage(string user, string msg);

    class CallbackHandler : IChatCallback
    {
        static public event CallbackDelegateOnRecieve OnRecieve;
        static public event CallbackDelegateOnUsersChange OnUsersChange;
        static public event CallbackDelegateOnReceivePrivateMessage OnReceivePrivateMessage;

        public void RecieveMessage(string user, string msg)
        {
            OnRecieve(user, msg);
        }

        public void UsersChange(string[] users)
        {
            OnUsersChange(users);
        }

        public void ReceivePrivateMessage(string user, string msg)
        {
            OnReceivePrivateMessage(user, msg);
        }
    }
   
    public partial class MainWindow : Window
    {
        static InstanceContext instance = new InstanceContext(new CallbackHandler());
        ChatClient proxy = new ChatClient(instance);

        List<string> listMainChat = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            msgList.ItemsSource = listMainChat;

            CallbackHandler.OnRecieve += AddMessageToMainChat;
            CallbackHandler.OnUsersChange += ListUsersChanged;
            CallbackHandler.OnReceivePrivateMessage += ReceivePrivateMessage;

            Logout.IsEnabled = false;
            Send.IsEnabled = false;
            SendPrivate.IsEnabled = false;
        }

        // Отримано повідомлення в головному чаті
        private void AddMessageToMainChat(string user, string msg)
        {
            listMainChat.Add(user + " : " + msg);
            msgList.Items.Refresh();
        }

        // Змінено список активних користувачів
        private void ListUsersChanged(string[] users)
        {
            usersList.Items.Clear();

            foreach(string name in users)
            {
                if(name != loginTb.Text)
                    usersList.Items.Add(name);
            }
        }

        // Отримано приватне повідомлення
        private void ReceivePrivateMessage(string user, string msg)
        {
            listMainChat.Add(user + " [PRIVATE]: " + msg);

            msgList.Items.Refresh();
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
            Send.IsEnabled = true;
            SendPrivate.IsEnabled = true;

            string []users = proxy.GetUsers();

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
            Send.IsEnabled = false;
            SendPrivate.IsEnabled = false;
            usersList.Items.Clear();
        }

        // Відправити повідомлення
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            proxy.SendMessasge(loginTb.Text, msgTb.Text);

            listMainChat.Add(loginTb.Text + " : "+ msgTb.Text);

            msgList.Items.Refresh();
        }

        private void UsersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SendPrivate_Click(object sender, RoutedEventArgs e)
        {
            if(usersList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select user from list");
                return;
            }

            proxy.SendPrivateMessasge(loginTb.Text, msgTb.Text, usersList.SelectedValue.ToString());

            listMainChat.Add(loginTb.Text + "[PRIVATE TO " + usersList.SelectedItem.ToString() + "] : " + msgTb.Text);

            msgList.Items.Refresh();
        }
    }
}
