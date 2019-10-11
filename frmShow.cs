using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace WinRobots
{
    public partial class frmShow : Form
    {
        public frmShow()
        {
            InitializeComponent();
        }

        private void FAction(object sender, EventArgs e)
        {
            Button _Control = sender as Button;
           // _Control.BackColor = System.Drawing.Color.LightCoral;
            switch (_Control.Text.Trim())
            {
                case "取消"://停止
                    this.Close();
                    break;
                case "回退"://停止
                    if (this.textBox1.Text.Trim().Length > 0)
                    {
                        this.textBox1.Text = this.textBox1.Text.Remove(this.textBox1.Text.Length - 1);
                    }
                    break;
                case "清除"://停止
                    this.textBox1.Text = "";
                    break;
                case "确定"://停止
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Text = textBox1.Text;
                    this.Close();
                    break;
                case "."://停止
                    this.textBox1.Text += ".";
                    break;
                case "0"://停止
                    this.textBox1.Text += "0";
                    break;
                case "1"://停止
                    this.textBox1.Text += "1";
                    break;
                case "2"://停止
                    this.textBox1.Text += "2";
                    break;
                case "3"://停止
                    this.textBox1.Text += "3";
                    break;
                case "4"://停止
                    this.textBox1.Text += "4";
                    break;
                case "5"://停止
                    this.textBox1.Text += "5";
                    break;
                case "6"://停止
                    this.textBox1.Text += "6";
                    break;
                case "7"://停止
                    this.textBox1.Text += "7";
                    break;
                case "8"://停止
                    this.textBox1.Text += "8";
                    break;
                case "9"://停止
                    this.textBox1.Text += "9";
                    break;
                case "-"://停止
                    if (this.textBox1.Text.Trim().Length == 0)
                    {
                        this.textBox1.Text = "-";
                        break;
                    }
                    if (this.textBox1.Text.Substring(0, 1) == "-")
                    {
                        this.textBox1.Text = this.textBox1.Text.Replace("-","");
                    }
                    else
                    {
                        this.textBox1.Text = "-" + this.textBox1.Text;
                    }
                    break;
            }
        }
    }
}
