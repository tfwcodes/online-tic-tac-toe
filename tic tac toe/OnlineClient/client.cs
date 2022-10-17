using System.Net.Sockets;
using ClientTicTacToe;

Console.ForegroundColor = ConsoleColor.Green;
var client = new Client();
client.MainClient();

class Client
{
    public void MainClient()
    {
        Console.Write("Enter an ip to connect: ");
        string? ip = Console.ReadLine();

        Console.Write("Enter a port to connect: ");
        int port = Convert.ToInt32(Console.ReadLine());

        var tcpClient = new TcpClient(); // initiate a tcp client
        tcpClient.Connect(ip, port); // connect 
        var stream = tcpClient.GetStream(); 


        while (true)
        {
            // start the game client
            GameClient.Engine(stream);
            Console.Clear();
        }
    }
}