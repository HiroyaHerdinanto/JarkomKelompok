using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private bool isConnected = false;
        private string username = "";
        private CancellationTokenSource cts;
        private readonly int port = 8888; // Default port

        public Form1()
        {
            InitializeComponent();
            ToggleChatFunctionality(false);
            // Setup event handlers yang belum ada di designer
            connect_ip_btn.Click += Connect_ip_btn_Click;
            message_box.KeyPress += Message_box_KeyPress;
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load settings jika ada
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Optional: Load previous settings (username, IP)
            try
            {
                if (File.Exists("chat_settings.txt"))
                {
                    var lines = File.ReadAllLines("chat_settings.txt");
                    if (lines.Length > 0) set_username_box.Text = lines[0];
                    if (lines.Length > 1) connect_ip_box.Text = lines[1];
                }
            }
            catch { /* Ignore errors */ }
        }

        private void SaveSettings()
        {
            // Optional: Save settings for next time
            try
            {
                File.WriteAllLines("chat_settings.txt",
                    new[] { set_username_box.Text, connect_ip_box.Text });
            }
            catch { /* Ignore errors */ }
        }

        private void ToggleChatFunctionality(bool enable)
        {
            send_msg_btn.Enabled = enable;
            message_box.Enabled = enable;
            chat_list_box.Enabled = enable;
        }

        private async void Connect_ip_btn_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                DisconnectFromServer();
                return;
            }

            await ConnectToServer();
        }

        private async Task ConnectToServer()
        {
            try
            {
                string ipAddress = connect_ip_box.Text.Trim();
                username = set_username_box.Text.Trim();

                if (string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("Please enter a username");
                    return;
                }

                if (string.IsNullOrEmpty(ipAddress))
                {
                    MessageBox.Show("Please enter a server IP address");
                    return;
                }

                // Update UI
                connect_ip_btn.Enabled = false;
                SetStatus("Connecting...");

                // Create connection
                client = new TcpClient();
                cts = new CancellationTokenSource();

                // Use async connect with timeout
                var connectTask = client.ConnectAsync(ipAddress, port);
                var timeoutTask = Task.Delay(5000); // 5 second timeout

                var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                if (completedTask == timeoutTask)
                {
                    throw new Exception("Connection timeout");
                }

                await connectTask; // Ensure any exceptions are propagated

                stream = client.GetStream();
                isConnected = true;

                // Send login information
                byte[] data = Encoding.UTF8.GetBytes($"LOGIN|{username}");
                await stream.WriteAsync(data, 0, data.Length, cts.Token);

                // Start receiving messages
                _ = Task.Run(() => ReceiveData(cts.Token), cts.Token);

                // Update UI
                ToggleChatFunctionality(true);
                connect_ip_btn.Text = "Disconnect";
                connect_ip_btn.Enabled = true;
                set_username_box.Enabled = false;
                connect_ip_box.Enabled = false;
                SetStatus("Connected");

                SaveSettings(); // Save successful connection details
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}");
                SetStatus("Connection failed");
                connect_ip_btn.Enabled = true;
                DisconnectFromServer();
            }
        }

        private async void ReceiveData(CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[4096];

            while (isConnected && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    if (bytesRead == 0) break; // Connection closed

                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    ProcessReceivedData(data);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (isConnected)
                    {
                        Invoke(new Action(() =>
                        {
                            AddMessageToChat("System", $"Connection error: {ex.Message}");
                            DisconnectFromServer();
                        }));
                    }
                    break;
                }
            }
        }

        private void ProcessReceivedData(string data)
        {
            // Split by lines in case multiple messages arrived together
            string[] messages = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string message in messages)
            {
                string[] parts = message.Split('|');
                if (parts.Length < 2) continue;

                string command = parts[0];
                string content = parts[1];

                switch (command)
                {
                    case "MESSAGE":
                        if (parts.Length >= 3)
                        {
                            string sender = parts[1];
                            string msg = parts[2];
                            AddMessageToChat(sender, msg);
                        }
                        break;

                    case "USERLIST":
                        UpdateUserList(content);
                        break;

                    case "ERROR":
                        ShowError(content);
                        break;

                    case "PRIVATE":
                        if (parts.Length >= 4)
                        {
                            string from = parts[1];
                            string msg = parts[3];
                            AddPrivateMessageToChat(from, msg);
                        }
                        break;

                    case "SYSTEM":
                        AddSystemMessage(content);
                        break;
                }
            }
        }

        private void AddMessageToChat(string sender, string message)
        {
            if (chat_list_box.InvokeRequired)
            {
                chat_list_box.Invoke(new Action<string, string>(AddMessageToChat), sender, message);
            }
            else
            {
                chat_list_box.AppendText($"[{DateTime.Now:HH:mm}] {sender}: {message}\r\n");
                chat_list_box.ScrollToCaret();
            }
        }

        private void AddPrivateMessageToChat(string sender, string message)
        {
            if (chat_list_box.InvokeRequired)
            {
                chat_list_box.Invoke(new Action<string, string>(AddPrivateMessageToChat), sender, message);
            }
            else
            {
                chat_list_box.AppendText($"[{DateTime.Now:HH:mm}] [PM from {sender}]: {message}\r\n");
                chat_list_box.ScrollToCaret();
            }
        }

        private void AddSystemMessage(string message)
        {
            if (chat_list_box.InvokeRequired)
            {
                chat_list_box.Invoke(new Action<string>(AddSystemMessage), message);
            }
            else
            {
                chat_list_box.AppendText($"[{DateTime.Now:HH:mm}] System: {message}\r\n");
                chat_list_box.ScrollToCaret();
            }
        }

        private void UpdateUserList(string userList)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action<string>(UpdateUserList), userList);
            }
            else
            {
                listBox1.Items.Clear();
                string[] users = userList.Split(',');
                foreach (string user in users)
                {
                    if (!string.IsNullOrEmpty(user))
                    {
                        listBox1.Items.Add(user);
                    }
                }

                user_online.Text = $"Users online: {listBox1.Items.Count}";
            }
        }

        private void ShowError(string error)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowError), error);
            }
            else
            {
                MessageBox.Show($"Server error: {error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetStatus(string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SetStatus), status);
            }
            else
            {
                this.Text = $"Wangsaf - {status}";
            }
        }

        private async void SendMessage()
        {
            if (!isConnected || stream == null)
            {
                MessageBox.Show("Not connected to server");
                return;
            }

            string message = message_box.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            try
            {
                // Check for private message command
                string protocolMessage;
                if (message.StartsWith("/w "))
                {
                    // Format: /w username message
                    var parts = message.Split(new[] { ' ' }, 3);
                    if (parts.Length >= 3)
                    {
                        string targetUser = parts[1];
                        string privateMsg = parts[2];
                        protocolMessage = $"PRIVATE|{targetUser}|{privateMsg}";
                    }
                    else
                    {
                        MessageBox.Show("Invalid private message format. Use: /w username message");
                        return;
                    }
                }
                else
                {
                    protocolMessage = $"MESSAGE|{message}";
                }

                byte[] data = Encoding.UTF8.GetBytes(protocolMessage);
                await stream.WriteAsync(data, 0, data.Length);
                message_box.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
                DisconnectFromServer();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void Message_box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && isConnected)
            {
                SendMessage();
                e.Handled = true;
            }
        }

        private void DisconnectFromServer()
        {
            isConnected = false;
            cts?.Cancel();

            if (stream != null)
            {
                try
                {
                    // Send logout message
                    if (client != null && client.Connected)
                    {
                        byte[] data = Encoding.UTF8.GetBytes("LOGOUT|");
                        stream.Write(data, 0, data.Length);
                    }
                }
                catch { /* Ignore errors during disconnect */ }

                stream.Close();
                stream = null;
            }

            if (client != null)
            {
                client.Close();
                client = null;
            }

            // Update UI
            if (InvokeRequired)
            {
                Invoke(new Action(DisconnectFromServer));
                return;
            }

            ToggleChatFunctionality(false);
            connect_ip_btn.Text = "Connect";
            connect_ip_btn.Enabled = true;
            set_username_box.Enabled = true;
            connect_ip_box.Enabled = true;
            listBox1.Items.Clear();
            user_online.Text = "Users online: 0";
            SetStatus("Disconnected");
            AddSystemMessage("Disconnected from server");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isConnected)
            {
                DisconnectFromServer();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: Add functionality when selecting a user from the list
            // For example, focus message box and add /w command
            if (listBox1.SelectedItem != null)
            {
                string selectedUser = listBox1.SelectedItem.ToString();
                message_box.Text = $"/w {selectedUser} ";
                message_box.Focus();
                message_box.SelectionStart = message_box.Text.Length;
            }
        }

        // Other existing event handlers...
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
    }
}