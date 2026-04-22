using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        TcpClient client = new TcpClient();
        client.Connect("127.0.0.1", 8080);

        NetworkStream stream = client.GetStream();

        Console.Write("Command: ");
        string cmd = Console.ReadLine();

        byte[] data = Encoding.UTF8.GetBytes(cmd);
        stream.Write(data, 0, data.Length);

        byte[] buffer = new byte[8192];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);

        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine(response);

        Console.ReadLine();
    }
}