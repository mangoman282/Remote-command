
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Threading;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> 4a38c3c (Big udate Server to handle multiple Clients, Add login, Block dangerous commands)

class Server
{
    private static readonly string validUsername = "admin";
    private static readonly string validPassword = "1234";
<<<<<<< HEAD
    
=======

    private static readonly HashSet<string> AllowedCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "dir", "ipconfig", "tasklist", "whoami", "hostname",
        "netstat", "echo", "date", "time", "ver", "exit"
    };

    private static readonly HashSet<string> BlockedCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "del", "format", "shutdown", "powershell", "cmd",
        "reg", "netsh", "rd", "rmdir"
    };

>>>>>>> 4a38c3c (Big udate Server to handle multiple Clients, Add login, Block dangerous commands)
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 8080);
        listener.Start();

        Console.WriteLine("Server is listening...");
        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected!");

            Thread t = new Thread(() => HandleClient(client));
            t.IsBackground = true;
            t.Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        try
        {
            Send(stream, "AUTH_REQUIRED");
<<<<<<< HEAD
=======

>>>>>>> 4a38c3c (Big udate Server to handle multiple Clients, Add login, Block dangerous commands)
            string? authLine = Receive(stream);
            if (authLine == null)
            {
                Send(stream, "AUTH_FAIL");
                return;
            }

            string[] parts = authLine.Split('|');

<<<<<<< HEAD
            if (parts.Length != 2) ||
               parts[0].Trim() != validUsername ||
               parts[1].Trim() != validPassword)
               {
                Send(stream, "AUTH_FAIL");
                return;
            }

            Send(stream, "AUTH_OK");

            while (true)
            {
                string? command = Receive(stream);
                if (command == null) break;

                command = command.Trim();
                Console.WriteLine("Command: " + command);

                if (command.ToLower() == "exit")
                {
                    Send(stream, "Disconnected from server.");
                    break;
                }

=======
            if (parts.Length != 2 ||
                parts[0].Trim() != validUsername ||
                parts[1].Trim() != validPassword)
            {
                Send(stream, "AUTH_FAIL");
                return;
            }

            Send(stream, "AUTH_OK");

            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                    break;

                string command = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine("Command: " + command);

                if (command.ToLower() == "exit")
                {
                    string goodbye = "Disconnecred from server.";
                    byte[] exitData = Encoding.UTF8.GetBytes(goodbye);
                    stream.Write(exitData, 0, exitData.Length);
                    break;
                }
                string cmdName = command.Split(' ')[0];

                if (BlockedCommands.Contains(cmdName))
                {
                    string blockedMsg = "[BLOCKED] This command is not allowed.";
                    byte[] blockedData = Encoding.UTF8.GetBytes(blockedMsg);
                    stream.Write(blockedData, 0, blockedData.Length);
                    continue;
                }

                if (!AllowedCommands.Contains(cmdName))
                {
                    string rejectedMsg = "[REJECTED] Command not in allowed list.";
                    byte[] rejectedData = Encoding.UTF8.GetBytes(rejectedMsg);
                    stream.Write(rejectedData, 0, rejectedData.Length);
                    continue;
                }

>>>>>>> 4a38c3c (Big udate Server to handle multiple Clients, Add login, Block dangerous commands)
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                Process process = new Process();
                process.StartInfo = psi;
                process.Start();
<<<<<<< HEAD
=======

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                string result = string.IsNullOrEmpty(output) ? error : output;
                if (string.IsNullOrEmpty(result))
                    result = "Command executed with no output.";

                byte[] data = Encoding.UTF8.GetBytes(result);
                stream.Write(data, 0, data.Length);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            stream.Close();
            client.Close();
>>>>>>> 4a38c3c (Big udate Server to handle multiple Clients, Add login, Block dangerous commands)

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                string result = string.IsNullOrEmpty(output) ? error : output;
                if (string.IsNullOrEmpty(result))
                    result = "Command executed with no output.";
                Send(stream, result);

            }
        }
<<<<<<< HEAD
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            stream.Close();
            client.Close();

        } 
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

=======
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
        if (bytesRead == 0)
            return null;

        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }
}
>>>>>>> 4a38c3c (Big udate Server to handle multiple Clients, Add login, Block dangerous commands)
