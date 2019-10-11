namespace WinRobots
{
    partial class USetPLC
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("1号AGV");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("2号AGV");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("一区", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(USetPLC));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtPort = new System.Windows.Forms.TextBox();
            this.TxtIP = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button15 = new System.Windows.Forms.Button();
            this.btnzzw = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.comboBox2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox4.Location = new System.Drawing.Point(427, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(321, 372);
            this.groupBox4.TabIndex = 59;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "COM连接配置";
            // 
            // comboBox2
            // 
            this.comboBox2.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "14400",
            "19200",
            "34800",
            "56000",
            "57600",
            "115200",
            "128000"});
            this.comboBox2.Location = new System.Drawing.Point(105, 96);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(156, 20);
            this.comboBox2.TabIndex = 51;
            this.comboBox2.Text = "9600";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(28, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 50;
            this.label1.Text = "波特率：";
            // 
            // comboBox1
            // 
            this.comboBox1.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Com1",
            "Com2",
            "Com3",
            "Com4"});
            this.comboBox1.Location = new System.Drawing.Point(105, 38);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(156, 20);
            this.comboBox1.TabIndex = 49;
            this.comboBox1.Text = "Com1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label4.Location = new System.Drawing.Point(28, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 48;
            this.label4.Text = "串口号：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.TxtPort);
            this.groupBox1.Controls.Add(this.TxtIP);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox1.Location = new System.Drawing.Point(824, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 114);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据服务配置";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 12);
            this.label2.TabIndex = 47;
            this.label2.Text = "服务IP：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "端口号：";
            // 
            // TxtPort
            // 
            this.TxtPort.Location = new System.Drawing.Point(85, 65);
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(193, 21);
            this.TxtPort.TabIndex = 2;
            this.TxtPort.Text = "30";
            this.TxtPort.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hightspeed_MouseDown);
            // 
            // TxtIP
            // 
            this.TxtIP.Location = new System.Drawing.Point(85, 26);
            this.TxtIP.Name = "TxtIP";
            this.TxtIP.Size = new System.Drawing.Size(193, 21);
            this.TxtIP.TabIndex = 15;
            this.TxtIP.Text = "100";
            this.TxtIP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hightspeed_MouseDown);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.treeView1);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox3.Location = new System.Drawing.Point(16, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(386, 375);
            this.groupBox3.TabIndex = 62;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "AGV配置";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.treeView1.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.treeView1.Location = new System.Drawing.Point(3, 17);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "节点1";
            treeNode1.Text = "1号AGV";
            treeNode2.Name = "节点2";
            treeNode2.Text = "2号AGV";
            treeNode3.Name = "节点0";
            treeNode3.Text = "一区";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.treeView1.Size = new System.Drawing.Size(323, 355);
            this.treeView1.TabIndex = 144;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::WinRobots.Properties.Resources.backpath;
            this.panel1.Controls.Add(this.button15);
            this.panel1.Controls.Add(this.btnzzw);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(825, 542);
            this.panel1.TabIndex = 63;
            // 
            // button15
            // 
            this.button15.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button15.BackgroundImage")));
            this.button15.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button15.FlatAppearance.BorderSize = 0;
            this.button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button15.Font = new System.Drawing.Font("幼圆", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button15.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button15.Location = new System.Drawing.Point(629, 445);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(119, 41);
            this.button15.TabIndex = 135;
            this.button15.Text = "设置";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // btnzzw
            // 
            this.btnzzw.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnzzw.BackgroundImage")));
            this.btnzzw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnzzw.FlatAppearance.BorderSize = 0;
            this.btnzzw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnzzw.Font = new System.Drawing.Font("幼圆", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnzzw.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnzzw.Location = new System.Drawing.Point(472, 445);
            this.btnzzw.Name = "btnzzw";
            this.btnzzw.Size = new System.Drawing.Size(119, 41);
            this.btnzzw.TabIndex = 134;
            this.btnzzw.Text = "编辑";
            this.btnzzw.UseVisualStyleBackColor = true;
            // 
            // USetPLC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "USetPLC";
            this.Size = new System.Drawing.Size(825, 542);
            this.Load += new System.EventHandler(this.USetPLC_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtPort;
        private System.Windows.Forms.TextBox TxtIP;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button btnzzw;
    }
}
