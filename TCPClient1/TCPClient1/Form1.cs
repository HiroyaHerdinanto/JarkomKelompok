//Hiroya Herdinanto
//5223600022
//GT11-A
using System;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;

namespace TCPClient1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = textBox1.Text;

                if (!File.Exists(filePath))
                {
                    MessageBox.Show("File tidak ditemukan!");
                    return;
                }

                byte[] fileBytes = File.ReadAllBytes(filePath);

                // Koneksi ke server localhost
                using (TcpClient clientSocket = new TcpClient())
                {
                    clientSocket.Connect("127.0.0.1", 2222); 
                    using (NetworkStream networkStream = clientSocket.GetStream())
                    {
                        if (!networkStream.CanWrite)
                        {
                            MessageBox.Show("NetworkStream tidak bisa menulis data!");
                            return;
                        }

                        // 1. Kirim panjang file (4 byte)
                        byte[] lengthBytes = BitConverter.GetBytes(fileBytes.Length);
                        networkStream.Write(lengthBytes, 0, lengthBytes.Length);
                        networkStream.Flush();

                        // 2. Kirim isi file
                        networkStream.Write(fileBytes, 0, fileBytes.Length);
                        networkStream.Flush();
                    }
                }

                MessageBox.Show("File berhasil dikirim ke server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal connect: " + ex.Message);
            }
        }

        // Kode untuk mencari file
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
            }
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e) { }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}
