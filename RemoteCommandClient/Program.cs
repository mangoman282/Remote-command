using System;
using System.Net.Sockets;

class Client
{
    static void Main()
    {
        TcpClient client = new TcpClient();
        client.Connect("127.0.0.1", 8080);

        Console.WriteLine("Connected to server!");

        Console.ReadLine();
    }
}