using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace kolko_i_krzyzyk_lan
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        public Game(bool isHost, string ip = null)
        {
            InitializeComponent();
            initServer(isHost,ip);
        }
        private void reciever_DoWork(object sender, DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (check())
                    return;
            });
            Application.Current.Dispatcher.Invoke(() =>
            {
                stopGame();
            });
            MessageBox.Show("Your turn!");
            Application.Current.Dispatcher.Invoke(() =>
            {
                move();
            });
            MessageBox.Show("Opponents turn!");
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!check())
                    startBoard();
            });
        }

        private char player;
        private char opponent;
        private Socket socket;
        private BackgroundWorker reciever = new BackgroundWorker();
        private TcpListener server = null;
        private TcpClient client;

        private void initServer(bool isHost, string ip)
        {
            reciever.DoWork += reciever_DoWork;
            if (isHost)
            {
                player = 'x';
                opponent = 'o';
                server = new TcpListener(System.Net.IPAddress.Any, 5732);
                server.Start();
                socket = server.AcceptSocket();
            }
            else
            {
                player = 'o';
                opponent = 'x';
                try
                {
                    client = new TcpClient(ip, 5732);
                    socket = client.Client;
                    reciever.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }
        private bool check()
        {
            if ((string) button1.Content == (string) button2.Content
                && (string)button2.Content == (string)button3.Content
                && (string) button3.Content != "3")
            {
                if ((string)button1.Content == "x")
                {
                    MessageBox.Show("You won!");
                }
                else
                {
                    MessageBox.Show("You lost!");
                }
                return true;
            }
            if ((string)button4.Content == (string)button5.Content && (string)button5.Content == (string)button6.Content && (string)button6.Content != "6")
            {
                if ((string)button4.Content == "x")
                {
                    MessageBox.Show("You won!");
                }
                else
                {
                    MessageBox.Show("You lost!");
                }
                return true;
            }
            if ((string)button7.Content == (string)button8.Content && (string)button8.Content == (string)button9.Content && (string)button9.Content != "9")
            {
                if ((string)button7.Content == "x")
                {
                    MessageBox.Show("You won!");
                }
                else
                {
                    MessageBox.Show("You lost!");
                }
                return true;
            }
            if ((string)button1.Content == (string)button5.Content && (string)button5.Content == (string)button9.Content && (string)button9.Content != "9")
            {
                if ((string)button1.Content == "x")
                {
                    MessageBox.Show("You won!");
                }
                else
                {
                    MessageBox.Show("You lost!");
                }
                return true;
            }
            if ((string)button3.Content == (string)button5.Content && (string)button5.Content == (string)button7.Content && (string)button7.Content != "7")
            {
                if ((string)button3.Content == "x")
                {
                    MessageBox.Show("You won!");
                }
                else
                {
                    MessageBox.Show("You lost!");
                }
                return true;
            }
            else if((string)button1.Content != "1" && (string)button2.Content != "2" && (string)button3.Content != "3" 
                && (string)button4.Content != "4" && (string)button5.Content != "5" && (string)button6.Content != "6" 
                && (string)button7.Content != "7" && (string)button8.Content != "8" && (string)button9.Content != "9")
            {
                MessageBox.Show("It's a draw!");
            }
            return false;
        }
        private void stopGame()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                button1.IsEnabled = false;
                button2.IsEnabled = false;
                button3.IsEnabled = false;
                button4.IsEnabled = false;
                button5.IsEnabled = false;
                button6.IsEnabled = false;
                button7.IsEnabled = false;
                button8.IsEnabled = false;
                button9.IsEnabled = false;
            });
        }
        private void startBoard()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if ((string)button1.Content == "1")
                    button1.IsEnabled = true;
                if ((string)button2.Content == "2")
                    button2.IsEnabled = true;
                if ((string)button3.Content == "3")
                    button3.IsEnabled = true;
                if ((string)button4.Content == "4")
                    button4.IsEnabled = true;
                if ((string)button5.Content == "5")
                    button5.IsEnabled = true;
                if ((string)button6.Content == "6")
                    button6.IsEnabled = true;
                if ((string)button7.Content == "7")
                    button7.IsEnabled = true;
                if ((string)button8.Content == "8")
                    button8.IsEnabled = true;
                if ((string)button9.Content == "9")
                    button9.IsEnabled = true;
            });
        }
        private void move()
        {
            byte[] buffer = new byte[1];
            socket.Receive(buffer);
            if (buffer[0] == 1)
                button1.Content = opponent.ToString();
            if (buffer[0] == 2)
                button2.Content = opponent.ToString();
            if (buffer[0] == 3)
                button3.Content = opponent.ToString();
            if (buffer[0] == 4)
                button4.Content = opponent.ToString();
            if (buffer[0] == 5)
                button5.Content = opponent.ToString();
            if (buffer[0] == 6)
                button6.Content = opponent.ToString();
            if (buffer[0] == 7)
                button7.Content = opponent.ToString();
            if (buffer[0] == 8)
                button8.Content = opponent.ToString();
            if (buffer[0] == 9)
                button9.Content = opponent.ToString();
        }

        private void btn1_click(object sender, EventArgs e){
            byte[] num = { 1 };
            socket.Send(num);
            button1.Content = player.ToString();
            reciever.RunWorkerAsync();
        }
        private void btn2_click(object sender, EventArgs e)
        {
            byte[] num = { 2 };
            socket.Send(num);
            button2.Content = player.ToString();
            reciever.RunWorkerAsync();
        }
        private void btn3_click(object sender, EventArgs e)
        {
            byte[] num = { 3 };
            socket.Send(num);
            button3.Content = player.ToString();
            reciever.RunWorkerAsync();
        }
        private void btn4_click(object sender, EventArgs e)
        {
            byte[] num = { 4 };
            socket.Send(num);
            button4.Content = player.ToString();
            reciever.RunWorkerAsync();
        }
        private void btn5_click(object sender, EventArgs e)
        {
            byte[] num = { 5 };
            socket.Send(num);
            button5.Content = player.ToString();
            reciever.RunWorkerAsync();
        }
        private void btn6_click(object sender, EventArgs e)
        {
            byte[] num = { 6 };
            socket.Send(num);
            button6.Content = player.ToString();
            reciever.RunWorkerAsync();
        }
        private void btn7_click(object sender, EventArgs e)
        {
            byte[] num = { 7 };
            socket.Send(num);
            button7.Content = player.ToString();
            reciever.RunWorkerAsync();
        }
        private void btn8_click(object sender, EventArgs e)
        {
            byte[] num = { 8 };
            socket.Send(num);
            button8.Content = player.ToString();
            reciever.RunWorkerAsync();
        }
        private void btn9_click(object sender, EventArgs e)
        {
            byte[] num = { 9 };
            socket.Send(num);
            button9.Content = player.ToString();
            reciever.RunWorkerAsync();
        }

        private void onClose(object sender, EventArgs e)
        {
            reciever.WorkerSupportsCancellation = true;
            reciever.CancelAsync();
            if (server != null)
                server.Stop();
        }
    }
}
