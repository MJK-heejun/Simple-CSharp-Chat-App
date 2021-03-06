﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public static class SocketHelper
    {

        public static Hashtable clientsList = new Hashtable();

        public static void BroadcastString(string msg, TcpClient broadcastSocket)
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



        public static void BroadcastStringAll(string msg)
        {
            List<string> tbdClient = new List<string>();
            Hashtable tmpNewList = new Hashtable(clientsList);

            //foreach dictionary in the hashtable...
            foreach (DictionaryEntry Item in tmpNewList)
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
                    Console.WriteLine(Item.Key + " disconnection detected");
                    tbdClient.Add((string)Item.Key);                    
                }

                //delete disconnected client socket from hash table
                foreach (string key in tbdClient)
                    clientsList.Remove(key);
            }
        }


        public static string ReadDataAsString(TcpClient clientSocket, string delimeter = null)
        {
            byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
            string dataFromClient = null;
            NetworkStream networkStream = clientSocket.GetStream();
            networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
            dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom).TrimEnd('\0'); //turn byte data to string. trim the null data
            if(!string.IsNullOrEmpty(delimeter))
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf(delimeter));            
            return dataFromClient;
        }

    }
}
