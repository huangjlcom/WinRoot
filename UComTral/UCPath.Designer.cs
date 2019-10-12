namespace WinRobots.UComTral
{
    partial class UCPath
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSendCmd = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.btnRW = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSendCmd
            // 
            this.btnSendCmd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSendCmd.Location = new System.Drawing.Point(758, 11);
            this.btnSendCmd.Name = "btnSendCmd";
            this.btnSendCmd.Size = new System.Drawing.Size(101, 36);
            this.btnSendCmd.TabIndex = 68;
            this.btnSendCmd.Text = "发送命令集";
            this.btnSendCmd.UseVisualStyleBackColor = true;
            this.btnSendCmd.Click += new System.EventHandler(this.btnSendCmd_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(640, 61);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(219, 227);
            this.richTextBox1.TabIndex = 67;
            this.richTextBox1.Text = "";
            // 
            // listBox4
            // 
            this.listBox4.FormattingEnabled = true;
            this.listBox4.ItemHeight = 12;
            this.listBox4.Items.AddRange(new object[] {
            "从1点",
            "从2点",
            "从3点",
            "从4点",
            "从5点",
            "从6点",
            "从7点",
            "从8点",
            "从9点",
            "从10点",
            "从11点",
            "从12点"});
            this.listBox4.Location = new System.Drawing.Point(391, 11);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(101, 268);
            this.listBox4.TabIndex = 66;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Items.AddRange(new object[] {
            "到1点",
            "到2点",
            "到3点",
            "到4点",
            "到5点",
            "到6点",
            "到7点",
            "到8点",
            "到9点",
            "到10点",
            "到11点",
            "到12点"});
            this.listBox2.Location = new System.Drawing.Point(515, 11);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(101, 268);
            this.listBox2.TabIndex = 65;
            // 
            // btnRW
            // 
            this.btnRW.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRW.Location = new System.Drawing.Point(638, 11);
            this.btnRW.Name = "btnRW";
            this.btnRW.Size = new System.Drawing.Size(101, 36);
            this.btnRW.TabIndex = 64;
            this.btnRW.Text = "生成命令集";
            this.btnRW.UseVisualStyleBackColor = true;
            this.btnRW.Click += new System.EventHandler(this.btnRW_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(25, 295);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 36);
            this.button1.TabIndex = 70;
            this.button1.Text = "画图";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.BackgroundImage = global::WinRobots.Properties.Resources.QQ图片20160809113248;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(385, 279);
            this.pictureBox2.TabIndex = 69;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(174, 285);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(345, 98);
            this.richTextBox2.TabIndex = 71;
            this.richTextBox2.Text = "";
            // 
            // UCPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnSendCmd);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.listBox4);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.btnRW);
            this.Name = "UCPath";
            this.Size = new System.Drawing.Size(876, 434);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnSendCmd;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button btnRW;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox2;
    }
}
