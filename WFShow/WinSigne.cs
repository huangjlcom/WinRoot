using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinRobots.UComTral;

namespace WinRobots.WFShow
{
    public partial class WinSigne : Form
    {
        public WinSigne()
        {
            InitializeComponent();

        }
        public WinSigne(string type)
        {
            InitializeComponent();
            if (type == "1")
            {
                comboBox2.Items.Clear();

                for (int i = 1; i < 15; i++)
                {
                    comboBox2.Items.Add(i.ToString());
                }
            }
        }
   
        public string RetuStr = "";
        private void button2_Click(object sender, EventArgs e)
        {
            RetuStr = this.comboBox2.Text;
            this.Close();
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
