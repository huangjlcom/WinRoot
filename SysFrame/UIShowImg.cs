using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WinRobots.Comm;
using WinRobots.UComTral;
using System.Threading;
using System.Collections;
using WinRobots.WFShow;

namespace WinRobots.SysFrame
{
    public partial class UIShowImg : UserControl
    {

        public Action<string> _SetModBus;
        public Action<object> _SetObjModBus;

        SetModbusItme _SetModbusItme;
        private delegate void ReadDataText();
        Queue _Ques;
        string _DevID = "1";
        //
        public static Hashtable _PublicHash;

        public UIShowImg()
        {
            InitializeComponent();
            _ComData = new ComData();
            _ComData.initFNode();
            _SetModbusItme = new SetModbusItme();
      
            ReadDataText _WritePLData = new ReadDataText(ReadData);
            _WritePLData.BeginInvoke(null, null);
            _Ques = new Queue();

            _PublicHash = new Hashtable();
        }

        public void ReadData()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (_Ques.Count > 0)
                {
                    string[] strl = _Ques.Dequeue().ToString().Replace("A", "").Split('_');
                    if (strl.Length == 0) continue;
                    textBox1.Text = strl[strl.Length - 1];
                    foreach (string tempstr in strl)
                    {
                        Thread.Sleep(500);
                        SetPoint(tempstr, _DevID);
                    }
                    Thread.Sleep(2000);
                    SetColor();
                }
            }
        }
        string LocalPoint = "";
        ComData _ComData;
        RoutePlanner planner = new RoutePlanner();


        //设置避障点
        public string _SetPoint = "";
        private void SetFunction(object sender, EventArgs e)
        {
            switch (((Button)sender).Text)
            {
                case "路径规划":
                    WinRole _WinRole = new WinRole();
                    if (_SetPoint != "")
                    {
                        _WinRole.MoveNodeID(_SetPoint);
                    }
                    _WinRole.ShowDialog();
                    if (_WinRole.PassStr != "")
                    {
                        _Ques.Enqueue(_WinRole.PassStr);
                        _DevID = _WinRole.DevID;
                    }
                    break;
                case "避障设置":
                    WinSigne _WinSigne = new WinSigne();
                    _WinSigne.ShowDialog();
                    if (_WinSigne.RetuStr != "")
                    {
                        foreach (Control Conn in this.panel2.Controls)
                        {
                            if (Conn is Button)
                            {
                                if (Conn.Text.Trim() == _WinSigne.RetuStr)
                                {
                                    _SetPoint = _WinSigne.RetuStr;
                                    btnA2.Location = new Point(Conn.Location.X, Conn.Location.Y);
                                }
                            }
                        }
                    }
                    SetColor();
                    break;

                case "任务管理":
                    WinTask _WinTask = new WinTask();
                    _WinTask.ShowDialog();
                    //WinSigne _WinSigne = new WinSigne();
                    //_WinSigne.ShowDialog();
                    //if (_WinSigne.RetuStr != "")
                    //{
                    //    foreach (Control Conn in this.panel2.Controls)
                    //    {
                    //        if (Conn is Button)
                    //        {
                    //            if (Conn.Text.Trim() == _WinSigne.RetuStr)
                    //            {
                    //                _SetPoint = _WinSigne.RetuStr;
                    //                btnA2.Location = new Point(Conn.Location.X, Conn.Location.Y);
                    //            }
                    //        }
                    //    }
                    //}
                    //SetColor();
                    break;
            }
        }

        public void SetColor()
        {
            foreach (Control Conn in this.panel2.Controls)
            {
                if (Conn is Button)
                {
                    if (Conn.Text.Length < 3)
                    {
                        Conn.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                    }
                }
            }

        }

        public void SetPoint(string _Point,string _show)
        {
            foreach (Control Conn in this.panel2.Controls)
            {
                if (Conn is Button)
                {
                    if (Conn.Text.Trim() == _Point)
                    {
                        if (_show == "1")
                        {
                            btnA1.Location = new Point(Conn.Location.X+ 30 , Conn.Location.Y);
                            Conn.ForeColor = Color.Red;
                        }
                       else if (_show == "2")
                        {
                            btnA2.Location = new Point(Conn.Location.X + 30, Conn.Location.Y);
                            Conn.ForeColor = Color.Blue;
                        }
                        else if (_show == "3")
                        {
                            btnA3.Location = new Point(Conn.Location.X + 30, Conn.Location.Y);
                            Conn.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }

        //避障处理
        //发送下位机指令
        public void SetSendCMD(string CMD)
        {
            if (_SetObjModBus != null)
            {
                _SetModbusItme.DevID = 1;
                _SetModbusItme.EndCount = 5;
                _SetModbusItme.Begin = 0;
                short[] S = new short[5];

                switch (CMD)
                {
                    case "5":
                        S[1] = 5;
                        S[2] = 2011;
                        S[3] = 3007;
                        S[4] = 0;
                        break;
                    case "10":
                        break;
                    case "12":
                        S[1] = 12;
                        S[2] = 2004;
                        S[3] = 3007;
                        S[4] = 0;
                        break;
                }
                _SetModbusItme.Reshort = S;
                _SetObjModBus(_SetModbusItme);

            }

        }
        public string ShowID = "1";
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

                if (VText.DevID.ToString() == ShowID)
                {
                    this.richTextBox1.Text = "设备编号：" + VText.DevID.ToString() + GetStringTo(VText.ResUshort);
                }
                ushort[] Val = VText.ResUshort;
                if (VText.DevID.ToString() == "1")
                {
                    if (VText.ResUshort[0].ToString() == "0")
                    {
                        textBox3.Text = "暂停";
                    }
                    else
                    {
                        textBox3.Text = "行驶";
                    }
                    textBox1.Text = Val[1].ToString(); textBox6.Text =(Val[10]*100).ToString(); textBox8.Text = Val[26].ToString();
                    SetPoint(textBox8.Text,"1");

                }
                else if (VText.DevID.ToString() == "2")
                {
                    if (VText.ResUshort[0].ToString() == "0")
                    {
                        txtRN1.Text = "暂停";
                    }
                    else
                    {
                        txtRN1.Text = "行驶";
                    }
                    txtRN2.Text = Val[1].ToString(); txtRN3.Text = Val[26].ToString();txtRN4.Text = Val[10].ToString(); 
                }
                else if (VText.DevID.ToString() == "3")
                {
                    if (VText.ResUshort[0].ToString() == "0")
                    {
                        txtRM1.Text = "暂停";
                    }
                    else
                    {
                        txtRM1.Text = "行驶";
                    }
                    txtRM2.Text = Val[1].ToString(); txtRM4.Text = Val[10].ToString(); txtRM3.Text = Val[26].ToString(); 
                    SetPoint(txtRM3.Text, "3");
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
                }
            }
            return Tempstr;
        }

        private void SetFuntion(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "button1":
                    ShowID = "1";
                    break;
                case "button2":
                    ShowID = "2";
                    break;
                case "button22":
                    ShowID = "3";
                    break;
            }

        }

    }
}
