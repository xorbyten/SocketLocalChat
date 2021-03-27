using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ServerConsole
{
    class Client
    {
        Socket clientSocket;
        Hashtable clientList;
        int clientNo;

        public void StartChat(Socket _clientSocket, Hashtable _hash, int _clientNo)
        {
            this.clientSocket = _clientSocket;
            this.clientList = _hash;
            this.clientNo = _clientNo;
            Thread thread1 = new Thread(DoChat);
            thread1.Start();
        }
        public void DoChat()
        {
            while (true)
            {   // Receive message from client
                byte[] buffer = new byte[1024];
                try
                {
                    clientSocket.Receive(buffer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(Encoding.UTF8.GetString(buffer));
                foreach (DictionaryEntry entry in clientList)
                {
                    if (clientSocket == (Socket)entry.Value)
                    {
                        Console.WriteLine($"{entry.Key} says {stringBuilder.ToString()}");
                    }
                }
                /*if(stringBuilder.ToString().Contains("&cls_cmd"))
                {
                    foreach(DictionaryEntry entry in clientList)
                    {
                        if(clientSocket == (Socket)entry.Value)
                        {
                            clientSocket.Close();
                            clientList.Remove((Socket)entry.Value);
                        }
                    }
                }*/
                BroadcastMessage(stringBuilder.ToString().Trim('\0'));
            }
        }

        public void BroadcastMessage(string message)
        {
            foreach (DictionaryEntry entry in clientList)
            {
                Socket broadcastSocket;
                broadcastSocket = (Socket)entry.Value;
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                try
                {
                    broadcastSocket.Send(buffer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine($"Broadcast to {entry.Key}");
            }
        }
    }
}
