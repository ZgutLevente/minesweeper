using System.Net.Sockets;
using System.Net;
using System.Text;

namespace minesweeper
{
    internal class Communicator
    {
        private IPEndPoint endpoint;
        public Communicator() => endpoint = new IPEndPoint(IPAddress.Parse("130.162.215.178"), 443);
        public string Request(string senddata)
        {
            try
            {
                Socket sender = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(endpoint);
                sender.Send(Encoding.UTF8.GetBytes($"Newß{senddata}"));
                byte[] messageReceived = new byte[1024];
                int byteRecv = sender.Receive(messageReceived);
                sender.Close();
                return Encoding.UTF8.GetString(messageReceived, 0, byteRecv);
            }
            catch (Exception) { }
            return string.Empty;
        }
    }
}
