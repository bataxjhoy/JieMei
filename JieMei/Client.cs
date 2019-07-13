using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;//通信
using System.Net;//通信
using System.Threading;
namespace JieMei
{
    /// <summary>
    /// 用来跟客户端做通信
    /// </summary>
    class Client
    {

        private Socket clientsocket;
        private byte[] data = new byte[1024];//数据容器
        public Client(Socket s)
        {
            clientsocket = s;
        }
        public string Receivemessage(string message)
        {           
            int lenght = clientsocket.Receive(data);
            return message = Encoding.UTF8.GetString(data, 0, lenght);//接收到数据
        }


        public void sendmessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientsocket.Send(data);
        }
        public bool connected//判断是否连接
        {
            get { return clientsocket.Connected; }
        }
        public void close() 
        {
            clientsocket.Close();
        }
    }
}
