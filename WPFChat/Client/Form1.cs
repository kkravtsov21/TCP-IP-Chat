using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void Send()
        {
            // (сервер должен быть запущен и слушать порт)

            Socket clientSocket = null;// переменная для цикла сообщений

            byte[] buf = new byte[256]; // буфер для обмена
            string str = "";//перевод буфера в строку

            // Начинаем писать сообщения
            string msg = String.Empty;
            
            // запрашивает сообщение в консоли
           
            msg =textBox3+" : "+ textBox4;

            
                // переводим сообщение в байты 
                buf = System.Text.Encoding.Unicode.GetBytes(msg);
                try
                {// подключаемся к пункту назначения - код как у сервера
                    clientSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);
                    // соединяемся...             
                    clientSocket.Connect(new IPEndPoint(IPAddress.Parse(textBox1.Text), Convert.ToInt32(textBox2.Text)));
                    // отправляем сообщение
                    clientSocket.Send(buf);
                    // получаем ответ и переводим в строку
                    str = "";
                    do
                    {
                        clientSocket.Receive(buf);
                        str += System.Text.Encoding.Unicode.GetString(buf);
                    } while (clientSocket.Available > 0);
                    // закрываем сокет
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
                catch (Exception ex) { MessageBox.Show("Exception: " + ex.Message); return; }

                //Выводим ответ сервера
                listBox1.Items.Add("SRV> " + str);


                //Ожидаем новое сообщение
                msg = textBox3 + " : " + textBox4;
            
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Send();
        }
    }
}
