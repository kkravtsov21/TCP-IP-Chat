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
using System.Threading;





namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // Заготавливаем TCP сокет
        static Socket serverSocket;
        Thread srv;
        void StartServer()
        {

            Socket request = null;  // сокет для установленного соединения
            try
            {
                serverSocket = new Socket(
                AddressFamily.InterNetwork,  // IPv4 адрессация
                SocketType.Stream,           // Потоковый (двусторонний) сокет
                ProtocolType.Tcp             // Transport control protocol
        );
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse(textBox1.Text), Convert.ToInt32(textBox2.Text)));  // привязываем сокет к пункту назначения
                serverSocket.Listen(100);  //слушаем порт, допускаем очередь из 100 запросов (101-й 
                                           // получит сообщение сервер занят)
                label4.Text = "ON";
                button1.Text = "Stop";
                byte[] buf = new byte[256];  // буфер для считаных данных
                string str;                  // строка для перевода байт в символы
                int n;                       // кол-во символов в буфере

                while (true)  // постоянно считываем данные с порта
                {
                    // ожидаем запрос и создаем сокет для соединения - обработки запроса
                    request = serverSocket.Accept();  // зависание потока до прихода запроса от клиента
                    str = "";
                    // начинаем прием данных
                    do
                    {
                        n = request.Receive(buf);  // получаем данные в байт-буфер
                        // переводим байты в символы и дописываем к строке
                        // чтобы не зачищать байт-буфер указываем кол-во принятых байт n
                        str += System.Text.Encoding.Unicode.GetString(buf, 0, n);
                    } while (request.Available > 0);  // пока есть доступные байты

                    // Выводим данные о полученном запросе
                    string receiveTime = DateTime.Now.ToString();
                    listBox1.Items.Add(receiveTime + " -  " + str);

                    // отправляем ответ
                    // готовим сообщение
                    string msg = "Cообщение успешно доставлено " + receiveTime;
                    // переводим в байты
                    buf = System.Text.Encoding.Unicode.GetBytes(msg);
                    request.Send(buf);

                    // закрываем сокет
                    request.Shutdown(SocketShutdown.Both);
                    request.Close();
                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Exception: " + ex.Message);
                label4.Text = "OFF";
                button1.Text = "Start";
                srv = null;
            }
        }



        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            if (srv == null)
            {
                srv = new Thread(StartServer);
                srv.Start();
               
                   
            }
            else
            {
                serverSocket?. Close();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Closing(object sender, FormClosingEventArgs e)
        {
            serverSocket?.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
      



       
}

//namespace Config
//{
//    public static class Ini
//    {
//        public static string
//            host = TextBox.;             // localhost - петля на локальную машину
//        public static IPAddress
//            IP = IPAddress.Parse(host);     // определение IP адресса
//        public static int
//            port = 8080;                    // порт - способ разделения программ на одном хосте
//        public static IPEndPoint
//            endPoint = new IPEndPoint(IP, port);  // Пункт назначения - хост : порт
//        public static System.Text.Encoding        // Транспортная кодировка
//            communicationEncoding = System.Text.Encoding.Unicode;

//    }
//}