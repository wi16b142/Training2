using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Training2.Models
{
    internal class ClientHandler
    {
        public Socket clientSocket { get; set; }
        public string Username { get => username; set => username = value; }

        private Action<Socket, string> NewMsg;
        byte[] buffer = new byte[512];
        Thread receivingThread;
        private string username;
        const string endMsg = "@quit";

        public ClientHandler(Socket accept, Action<Socket, string> NewMsg)
        {
            this.clientSocket = accept;
            this.NewMsg = NewMsg;
            StartReceiving();
        }

        private void StartReceiving()
        {
            receivingThread = new Thread(Receive);
            receivingThread.IsBackground = true;
            receivingThread.Start();
        }

        private void Receive()
        {
            string msg = "";
            string newMsg = "";
            while (!msg.Equals(endMsg))
            {
                int length = clientSocket.Receive(buffer);
                msg = Encoding.UTF8.GetString(buffer, 0, length);

                if(Username == null)
                {
                    Username = msg;
                    newMsg = Username;
                }
                else
                {
                    newMsg = Username + ": " + msg;
                }

                NewMsg(clientSocket, newMsg);
            }
            Close();
        }

        public void Close()
        {
            Send(Username+": "+endMsg);
            clientSocket.Close(1);
            receivingThread.Abort();      
        }

        public void Send(string newMsg)
        {
            if(clientSocket.Connected) clientSocket.Send(Encoding.UTF8.GetBytes(newMsg+"\r\n"));
        }
    }
}
