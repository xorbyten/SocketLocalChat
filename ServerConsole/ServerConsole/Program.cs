using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.Json;

namespace ServerConsole
{
    class Program
    {
        /*static string serverIP = "127.0.0.1";
        static int port = 8888;*/
        static int counter = 0;
        static Socket listenerSocket = null;
        static Socket acceptedClient = null;
        static string clientData = null;
        static Hashtable clientList = new Hashtable();
        static void Main(string[] args)
        {
            var jsonString = File.ReadAllText(@"settings.json");
            AppSettings appSet = new AppSettings();
            appSet = JsonSerializer.Deserialize<AppSettings>(jsonString);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(appSet.serverIP), Int32.Parse(appSet.serverPort));
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listenerSocket.Bind(endPoint);
            Console.WriteLine($"Server {endPoint.Address} is started. Listening for connections...");
            listenerSocket.Listen(3);

            while (true)
            {
                acceptedClient = listenerSocket.Accept();
                counter += 1;

                byte[] buffer = new byte[1024];
                int bytesCount = 0;

                Console.WriteLine("Start to receive any messages...");
                bytesCount = acceptedClient.Receive(buffer);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(Encoding.UTF8.GetString(buffer));
                Console.WriteLine($"Сlient {stringBuilder.ToString()} with {acceptedClient.RemoteEndPoint} connected.");
                clientList.Add(stringBuilder.ToString(), acceptedClient);

                Client client = new Client();
                client.StartChat(acceptedClient, clientList, counter);
                client.BroadcastMessage($"{stringBuilder.ToString()} joined to chat room.");
                stringBuilder.Clear();
            }
            acceptedClient.Close();
            listenerSocket.Close();
            Console.WriteLine("Server shutdown...");
            Console.ReadLine();
        }
    }
}
