using System;
using System.Net;
using System.Net.Sockets;

class Server
{
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 8080);
        listener.Start();

        Console.WriteLine("Server is listening on port 8080...");

        TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Client connected!");

        Console.ReadLine();
    }
}