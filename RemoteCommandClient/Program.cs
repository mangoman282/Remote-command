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
        int port = int.Parse(Console.ReadLine() ?? "8080");

        TcpClient client = new TcpClient();
        client.Connect(ip, port);

        Console.WriteLine("[INFO] Connected to server.");

        NetworkStream stream = client.GetStream();
        string? challenge = Receive(stream);

        if (challenge == "AUTH_REQUIRED")
        {
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            Send(stream, username + "|" + password);

            string? authResult = Receive(stream);

            if (authResult != "AUTH_OK")
            {
                Console.WriteLine("[ERROR] Authentication failed!");
                client.Close();
                return;
            }

            Console.WriteLine("[OK]Authentication successful!");
        }
        else
        {
            Console.WriteLine("[ERROR] Server did not request authentication.");
            client.Close();
            return;
        }
        while (true)
        {

            Console.Write("Command: ");
            string command = Console.ReadLine() ?? "";
<<<<<<< HEAD
=======

            Console.WriteLine("[INFO] Sending command: " + command);

>>>>>>> 8c672f8 (Final Update Client)
            Send(stream, command);
            string? response = Receive(stream);
            if (response == null)
            {
                Console.WriteLine("[ERROR] Server disconnected.");
                break;
            }

            Console.WriteLine(response);
            if (response.StartsWith("[BLOCKED]"))
            {
                Console.WriteLine("[SECURITY] " + response);
            }
            else if (response.StartsWith("[REJECTED]"))
            {
                Console.WriteLine("[SECURITY] " + response);
            }
            else
            {
                Console.WriteLine("[RESULT]");
                Console.WriteLine(response);
            }

<<<<<<< HEAD
            if (command.ToLower == "exit") {
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
=======
            if (command.ToLower() == "exit")
            {
                Console.WriteLine("[INFO] Exiting...");
                break;
            }

        }
        client.Close();
        Console.WriteLine("[INFO] Client closed.");
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
>>>>>>> 8c672f8 (Final Update Client)
    }