using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace ChatServer
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();

        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(8888);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine("Chat Server Started ....");
            counter = 0;
            while ((true))
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                string clientName = readDataAsString(clientSocket);

                if (clientsList.ContainsKey(clientName))
                {
                    broadcast("The username already exist", clientSocket);
                }
                else
                {                    
                    clientsList.Add(clientName, clientSocket); //add to hash table - data received , the specific client socket                    
                    broadcastAll("--- " + clientName + " Joined chat room ---");
                    Console.WriteLine(clientName + " Joined chat room ");
                    //start new thread
                    HandleClient client = new HandleClient();
                    client.startClient(clientSocket, clientName, clientsList);
                }
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine("exit");
            Console.ReadLine();
        }

        private static void broadcast(string msg, TcpClient broadcastSocket)
        {
            if (broadcastSocket.Connected)
            {
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;

                broadcastBytes = Encoding.ASCII.GetBytes(msg);

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }



        public static void broadcastAll(string msg)
        {
            List<string> tbdClient = new List<string>();
            //foreach dictionary in the hashtable...
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;

                if (broadcastSocket.Connected)
                {
                    NetworkStream broadcastStream = broadcastSocket.GetStream();
                    Byte[] broadcastBytes = null;

                    broadcastBytes = Encoding.ASCII.GetBytes(msg);

                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                    broadcastStream.Flush();
                }
                else
                {
                    tbdClient.Add((string)Item.Key);
                }

                //delete disconnected client socket from hash table
                foreach(string key in tbdClient)                
                    clientsList.Remove(key);                

            }
        } 


        public static string readDataAsString(TcpClient clientSocket)
        {
            byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
            string dataFromClient = null;            
            NetworkStream networkStream = clientSocket.GetStream();
            networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);            
            dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom).TrimEnd('\0'); //turn byte data to string
            //dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
            return dataFromClient;
        }


    }
    
}