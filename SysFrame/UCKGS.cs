using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using WinRobots.UComTral;
using WinRobots.Comm;
using System.Threading;
using WinRobots.WFShow;

namespace WinRobots.SysFrame
{
    public partial class UCKGS : UserControl
    {

        public Action<string> _SetModBus;
        public Action<object> _SetObjModBus;

        SetModbusItme _SetModbusItme;
        private delegate void ReadDataText();
        Queue _Ques;

        private delegate void RunDataText();

        private delegate void PLData();
        public string _SetCall = "";

        //D
        public string _LocalHost = "";
        //DateTime on Point
        public string _LPoint = "";

        //缓冲的任务
        public static Hashtable _PublicHash;

        string LocalPoint = "";
        string LocalFW = "";
        //路径规划
       
       // RoutePlanner planner = new RoutePlanner();

        public UCKGS()
        {
            InitializeComponent();
            _PublicHash = new Hashtable();
            _ComData = new ComData();
            _ComData.initFNode();
            _SetModbusItme = new SetModbusItme();
            _Ques = new Queue();
            initHT();

            RunDataText _RunDataText = new RunDataText(RunData);
            _RunDataText.BeginInvoke(null, null);


            PLData _WritePLData = new PLData(ExceLback);
            _WritePLData.BeginInvoke(null, null);
          
        }
        //获取地标
        //获取路径规划。
        public void RunData()
        {
            while (true)
            {
                Thread.Sleep(1000);

                if (_Ques.Count > 0)
                {
                    Hashtable _ht = (Hashtable)_Ques.Dequeue();

                    Queue _Exeque;
                    Hashtable _tempht = new Hashtable();
                    foreach (string skey in _ht.Keys)
                    {
                        _Exeque = new Queue();
                        string[] strl = _ht[skey].ToString().Replace("A", "").Split('_');
                        if (strl.Length == 0) continue;
                        if (_LocalHost == "")
                        {
                            _LocalHost = strl[strl.Length - 1];
                        }
                        else
                        {
                            ArrayList _tempstr = new ArrayList();
                            foreach (string str in strl)
                            {
                                _tempstr.Add(str);
                            }

                            if (_tempstr.Contains(_LPoint))
                            {
                                this.label5.Text = "地标" + _LocalHost + "被占用，AGV#" + skey + " 请等待行驶！";
                                continue;
                            }

                            //if (_tempstr.Contains(_LocalHost))
                            //{
                            //    this.label5.Text = "地标" + _LocalHost + "被占用，AGV#" + skey + " 请等待行驶！";
                            //    continue;
                            //}
                        }

                        foreach (string tempstr in strl)
                        {
                            _Exeque.Enqueue(tempstr);
                        }
                        _tempht.Add(skey, _Exeque);
                    }
                    ExeTask(_tempht);
              
                    Thread.Sleep(2000);
                    SetColor();
                }
            }
        }

        //执行布进的任务
        public void ExeTask(Hashtable _Val)
        {
            foreach (string skey in _Val.Keys)
            {
                if (((Queue)_Val[skey]).Count > 0)
                {
                    string[] tempstrl = new string[2];
                    tempstrl[0] = skey;
                    tempstrl[1] = ((Queue)_Val[skey]).Dequeue().ToString();
                    SetPoint(tempstrl);
                    Thread.Sleep(500);
                    ExeTask(_Val);
                }
            }
         
        }

        public void SetRunTask(string[] Val)
        {
            SetPoint(Val);
        }
       

