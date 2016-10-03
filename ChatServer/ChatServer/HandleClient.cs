using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections;

namespace ChatServer
{
    public class HandleClient
    {
        TcpClient clientSocket;
        string clNo;

        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            //start new thread for the client 'clineNo'
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()
        {
            int requestCount = 0;
            string dataFromClient = null;
            string rCount = null;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    dataFromClient = SocketHelper.ReadDataAsString(clientSocket, "$$");
                    Console.WriteLine("From client - " + clNo + " : " + dataFromClient);
                    rCount = Convert.ToString(requestCount);

                    SocketHelper.BroadcastStringAll(clNo + ">> " + dataFromClient);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    break;
                }
            }
        }
    } 


}