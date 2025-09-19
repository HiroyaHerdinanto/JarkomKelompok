//Hiroya Herdinanto
//5223600022
//GT11-A
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        private ArrayList alSockets;
        private Socket serverSocket;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;//Untuk mengatur apa yang mau dikerjain pas form selesai dibuka pertama kali
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string localIP = "127.0.0.1";
            lblStatus.Text = "My IP address is " + localIP;

            alSockets = new ArrayList();

            StartServer();
        }

        private void StartServer()//Untuk insialisasi server
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, 2222));
                serverSocket.Listen(10);
                serverSocket.BeginAccept(AcceptCallback, null);

                lbConnections.Invoke((MethodInvoker)(() =>
                    lbConnections.Items.Add("Server listening on 127.0.0.1:2222")));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server error: " + ex.Message);
            }
        }

        private void AcceptCallback(IAsyncResult AR)//Untuk terima koneksi dari client
        {
            try
            {
                Socket handlerSocket = serverSocket.EndAccept(AR);
                alSockets.Add(handlerSocket);

                lbConnections.Invoke((MethodInvoker)(() =>
                    lbConnections.Items.Add(handlerSocket.RemoteEndPoint.ToString() + " connected.")));

                // Mulai thread handler seperti sebelumnya
                Thread thdHandler = new Thread(new ParameterizedThreadStart(handlerThread));
                thdHandler.IsBackground = true;
                thdHandler.Start(handlerSocket);

                // Terus listen client baru
                serverSocket.BeginAccept(AcceptCallback, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Accept error: " + ex.Message);
            }
        }

        
        public void handlerThread(object clientSocket)//Untuk Handler menerima file
        {
            Socket handlerSocket = (Socket)clientSocket;
            using (NetworkStream networkStream = new NetworkStream(handlerSocket))
            {
                try
                {
                    
                    byte[] lengthBytes = new byte[4];
                    int readLen = networkStream.Read(lengthBytes, 0, 4);
                    if (readLen < 4) return;

                    int fileLength = BitConverter.ToInt32(lengthBytes, 0);

                    // Terima isi file
                    byte[] buffer = new byte[1024];
                    int totalRead = 0;

                    string savePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "received_" + DateTime.Now.Ticks + ".dat"
                    );

                    using (FileStream fileStream = File.OpenWrite(savePath))
                    {
                        while (totalRead < fileLength)
                        {
                            int toRead = Math.Min(buffer.Length, fileLength - totalRead);
                            int read = networkStream.Read(buffer, 0, toRead);
                            if (read == 0) break;

                            fileStream.Write(buffer, 0, read);
                            totalRead += read;
                        }
                    }

                    lbConnections.Invoke((MethodInvoker)(() =>
                        lbConnections.Items.Add("File diterima, disimpan di: " + savePath)));
                }
                catch (Exception ex)
                {
                    lbConnections.Invoke((MethodInvoker)(() =>
                        lbConnections.Items.Add("Handler error: " + ex.Message)));
                }
            }
        }
    }
}
