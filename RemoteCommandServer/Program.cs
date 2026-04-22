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
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);

        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Received: " + message);

        string response = "Server received: " + message;
        byte[] data = Encoding.UTF8.GetBytes(response);

        stream.Write(data, 0, data.Length);

        Console.ReadLine();
    }
}