        public void ReadData()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (_Ques.Count > 0)
                {
                 
                }
            }
        }
        ComData _ComData;
        RoutePlanner planner = new RoutePlanner();

        //设置避障点
        public string _SetPoint = "";
        public string _DevID = "1";

        public string _ZAD = "0";
        private void SetFunction(object sender, EventArgs e)
        {
            switch (((Button)sender).Text)
            {
                case "路径规划":
                    WinRule _WinRule = new WinRule();
                    _LocalHost = "";
                    this.label5.Text = "";
                    if (_SetPoint != "")
                    {
                        _WinRule.MoveNodeID(_SetPoint);
                    }
                    _WinRule.ShowDialog();
                    if (_WinRule.PassStr != "")
                    {
                        _Ques.Enqueue(_WinRule._hts);
                        _DevID = _WinRule.DevID;
                    }
                    break;
                case "避障设置":
                    WinSigne _WinSigne = new WinSigne("1");
                    _WinSigne.ShowDialog();
                    if (_WinSigne.RetuStr != "")
                    {
                        foreach (Control Conn in this.panel1.Controls)
                        {
                            if (Conn is Button)
                            {
                                if (Conn.Text.Trim() == _WinSigne.RetuStr)
                                {
                                    _SetPoint = _WinSigne.RetuStr;
                                    //btnA2.Location = new Point(Conn.Location.X, Conn.Location.Y);
                                }
                            }
                        }
                    }
                    SetColor();
                    break;

                case "任务管理":
                    WinTask _WinTask = new WinTask();
                    _WinTask.ShowDialog();
                    break;
            }

        }

        public void SetColor()
        {
            foreach (Control Conn in this.panel1.Controls)
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

        public void SetPoint(string _Point)
        {
            foreach (Control Conn in this.panel1.Controls)
            {
                if (Conn is Button)
                {
                    if (Conn.Text.Trim() == _Point)
                    {
                        btnA1.Location = new Point(Conn.Location.X - 30, Conn.Location.Y);
                        Conn.ForeColor = Color.Red;
                        _LPoint = _Point;
                    }
                }
            }
        }
        //显示地标
        public void SetPoint(string[] _Point)
        {
            foreach (Control Conn in this.panel1.Controls)
            {
                if (Conn is Button)
                {
                    if (Conn.Text.Trim() == _Point[1])
                    {
                        if (_Point[0] == "1")
                        {
                            btnA1.Location = new Point(Conn.Location.X - 30, Conn.Location.Y);
                            Conn.ForeColor = Color.Red;
                        }
                        if (_Point[0] == "2")
                        {
                            btnA2.Location = new Point(Conn.Location.X - 30, Conn.Location.Y);
                            Conn.ForeColor = Color.Blue;
                        }
                        if (_Point[0] == "3")
                        {
                            btnA3.Location = new Point(Conn.Location.X + 30, Conn.Location.Y);
                            Conn.ForeColor = Color.Blue;
                        }
                    }
                }
            }
            //}
        }

        public string ShowID = "1";
        public delegate void CbDelegate<T>(T obj);
        public delegate void PointDelegate<T>(T obj);
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
                    LocalPoint = VText.ResUshort[26].ToString();
                    LocalFW = VText.ResUshort[25].ToString();
                 
                    GoCMD = VText.ResUshort[0].ToString();
                    GoPoint = VText.ResUshort[1].ToString();
                    GoQD = VText.ResUshort[9].ToString();
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
                    textBox1.Text = Val[1].ToString(); textBox6.Text = Val[10].ToString(); textBox8.Text =GetValue(Val[26].ToString())+"_"+ Val[26].ToString();
                    SetPoint(GetValue(Val[26].ToString()));

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
                    txtRN2.Text = Val[1].ToString(); txtRN3.Text = Val[26].ToString(); txtRN4.Text = Val[10].ToString();
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
                }
            }
        }
        //显示命令
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
            if (((Button)sender).Text.Trim().Length < 3)
            {
                SetGoPiont(((Button)sender).Text.Trim());
            }

        }
        public void SetGoPiont(string GoPoint)
        {
          
            string BeginP = "";
            string EndP = "";
            string ReBack = "";
            BeginP = GetValue(LocalPoint).Replace("_","");
            if (LocalPoint.Trim() == "")
            {
                MessageBox.Show("无当前坐标！");
                return;
            }
            if (BeginP != GoPoint)
            {
                BeginP = "A" + BeginP;
                EndP = "A" + GoPoint;
            }
            RoutePlanResult Tresult = planner.Paln(_ComData.nodeList, BeginP, EndP);
            foreach (string strln in Tresult.getPassedNodeIDs())
            {
                ReBack += "_" + strln;
            }
        
           string PassStr = ReBack + "_" + EndP;
           SetTask(PassStr.Replace("A", ""));
        }

        #region 仓库运行
        Hashtable _Fhts;
        public void initHT()
        {
            _Fhts = new Hashtable();
            _Fhts.Add("1", "2");
            _Fhts.Add("2", "5");
            _Fhts.Add("3", "8");
            _Fhts.Add("4", "11");
            _Fhts.Add("5", "14");
            _Fhts.Add("6", "17");
            _Fhts.Add("7", "20");
            _Fhts.Add("8", "23");
            _Fhts.Add("_8", "910");
            _Fhts.Add("9", "26");
            _Fhts.Add("10", "29");
            _Fhts.Add("11", "32");
            _Fhts.Add("12", "35");
            _Fhts.Add("13", "38");
            _Fhts.Add("14", "41");
            _Fhts.Add("15", "0");
        }
        #endregion

        #region  路径计算
        //设置任务型
        public void SetTask(string Task)
        {

            Task = Task.Replace("A", "_");
            string[] temps = Task.Split('_');
            string FunC = temps[temps.Length - 1];
            short[] S = new short[10];
            for (int i = 0; i < 10; i++)
            {
                S[i] = 0;
            }
            if (Task.IndexOf("14_9_6") > -1)
            {
                if (LocalFW == "3")
                {
                    S[0] = 9004;
                }
                else
                {
                    S[0] = 9001;
                }
                S[2] = (short)(2000 + int.Parse(_Fhts["14"].ToString()));
              
            }
            else if (Task.IndexOf("6_9_14") > -1)
            {
                if (LocalFW == "3")
                {
                    S[0] = 9004;
                }
                else
                {
                    S[0] = 9001;
                }
                S[2] = (short)(2000 + int.Parse(_Fhts["6"].ToString()));
                S[3] = 0;
                S[4] = 0;
            }
            else if (Task.IndexOf("9_14") > -1)
            {

                if (LocalFW == "4")
                {
                    S[0] = 9004;
                    S[3] = (short)(4000 + int.Parse(_Fhts["14"].ToString()));
                }
                else
                {
                    S[0] = 9001;
                    S[3] = (short)(1000 + int.Parse(_Fhts["14"].ToString()));
                }
                S[2] = (short)(3000 + int.Parse(_Fhts["9"].ToString()));

            }
            else if (Task.IndexOf("6_9") > -1)
            {
                if (LocalFW == "4")
                {
                    S[0] = 9004;
                    S[3] = (short)(4000 + int.Parse(_Fhts["9"].ToString()));
                }
                else
                {
                    S[0] = 9001;
                    S[3] = (short)(1000 + int.Parse(_Fhts["9"].ToString()));
                }
                S[2] = (short)(3000 + int.Parse(_Fhts["6"].ToString()));

            }
            else if (Task.IndexOf("9_6") > -1)
            {
                if (LocalFW == "4")
                {
                    S[0] = 9004;
                    S[3] = (short)(4000 + int.Parse(_Fhts["6"].ToString()));
                }
                else
                {
                    S[0] = 9001;
                    S[3] = (short)(1000 + int.Parse(_Fhts["6"].ToString()));
                }
                S[2] = (short)(2000 + int.Parse(_Fhts["9"].ToString()));


            }
            else if (Task.IndexOf("9_1") > -1)
            {
                if (LocalFW == "4")
                {
                    S[0] = 9004;
                    S[2] = (short)(3000 + int.Parse(_Fhts["9"].ToString()));
                }
                else
                {
                    S[0] = 9001;
                    S[2] = (short)(2000 + int.Parse(_Fhts["9"].ToString()));
                }
               // S[2] = (short)(2000 + int.Parse(_Fhts["9"].ToString()));

            }
            else if (Task.IndexOf("1_9") > -1)
            {
                if (LocalFW == "4")
                {
                    S[0] = 9001;
                    S[2] = (short)(2000 + int.Parse(_Fhts["1"].ToString()));
                }
                else
                {
                    S[0] = 9004;
                    S[2] = (short)(3000 + int.Parse(_Fhts["1"].ToString()));
                }
                // S[2] = (short)(2000 + int.Parse(_Fhts["9"].ToString()));

            }
            else if (Task.IndexOf("7_10") > -1)
            {
                if (LocalFW == "4")
                {
                    S[0] = 9004;
                }
                else
                {
                    S[0] = 9001;
                }
            }
            else if (Task.IndexOf("6_14") > -1)
            {
                if (LocalFW == "3")
                {
                    S[0] = 9004;
                    S[3] = (short)(4000 + int.Parse(_Fhts["14"].ToString()));
                }
                else
                {
                    S[0] = 9001;
                    S[3] = (short)(1000 + int.Parse(_Fhts["14"].ToString()));
                }
                S[2] = (short)(2000 + int.Parse(_Fhts["6"].ToString()));
            }
            else if (Task.IndexOf("14_9") > -1)
            {
                if (LocalFW == "1")
                {
                    S[0] = 9001;
                    S[3] = (short)(1000 + int.Parse(_Fhts["9"].ToString()));
                }
                else
                {
                    S[0] = 9004;
                    S[3] = (short)(4000 + int.Parse(_Fhts["9"].ToString()));
                }
                S[2] = (short)(2000 + int.Parse(_Fhts["14"].ToString()));
            }

            if (_SetObjModBus != null)
            {
                _SetModbusItme.DevID = 1;
                _SetModbusItme.EndCount = 10;
                _SetModbusItme.Begin = 0;
                S[1] = short.Parse(_Fhts[FunC].ToString());
                S[9] = 1;
                _SetModbusItme.Reshort = S;
                _SetObjModBus(_SetModbusItme);
            }
        }

        //来回走
        bool _FLBack = false;
        public string GoCMD = "";
        public string GoPoint = "";
        public string GoQD = "";
        public void ExceLback()
        {
            while (true)
            {
                if (_FLBack)
                {
                    Thread.Sleep(5000);
                    if (GetValue(LocalPoint) == "13")
                    {
                        if (GoCMD == "0" && GoPoint == "0" && GoQD == "0")
                        {
                            SetTask("_14_9_8");
                        }
                    }
                    if (GetValue(LocalPoint) == "8")
                    {
                        if (GoCMD == "0" && GoPoint == "0" && GoQD == "0")
                        {
                            SetTask("_9_14_13");
                        }
                    }
                    if (GetValue(LocalPoint) == "9")
                    {
                        if (GoCMD == "0" && GoPoint == "0" && GoQD == "0")
                        {
                            SetTask("9_1");
                        }
                    }
                    if (GetValue(LocalPoint) == "1")
                    {
                        if (GoCMD == "0" && GoPoint == "0" && GoQD == "0")
                        {
                            SetTask("1_9");
                        }
                    }

                }

            }
        }
        //转换坐标
        public string GetValue(string Val)
        {
            foreach (string Kval in _Fhts.Keys)
            {
                if (_Fhts[Kval].ToString() == Val)
                {
                    return Kval;
                }
            }
            return "0";
        }


        #endregion

        private void label6_Click(object sender, EventArgs e)
        {
            if (_FLBack)
            {
                _FLBack = false;
                label6.Text = "循：开启" ;
            }
            else
            {
                _FLBack = true;
                label6.Text = "循：停止" ;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
