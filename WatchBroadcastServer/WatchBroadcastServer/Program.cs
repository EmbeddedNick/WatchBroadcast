using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WatchBroadcastServer
{
    internal class Program
    {
        static bool g_bServerIsWork = true;
        static void Main(string[] args)
        {
            Socket serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            serverSock.Bind(new IPEndPoint(IPAddress.Parse("192.168.2.115"), 44112));
            serverSock.Listen(150);


            while (g_bServerIsWork) 
            {
                var clientSocket = serverSock.Accept();
                if (clientSocket != null)
                {
                    new Thread((object obj) =>
                    {
                        Socket socket = obj as Socket;
                        byte[] buf = new byte[1024];
                        byte[] answer = new byte[3];
                        answer[0] = (byte)DateTime.Now.Hour;
                        answer[1] = (byte)DateTime.Now.Minute;
                        answer[2] = (byte)DateTime.Now.Second;

                        while (g_bServerIsWork)
                        {
                            socket.Receive(buf);
                            // working with buf ...
                            socket.Send(answer);
                        }
                    }).Start(clientSocket);
                }
            }
        }
    }
}
