using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Training2.Models
{
    class Server
    {
        Socket serverSocket;
        byte[] buffer = new byte[512];
        List<ClientHandler> clients = new List<ClientHandler>();
        Thread acceptingThread;
        Action<Entry> guiUpdate;

        public Server(string ip, int port, Action<Entry> guiUpdate)
        {
            this.guiUpdate = guiUpdate;

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            serverSocket.Listen(10);
            StartAccepting();
        }

        private void StartAccepting()
        {
            acceptingThread = new Thread(Accept);
            acceptingThread.IsBackground = true;
            acceptingThread.Start();
        }

        private void Accept()
        {
            while(acceptingThread.IsAlive) clients.Add(new ClientHandler(serverSocket.Accept(), NewMsg));  
        }

        private void NewMsg(Socket sender, string newMsg)
        {
            if (!newMsg.Contains(':'))
            {
                newMsg += ": @joined";
            }

            string[] msg = newMsg.Split(':');

            foreach (var client in clients)
            {
                if (!client.clientSocket.Equals(sender))
                {
                    client.Send(msg[0].Trim(' ')+": "+msg[1].Trim(' '));
                }else client.Send("You: " + msg[1].Trim(' '));
            }

            guiUpdate(new Entry(msg[0].Trim(' '), msg[1].Trim(' ')));
        }
    }
}
