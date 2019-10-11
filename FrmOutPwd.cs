using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinRobots
{
    public partial class FrmOutPwd : Form
    {
        public FrmOutPwd()
        {
            InitializeComponent();
        }
        public string cr = "";
        private void button2_Click(object sender, EventArgs e)
        {
            cr = "cm";
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cr = this.textBox1.Text;
            this.Close();
        }
    }
}
