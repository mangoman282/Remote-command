using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        Console.Write("Enter server IP: ");
        string Ip = Console.ReadLine();
        Console.Write("Enter port: ");
        int port = int.Parse(Console.ReadLine());

        TcpClient client = new TcpClient();
        client.Connect(Ip, port);

        NetworkStream stream = client.GetStream();
        while (true)
        {

            Console.Write("Command: ");
            string command = Console.ReadLine();

            byte[] data = Encoding.UTF8.GetBytes(command);
            stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);


            Console.WriteLine(response);

            if (command == "exit") break;
        }

        client.Close();
    }
}