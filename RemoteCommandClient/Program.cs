using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        Console.Write("Enter server IP: ");
        string ip = Console.ReadLine() ?? "127.0.0.1";
        Console.Write("Enter port: ");
        int port = int.Parse(Console.ReadLine()?? "8080");

        TcpClient client = new TcpClient();
        client.Connect(ip, port);

        NetworkStream stream = client.GetStream();
        string? challenge = Receive(stream);
        
        if (challenge == "AUTH_REQUIRED")
        {
            Console.Write("Username: ");
            string username = Console.ReadLine()?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine()?? "";

            Send(stream, username + "|" + password);

            string? authResult = Receive(stream);

            if (authResult != "AUTH_OK")
            {
                Console.WriteLine("Authentication failed!");
                client.Close();
                return;
            }

            Console.WriteLine("Authentication successful!");
        }
        else
        {
            Console.WriteLine("Server did not request authentication.");
            client.Close();
            return;
        }
        while (true)
        {

            Console.Write("Command: ");
            string command = Console.ReadLine()?? "";
            Send(stream, command);
            string? response = Receive(stream);
            if (response == null)
            {
                Console.WriteLine("Server disconnected.");
                break;
            }

            Console.WriteLine(response);

            if (command.ToLower == "exit"){
                Console.WriteLine("Exiting...");
                break;
        }

        client.Close();
    }
    static void Send(NetworkStream stream, string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    static string? Receive(NetworkStream stream)
    {
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        if (bytesRead == 0) {
            return null;
        }
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }
}