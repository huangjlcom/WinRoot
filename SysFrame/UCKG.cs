using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WinRobots.Comm;
using WinRobots.UComTral;
using WinRobots.SysFrame;
using System.Collections;

namespace WinRobots
{
    public partial class UCKG : UserControl
    {

        public UCKG()
        {
            InitializeComponent();
            _ComData = new ComData();
            _ComData.initFNode();
            initButton();
        }
        public Action<string> _SetModBus;
        public Action<object> _SetObjModBus;

        SetModbusItme _SetModbusItme;

        string LocalPoint = "";
        string LocalFW = "";

        public void initButton()
        {
            foreach (Control Conn in this.Controls)
            {
                if (Conn is Button)
                {
                    Conn.MouseClick += new MouseEventHandler(Conn_MouseClick);
                }
            }
        }
        void Conn_MouseClick(object sender, MouseEventArgs e)
        {
            if (_SetObjModBus != null)
            {
                _SetModbusItme.DevID = 1;
                _SetModbusItme.EndCount = 5;
                _SetModbusItme.Begin = 0;
                short[] S = new short[5];
                switch (((Button)sender).Text)
                {
                    case "5":
                        if (LocalFW == "3")
                        {
                            S[0] = 9004;
                        }
                        else
                        {
                            S[0] = 9001;
                        }
                        S[1] = 5;
                        S[2] = 2011;
                        S[3] = 3007;
                        S[4] = 0;
                        break;
                    case "10":
                        break;
                    case "12":
                        if (LocalFW == "4")
                        {
                            S[0] = 9001;
                        }
                        else
                        {
                            S[0] = 9004;
                        }
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
        string LocalTxt = "";
        //设置坐标
        public object _TempObj;
        public void SetLocal(string Point)
        {
            foreach (Control Conn in this.panel2.Controls)
            {
                if (Conn.Text == Point)
                {
                    this.pictureBox2.Location =new Point( Conn.Location.X+35, Conn.Location.Y);
                }
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            _TempObj = sender;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
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
                if (VText.DevID.ToString() == "1")
                {
                    this.richTextBox1.Text = "设备编号：" + VText.DevID.ToString() + GetStringTo(VText.ResUshort);
                    SetLocal(VText.ResUshort[26].ToString());
                    LocalPoint = VText.ResUshort[26].ToString();
                    LocalFW = VText.ResUshort[25].ToString();
                }
                else
                {
                    this.richTextBox2.Text = "设备编号：" + VText.DevID.ToString() + GetStringTo(VText.ResUshort);
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

        ComData _ComData;
        RoutePlanner planner = new RoutePlanner();

        public void SetCall()
        {
            string Fction = "";
            string BeginP = "A14";
            string EndP = "A11";
            RoutePlanResult Tresult = planner.Paln(_ComData.nodeList, BeginP, EndP);
            foreach (string strln in Tresult.getPassedNodeIDs())
            {
                Fction += strln;
            }
            CreateNode(Fction + EndP);
        }
        public void CreateNode(string Path)
        {
            string Strl = Path.Replace("A", "_");
        }

        public void ConnBack()
        {

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
                S[3] = (short)(3000 + int.Parse(_Fhts["9"].ToString()));
                S[4] = 0;
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
                //  S[0] = 9001;
                S[1] = short.Parse(_Fhts[FunC].ToString());
                S[9] = 1;
                _SetModbusItme.Reshort = S;
                _SetObjModBus(_SetModbusItme);

            }
        }

        ////来回走
        //bool _FLBack = false;
        //public void ExceLback()
        //{
        //    while (true)
        //    {
        //        if (_FLBack)
        //        {
        //            Thread.Sleep(7000);
        //            if (GetValue(LocalPoint) == "13")
        //            {
        //                if (GoCMD == "0" && GoPoint == "0" && GoQD == "0")
        //                {
        //                    SetTask("_14_9_8");
        //                }
        //            }
        //            if (GetValue(LocalPoint) == "8")
        //            {
        //                if (GoCMD == "0" && GoPoint == "0" && GoQD == "0")
        //                {
        //                    SetTask("_9_14_13");
        //                }
        //            }

        //        }

        //    }
        //}

        #endregion
    }
}
