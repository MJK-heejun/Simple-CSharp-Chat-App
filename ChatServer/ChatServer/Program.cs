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
                string clientName = SocketHelper.ReadDataAsString(clientSocket);

                if (SocketHelper.clientsList.ContainsKey(clientName))
                {
                    SocketHelper.BroadcastString("The username already exist", clientSocket);
                }
                else
                {
                    SocketHelper.clientsList.Add(clientName, clientSocket); //add to hash table - data received , the specific client socket                    
                    SocketHelper.BroadcastStringAll("--- " + clientName + " Joined chat room ---");
                    Console.WriteLine(clientName + " Joined chat room ");
                    //start new thread
                    HandleClient client = new HandleClient();
                    client.startClient(clientSocket, clientName);
                }
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine("exit");
            Console.ReadLine();
        }
       

    }
    
}