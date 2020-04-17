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
    public delegate void CallbackDelegateOnRecieve(string msg);
    public delegate void CallbackDelegateOnUsersChange(string[] users);
    public delegate void CallbackDelegateOnReceivePrivateMessage(string user, string msg);

    class CallbackHandler : IChatCallback
    {
        static public event CallbackDelegateOnRecieve OnRecieve;
        static public event CallbackDelegateOnUsersChange OnUsersChange;
        static public event CallbackDelegateOnReceivePrivateMessage OnReceivePrivateMessage;

        public void RecieveMessage(string msg)
        {
            OnRecieve(msg);
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
        }

        // Отримано повідомлення в головному чаті
        private void AddMessageToMainChat(string msg)
        {
            listMainChat.Add(msg);
            msgList.Items.Refresh();
        }

        // Змінено список активних користувачів
        private void ListUsersChanged(string[] users)
        {

        }

        // Отримано приватне повідомлення
        private void ReceivePrivateMessage(string user, string msg)
        {

        }

        // Авторизуватись
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (!proxy.Login(loginTb.Text))
                MessageBox.Show("User is already exist.");
        }

        // Вийти
        private void Logout_Click(object sender, RoutedEventArgs e)
        {

        }

        // Відправити повідомлення
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            proxy.SendMessasge(loginTb.Text, msgTb.Text);

            listMainChat.Add(loginTb.Text + " : "+ msgTb.Text);

            msgList.Items.Refresh();
        }
    }
}
