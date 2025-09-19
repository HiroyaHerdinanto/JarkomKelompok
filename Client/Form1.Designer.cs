namespace Client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.send_msg_btn = new System.Windows.Forms.Button();
            this.message_box = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.connect_ip_box = new System.Windows.Forms.TextBox();
            this.connect_ip_btn = new System.Windows.Forms.Button();
            this.uname_lbl = new System.Windows.Forms.Label();
            this.user_online = new System.Windows.Forms.Label();
            this.set_username_box = new System.Windows.Forms.TextBox();
            this.chat_list_box = new System.Windows.Forms.TextBox();
            this.message_lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // send_msg_btn
            // 
            this.send_msg_btn.Location = new System.Drawing.Point(435, 415);
            this.send_msg_btn.Name = "send_msg_btn";
            this.send_msg_btn.Size = new System.Drawing.Size(75, 23);
            this.send_msg_btn.TabIndex = 0;
            this.send_msg_btn.Text = "Send";
            this.send_msg_btn.UseVisualStyleBackColor = true;
            this.send_msg_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // message_box
            // 
            this.message_box.Location = new System.Drawing.Point(59, 418);
            this.message_box.Name = "message_box";
            this.message_box.Size = new System.Drawing.Size(370, 20);
            this.message_box.TabIndex = 1;
            this.message_box.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(522, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 64);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(155, 342);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // connect_ip_box
            // 
            this.connect_ip_box.Location = new System.Drawing.Point(316, 3);
            this.connect_ip_box.Name = "connect_ip_box";
            this.connect_ip_box.Size = new System.Drawing.Size(113, 20);
            this.connect_ip_box.TabIndex = 4;
            this.connect_ip_box.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // connect_ip_btn
            // 
            this.connect_ip_btn.Location = new System.Drawing.Point(435, 1);
            this.connect_ip_btn.Name = "connect_ip_btn";
            this.connect_ip_btn.Size = new System.Drawing.Size(75, 23);
            this.connect_ip_btn.TabIndex = 5;
            this.connect_ip_btn.Text = "Connect";
            this.connect_ip_btn.UseVisualStyleBackColor = true;
            // 
            // uname_lbl
            // 
            this.uname_lbl.AutoSize = true;
            this.uname_lbl.Location = new System.Drawing.Point(9, 9);
            this.uname_lbl.Name = "uname_lbl";
            this.uname_lbl.Size = new System.Drawing.Size(58, 13);
            this.uname_lbl.TabIndex = 6;
            this.uname_lbl.Text = "Username:";
            // 
            // user_online
            // 
            this.user_online.AutoSize = true;
            this.user_online.Location = new System.Drawing.Point(9, 45);
            this.user_online.Name = "user_online";
            this.user_online.Size = new System.Drawing.Size(63, 13);
            this.user_online.TabIndex = 7;
            this.user_online.Text = "User online:";
            // 
            // set_username_box
            // 
            this.set_username_box.Location = new System.Drawing.Point(73, 4);
            this.set_username_box.Name = "set_username_box";
            this.set_username_box.Size = new System.Drawing.Size(94, 20);
            this.set_username_box.TabIndex = 8;
            // 
            // chat_list_box
            // 
            this.chat_list_box.Location = new System.Drawing.Point(173, 45);
            this.chat_list_box.Multiline = true;
            this.chat_list_box.Name = "chat_list_box";
            this.chat_list_box.Size = new System.Drawing.Size(337, 361);
            this.chat_list_box.TabIndex = 9;
            // 
            // message_lbl
            // 
            this.message_lbl.AutoSize = true;
            this.message_lbl.Location = new System.Drawing.Point(9, 421);
            this.message_lbl.Name = "message_lbl";
            this.message_lbl.Size = new System.Drawing.Size(50, 13);
            this.message_lbl.TabIndex = 10;
            this.message_lbl.Text = "Message";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 450);
            this.Controls.Add(this.message_lbl);
            this.Controls.Add(this.chat_list_box);
            this.Controls.Add(this.set_username_box);
            this.Controls.Add(this.user_online);
            this.Controls.Add(this.uname_lbl);
            this.Controls.Add(this.connect_ip_btn);
            this.Controls.Add(this.connect_ip_box);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.message_box);
            this.Controls.Add(this.send_msg_btn);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Wangsaf";
            this.Text = "Wangsaf";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button send_msg_btn;
        private System.Windows.Forms.TextBox message_box;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox connect_ip_box;
        private System.Windows.Forms.Button connect_ip_btn;
        private System.Windows.Forms.Label uname_lbl;
        private System.Windows.Forms.Label user_online;
        private System.Windows.Forms.TextBox set_username_box;
        private System.Windows.Forms.TextBox chat_list_box;
        private System.Windows.Forms.Label message_lbl;
    }
}

