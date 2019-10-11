using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using WinRobots.UComTral;
using WinRobots.Comm;

namespace WinRobots
{
    public partial class UCanPanl : UserControl
    {
        public Action<string> _UTask;
        ComData _ComData;
        RoutePlanner planner = new RoutePlanner();
        DataTable _Pdatatable;

        public UCanPanl()
        {
            InitializeComponent();
            _ComData = new ComData();
            _ComData.initFNode();
            initPath();
        }

        public void initData(DataRow Dr)
        {
            //this.label1.Text = Dr["菜名"].ToString();
            //this.pictureBox1.BackgroundImage = ByteArrayToImage((byte[])Dr["照片"]);
        }
        public void InitSytle()
        {
            #region DataGridVeiw Style
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightCyan;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveCaption; ;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;//211, 223, 240
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(223)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridView1.ReadOnly = true;
            //this.dataGridView1.RowHeadersVisible = false;
            //this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            #endregion
        }

        public void initTable()
        {
            this.comboBox1.SelectedIndex = 1;
            this.comboBox2.SelectedIndex = 1;
            DataTable _datatable = new DataTable();
            _datatable.Columns.Add("起始地标");
            _datatable.Columns.Add("目的地标");
            _datatable.Columns.Add("执行时间");
            _datatable.Columns.Add("执行状态");
            this.dataGridView1.DataSource = _datatable;
         
        }

        public string LocalPoint = "";

        public void SetLocal(string Point)
        {
            LocalPoint = Point;
            this.label4.Text = "当前坐标：" + Point;
        }

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.label1.Text = "目的地区域：" + comboBox1.Text + "  坐标：" + comboBox2 .Text + " 已提交。";
            string Fction = "";
            string BeginP = "A14";
            string EndP = "A11";
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
                Fction += strln;
            }
            CreateNode(Fction + EndP);
        }

        public void CreateNode(string Path)
        {
            string Strl = Path.Replace("A","_");
            if (_UTask != null)
            {
                _UTask(Strl);
            }
        }

        //区域设置
        Hashtable _htC = new Hashtable();
        Hashtable _htCName = new Hashtable();
        public void initPath()
        {
            _htC.Add("A", "1_2_3");
            _htC.Add("B", "4_5_6");
            _htC.Add("C", "9_8_7");
            _htC.Add("D", "14_13");
            _htC.Add("E", "10_11_12");
            _htCName.Add("成品区", "1_2_3");
            _htCName.Add("半成品区1", "4_5_6");
            _htCName.Add("流水线1", "9_8_7");
            _htCName.Add("流水线2", "14_13");
            _htCName.Add("半成品区2", "10_11_12");
        }

        private void UCanPanl_Load(object sender, EventArgs e)
        {
            InitSytle();
            initTable();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (string skey in _htCName.Keys)
            {
                if (skey == comboBox1.Items[comboBox1.SelectedIndex].ToString())
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
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _Pdatatable = this.dataGridView1.DataSource as DataTable;

            DataRow Dr = _Pdatatable.NewRow();
          
            Dr["起始地标"] = "21";
            Dr["目的地标"] = "11";
            Dr["执行时间"] = "2016-01-23";
            Dr["执行状态"] = "正常流程";
            _Pdatatable.Rows.Add(Dr);
            dataGridView1.DataSource = _Pdatatable;

        }
    }
}
