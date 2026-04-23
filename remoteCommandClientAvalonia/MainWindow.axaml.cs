using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace remoteCommandClientAvalonia
{
    public partial class MainWindow : Window
    {
        private TcpClient? _client;
        private NetworkStream? _stream;
        private bool _isConnected = false;

        public MainWindow()
        {
            InitializeComponent();
            PrintWelcome();
        }
        private async void BtnConnect_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string ip       = TxtServerIp.Text?.Trim() ?? "";
            string portText = TxtPort.Text?.Trim() ?? "";
            string username = TxtUsername.Text?.Trim() ?? "";
            string password = TxtPassword.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(ip))       { AppendError("IP cannot be empty."); return; }
            if (!int.TryParse(portText, out int port)) { AppendError("Invalid port number."); return; }
            if (string.IsNullOrWhiteSpace(username))  { AppendError("Username cannot be empty."); return; }
            if (string.IsNullOrWhiteSpace(password))  { AppendError("Password cannot be empty."); return; }

            AppendInfo($"Connecting to {ip}:{port}...");
            SetControlsForConnecting();

            try
            {
                string authResult = "";

                await Task.Run(() =>
                {
                    _client = new TcpClient();
                    _client.ConnectAsync(ip, port).Wait(5000);
                    _stream = _client.GetStream();


                    string challenge = Receive(_stream);

                    if (challenge == "AUTH_REQUIRED")
                    {

                        Send(_stream, $"{username}|{password}");
                        authResult = Receive(_stream);
                    }
                    else
                    {
                        authResult = challenge;
                    }
                });

                if (authResult == "AUTH_OK")
                {
                    _isConnected = true;
                    AppendSuccess("Authentication successful!");
                    AppendInfo($"Connected to {ip}:{port} — type a command below.\n");
                    SetControlsConnected();
                }
                else
                {
                    AppendError($"Authentication failed: {authResult}");
                    _stream?.Close(); _client?.Close();
                    SetControlsDisconnected();
                }
            }
            catch (Exception ex)
            {
                AppendError($"Connection failed: {ex.Message}");
                AppendInfo("Make sure the server is running and IP/port are correct.");
                SetControlsDisconnected();
            }
        }


        private void BtnDisconnect_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Disconnect("User disconnected.");
        }


        private async void BtnSend_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
            => await SendCommandAsync();

        private async void TxtCommand_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                await SendCommandAsync();
        }

        private void QuickCmd_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string cmd)
            {
                TxtCommand.Text = cmd;
                if (_isConnected)
                    _ = SendCommandAsync();
            }
        }

        private async Task SendCommandAsync()
        {
            if (!_isConnected || _stream == null) return;

            string command = TxtCommand.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(command))
            {
                AppendError("Command cannot be empty.");
                return;
            }

            TxtCommand.Text = "";
            AppendPrompt(command);
            BtnSend.IsEnabled = false;
            TxtCommand.IsEnabled = false;

            try
            {
                string response = "";
                long elapsed = 0;

                await Task.Run(() =>
                {
                    var sw = Stopwatch.StartNew();
                    Send(_stream!, command);
                    response = Receive(_stream!);
                    sw.Stop();
                    elapsed = sw.ElapsedMilliseconds;
                });

                if (string.IsNullOrEmpty(response))
                {
                    Disconnect("Server closed the connection.");
                    return;
                }

                AppendResult(response, elapsed);

                if (command.Trim().ToLower() == "exit")
                {
                    Disconnect("Session ended by exit command.");
                }
            }
            catch (Exception ex)
            {
                AppendError($"Error: {ex.Message}");
                Disconnect("Connection lost.");
            }
            finally
            {
                if (_isConnected)
                {
                    BtnSend.IsEnabled = true;
                    TxtCommand.IsEnabled = true;
                    TxtCommand.Focus();
                }
            }
        }

        private void Disconnect(string reason)
        {
            try { _stream?.Close(); _client?.Close(); } catch { }
            _stream = null; _client = null; _isConnected = false;
            AppendInfo(reason);
            SetControlsDisconnected();
        }

        private static void Send(NetworkStream stream, string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }

        private static string Receive(NetworkStream stream)
        {
            byte[] buf = new byte[65536];
            int n = stream.Read(buf, 0, buf.Length);
            return n == 0 ? "" : Encoding.UTF8.GetString(buf, 0, n);
        }


        private void SetControlsForConnecting()
        {
            BtnConnect.IsEnabled    = false;
            BtnDisconnect.IsEnabled = false;
            TxtServerIp.IsEnabled   = false;
            TxtPort.IsEnabled       = false;
            TxtUsername.IsEnabled   = false;
            TxtPassword.IsEnabled   = false;
            SetStatus("Connecting...", "#f0883e");
        }

        private void SetControlsConnected()
        {
            BtnDisconnect.IsEnabled = true;
            BtnSend.IsEnabled       = true;
            TxtCommand.IsEnabled    = true;
            TxtCommand.Focus();
            SetStatus("Connected", "#3fb950");
        }

        private void SetControlsDisconnected()
        {
            BtnConnect.IsEnabled    = true;
            BtnDisconnect.IsEnabled = false;
            BtnSend.IsEnabled       = false;
            TxtCommand.IsEnabled    = false;
            TxtServerIp.IsEnabled   = true;
            TxtPort.IsEnabled       = true;
            TxtUsername.IsEnabled   = true;
            TxtPassword.IsEnabled   = true;
            SetStatus("Disconnected", "#f85149");
        }

        private void SetStatus(string text, string hexColor)
        {
            TxtStatus.Text       = text;
            TxtStatus.Foreground = SolidColorBrush.Parse(hexColor);
            StatusDot.Fill       = SolidColorBrush.Parse(hexColor);
        }


        private void PrintWelcome()
        {
            TxtOutput.Text =
                "╔══════════════════════════════════════════╗\n" +
                "║        REMOTE COMMAND CLIENT             ║\n" +
                "║        TCP Socket — Avalonia UI          ║\n" +
                "╚══════════════════════════════════════════╝\n\n" +
                "Enter server IP, port, and credentials, then click CONNECT.\n\n";
        }

        private void AppendPrompt(string cmd)
        {
            TxtOutput.Text += $"❯ {cmd}\n";
            ScrollBottom();
        }

        private void AppendResult(string result, long ms)
        {
            TxtOutput.Text += result.TrimEnd() + "\n";
            TxtOutput.Text += $"\n── {ms} ms ──────────────────────────────────\n\n";
            ScrollBottom();
        }

        private void AppendInfo(string msg)
        {
            TxtOutput.Text += $"[INFO]  {msg}\n";
            ScrollBottom();
        }

        private void AppendSuccess(string msg)
        {
            TxtOutput.Text += $"[OK]    {msg}\n";
            ScrollBottom();
        }

        private void AppendError(string msg)
        {
            TxtOutput.Text += $"[ERROR] {msg}\n";
            ScrollBottom();
        }

        private void ScrollBottom()
            => OutputScroll.ScrollToEnd();

        private void BtnClear_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
            => TxtOutput.Text = "";
    }
}
