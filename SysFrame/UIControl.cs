using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WinRobots.SysFrame
{
    public partial class UIControl : UserControl
    {

        public Action<string> _SetCMD;
        frmShow _frmShow;
        object _obj;

        public Action<object> _SetObjModBus;

        SetModbusItme _SetModbusItme;

        public static bool _SetCall = false;
        public string DevID = "1";
        public UIControl()
        {
            InitializeComponent();
            SetColor();
        
            _SetModbusItme = new SetModbusItme();
        }

        private void SetFunction(object sender, EventArgs e)
        {
            Button _Control = sender as Button;
            SetColor();
            _Control.BackColor = System.Drawing.Color.LightCoral;
            switch (_Control.Text)
            {
                case "键盘"://停止
                    if (_obj == null)
                    {
                        return;
                    }
                    _frmShow = new frmShow();
                    if (_frmShow.ShowDialog() == DialogResult.OK)
                    {
                        ((Control)_obj).Text = _frmShow.Text;
                    }
                    break;
                case "开启"://停止
                    SetCMD(0);
                    break;
                case ""://停止
                    SetCMD(5);
                    break;
                case "前进"://前进
                    if (isqg)
                        SetUp();
                    SetCMD(1);
                    break;
                case "左转"://后退
                    if (isqg)
                        SetUp();
                    SetCMD(2);
                    break;
                case "目的地设置"://停止
                    if (_SetCMD != null)
                    {
                        _SetCMD("12_" + this.txtmdd.Text.ToString());
                    }
                    break;
                case "右转"://左转90
                    if (isqg)
                        SetUp();
                    SetCMD(3);
                    break;
                case "后退"://右转90
                    if (isqg)
                        SetUp();
                    SetCMD(4);
                    break;
                case "设置速度"://
                    _SetModbusItme.DevID = 1;
                    _SetModbusItme.EndCount = 5;
                    _SetModbusItme.Begin = 0;
                    short[] S = new short[5];
                    if (_SetObjModBus != null)
                    {
                        _SetModbusItme.Reshort = S;
                        _SetObjModBus(_SetModbusItme);
                    }
                    break;
                case "回原点":
                    break;
                case "11"://左转90
                    SetCMD(11);
                    break;
                case "12"://左转90
                    SetCMD(12);
                    break;
                case "AGV#1":
                    DevID = "1";
                    break;
                case "AGV#2":
                    DevID = "2";
                    break;
                case "AGV#3":
                    DevID = "3";
                    break;
            }
        }

        public void SetColor()
        {
            foreach (Control Con in this.panel1.Controls)
            {
                if (Con is Button)
                {
                    Con.BackColor = System.Drawing.Color.Transparent;
                }
            }
        }
        public void SetCMD(int cmd)
        {
            if (_SetCMD != null)
            {
                _SetCMD(cmd.ToString());
            }
        }

        public void ReText(string cmd)
        {
           
        }
        public delegate void CbDelegate<T>(T obj);

        //接收信息
        public void GetContext(GetModbusItme VText)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbDelegate<GetModbusItme>(this.GetContext), VText);
            }
            else
            {
                if (DevID == VText.DevID.ToString())
                {
                    this.richTextBox1.Text = "设备编号：" + VText.DevID.ToString() + GetStringTo(VText.ResUshort);
                }
            }
        }

        public string GetStringTo(ushort[] Val)
        {
            string Tempstr = "";
            for (int i = 0; i < Val.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        Tempstr += "\n当前指令：" + Val[i].ToString();
                        break;
                    case 1:
                        Tempstr += "\n目的坐标：" + Val[i].ToString();
                        break;
                    case 10:
                        Tempstr += "\n档位速度：" + Val[i].ToString();
                        break;
                    case 25:
                        Tempstr += "\n当前方位：" + Val[i].ToString();
                        break;
                    case 26:
                        LocalPoint = Val[i].ToString();
                        Tempstr += "\n当前坐标：" + Val[i].ToString();
                        break;
                    case 11:
                        Tempstr += "\n弯道速度：" + Val[i].ToString();
                        break;
                    case 28:
                        Tempstr += "\n电量：" + Val[i].ToString();
                        break;
                    case 34:
                        Tempstr += "\n电压：" + Val[i].ToString();
                        break;
                    case 9:
                        Tempstr += "\n卸货状态：" + Val[i].ToString();
                        break;
                }
            }
            return Tempstr;
        }

        public string LocalPoint;//坐标点
        public bool isqg = false;//

        private void hightspeed_MouseDown(object sender, MouseEventArgs e)
        {
            _obj = sender;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "下降顶杆")
            {
                if (_SetCMD != null)
                {
                    _SetCMD("12_" + LocalPoint);
                }
                button3.Text = "上升顶杆";
                isqg = true;
            }
            else
            {
                button3.Text = "下降顶杆";
                isqg = false;
            }
        }
        public void SetUp()
        {
            if (_SetCMD != null)
            {
                _SetCMD("13_1");
            }
        }
    }
      
}
