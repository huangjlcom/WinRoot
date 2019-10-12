using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinRobots.Comm;
using System.Collections;
using WinRobots.UComTral;

namespace WinRobots.WFShow
{
    public partial class WinRule : Form
    {

        ComData _ComData;
        RoutePlanner planner = new RoutePlanner();
        Hashtable _htCName = new Hashtable();
        public string PassStr = "";
        public string DevID = "1";

        public Queue _que = new Queue();

        public Hashtable _hts = new Hashtable();
        public WinRule()
        {
            InitializeComponent();

            _ComData = new ComData();
            _ComData.initFNode();
            initPath();
            InitComtext();
            comboBox5.SelectedIndex = 0;

            this.comboBox1.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PassStr = "";
            this.Close();
        }

        public string LocalPoint = "";

        private void button2_Click(object sender, EventArgs e)
        {
           
            if (comboBox2.Text == comboBox4.Text)
            {
                MessageBox.Show("不能同在一个地标上！");
                return;
            }
            LocalPoint = this.comboBox4.Text;
           
            string Fction = "";
            string BeginP = "A14";
            string EndP = "A11";
            string ReBack = "";
            if (LocalPoint.Trim() == "")
            {
                MessageBox.Show("无当前坐标！");
                return;
            }
            if (LocalPoint != comboBox2.Text)
            {
                BeginP = "A" + LocalPoint;
                EndP = "A" + comboBox2.Text;
            }
            RoutePlanResult Tresult = planner.Paln(_ComData.nodeList, BeginP, EndP);
            foreach (string strln in Tresult.getPassedNodeIDs())
            {
                Fction += ">" + strln;
                ReBack += "_" + strln;
            }
            Fction = Fction + "->" + EndP;
            PassStr = ReBack + "_" + EndP;

            if (_hts.ContainsKey(comboBox5.Text))
            {
                if (MessageBox.Show("已经设置过AGV#" + comboBox5.Text + ",是否要覆盖之前设置", "提示！", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    _hts[comboBox5.Text] = PassStr;
                    return;
                }
                else
                {
                    return;
                }
            }
            _hts.Add(comboBox5.Text, PassStr);
            this.richTextBox1.Text += "\nAGV#编号： " + comboBox5.Text + " 已设置,";
            richTextBox1.Text += " 路径：" + Fction.Replace("A", "");

            DevID = comboBox5.Text;
        }

        public void initPath()
        {
            _htCName.Add("成品区", "1_2_3"); 
            _htCName.Add("半成品区", "4_5_6"); 
            _htCName.Add("流水线1", "7_8_9");
            _htCName.Add("流水线2", "13_14");
            _htCName.Add("仓库区", "10_11_12");

        }

        public void InitComtext()
        {
            foreach (string skey in _htCName.Keys)
            {
                this.comboBox1.Items.Add(skey);
                this.comboBox3.Items.Add(skey);
            }
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
                        comboBox2.Items.Clear();
                        string[] strl = _htCName[skey].ToString().Split('_');
                        foreach (string tempstr in strl)
                        {
                            comboBox2.Items.Add(tempstr);
                        }
                        if (comboBox2.Items.Count > 0)
                        {
                            comboBox2.SelectedIndex = 0;
                        }
                    }
                    else
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
        //
        public void MoveNodeID(string _Point)
        {
            _ComData.Clear();
            _ComData.initRNode();
            _ComData.MovNode(_Point);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
