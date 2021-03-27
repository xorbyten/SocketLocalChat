using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace WinFormClient
{
    class ClientClass
    {
        Socket serverSocket;
        public ClientClass()
        {
            
        }

        public void ConnectToServer(string serverIP, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIP), port);
            Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket = connectionSocket;

            try
            {
                connectionSocket.Connect(endPoint);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Server is not reponding. {e.Message}");
            }
        }

        public void SendMessage(object obj)
        {
            string msg = (string)obj;
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            int sendBytes = 0;
            sendBytes = serverSocket.Send(buffer);
        }

        public void ReceiveMessageFromServer()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int receiveBytes = 0;
                
                receiveBytes = serverSocket.Receive(buffer);
                StringBuilder stringBuilder = new StringBuilder(Encoding.UTF8.GetString(buffer));
                stringBuilder.ToString();
                Form1.form.SetTextStdrt(stringBuilder.ToString());
                stringBuilder.Clear();
            }
        }
    }
}
