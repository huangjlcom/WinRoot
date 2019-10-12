using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinRobots.UComTral;
using WinRobots.Comm;
using System.Collections;

namespace WinRobots.WFShow
{
    public partial class WinTask : Form
    {

        ComData _ComData;
        RoutePlanner planner = new RoutePlanner();
        Hashtable _htCName = new Hashtable();
        public string PassStr = "";
        public string DevID = "1";

        string LocalPoint = "";

        public WinTask()
        {
            InitializeComponent();

            _ComData = new ComData();
            _ComData.initRNode();
            initPath();

            comboBox2.SelectedIndex = 0;

            this.comboBox1.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void initPath()
        {
            _htCName.Add("成品区", "1_2_3");
            _htCName.Add("半成品区", "5_6_7");
            _htCName.Add("仓库区", "10_9_8");
            _htCName.Add("虚拟区", "11_12_13");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox5.Text == comboBox4.Text)
            {
                MessageBox.Show("不能同在一个地标上！");
                return;
            }
            LocalPoint = this.comboBox4.Text;
            this.richTextBox1.Text += "\n AGV#" + comboBox2 .Text+ ">> 起始地标: " + LocalPoint + " 开往目的地标：" + comboBox5.Text + " 已设定。";
            string Fction = "";
            string BeginP = "A14";
            string EndP = "A11";
            string ReBack = "";
            if (LocalPoint.Trim() == "")
            {
                MessageBox.Show("无当前坐标！");
                return;
            }
            if (LocalPoint != comboBox5.Text)
            {
                BeginP = "A" + LocalPoint;
                EndP = "A" + comboBox5.Text;
            }
            RoutePlanResult Tresult = planner.Paln(_ComData.nodeList, BeginP, EndP);
            foreach (string strln in Tresult.getPassedNodeIDs())
            {
                Fction += "->" + strln;
                ReBack += "_" + strln;
            }
            Fction = Fction + "->>" + EndP;
            PassStr = ReBack + "_" + EndP;

            richTextBox1.Text += " 路径：" + Fction.Replace("A", "");

            DevID = comboBox2.Text;
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox TempsCB = ((ComboBox)sender);
            foreach (string skey in _htCName.Keys)
            {
                if (skey == TempsCB.Items[TempsCB.SelectedIndex].ToString())
                {
                    if (TempsCB.Name == "comboBox1")
                    {
                        comboBox5.Items.Clear();
                        string[] strl = _htCName[skey].ToString().Split('_');
                        foreach (string tempstr in strl)
                        {
                            comboBox5.Items.Add(tempstr);
                        }
                        if (comboBox5.Items.Count > 0)
                        {
                            comboBox5.SelectedIndex = 0;
                        }
                    }
                    else if (TempsCB.Name == "comboBox3")
                    {
                        comboBox4.Items.Clear();
                        string[] strl = _htCName[skey].ToString().Split('_');
                        foreach (string tempstr in strl)
                        {
                            comboBox4.Items.Add(tempstr);
                        }
                        if (comboBox4.Items.Count > 0)
                        {
                            comboBox4.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }


    }
}
