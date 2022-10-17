using System.Net.Sockets;
using System.Net;
using OnlineTicTacToe;


Console.ForegroundColor = ConsoleColor.Green;
var server = new MainServer();
server.Server();

class MainServer
{
    public void Server()
    {
        Console.Write("Enter an ip to listen: ");
        string? ip = Console.ReadLine();

        Console.Write("Enter a port to listen: ");
        int port = Convert.ToInt32(Console.ReadLine());

        // initiate the server
        var ipParsed = IPAddress.Parse(ip);
        var listener = new TcpListener(ipParsed, port);
        listener.Start();

        Console.WriteLine("listening...");

        var client = listener.AcceptTcpClient();
        var netStream = client.GetStream();

        // accept the client
        Console.WriteLine($"> new connection {client.Client.RemoteEndPoint}\n");

        while (true)
        {
            // start the game server
            GameServer.Engine(netStream);
            Console.Clear();
        }
    }
}