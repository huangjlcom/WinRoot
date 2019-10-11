using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Threading;
using IniFileRobot;
using ModelData;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using WinRobots.Comm;
using System.Net;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Collections;
using WinRobots.SysFrame;


namespace WinRobots
{
    public partial class RobotUI : Form
    {
        #region 功能定义

        UState _UState;

        UIControl _UIControl;
        UCKG _UCKG;
        UIShow _UIShow;
        UCanPanl _UCanPanl;
        UCKGS _UCKGS;


        IniFiles _Ini;
        private delegate void ReadDataText();
        IniFiles ini;
        JTQModBus _JTQModBus;
        Queue _Que;
        AGVManager[] _AGVManager;

        UIShowImg _UIShowImg;
        #endregion

        public RobotUI()
        {
            InitializeComponent();
            string FileIni = System.Windows.Forms.Application.StartupPath + @"/Config.ini";
            if (File.Exists(FileIni))
            {
                ini = new IniFiles(FileIni);
            }

            string _ComPort = ini.ReadString("Config", "ComPort", "");
            //  ComPort 
            initComm(_ComPort);//

            _AGVManager = new AGVManager[3];
            _AGVManager[0] = new AGVManager();
            _AGVManager[0].AGVID = 1;
            _AGVManager[1] = new AGVManager();
            _AGVManager[1].AGVID = 2;
            _AGVManager[2] = new AGVManager();
            _AGVManager[2].AGVID = 3;
            //_AGVManager[2] = new AGVManager();
      
        }

        #region 界面参数初始化
        public void InitParameter()
        {
            tabControl1.SelectedIndex = 0;
            RobotStatusTable();
            RobotStatusPage();
        }
     
        #endregion

        #region 通讯定义

        /// <summary>
        /// 将对象转换为byte数组
        /// </summary>
        /// <param name="obj">被转换对象</param>
        /// <returns>转换后byte数组</returns>
        public byte[] Object2Bytes(object obj)
        {
            byte[] buff;
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter iFormatter = new BinaryFormatter();
                iFormatter.Serialize(ms, obj);
                buff = ms.GetBuffer();
            }
            return buff;
        }
     
        private void button2_Click(object sender, EventArgs e)
        {
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            KillProcess("WinRobots");
        }
        private void KillProcess(string processName) //调用方法，传参
        {
            try
            {
                Process[] thisproc = Process.GetProcessesByName(processName);
                if (thisproc.Length > 0)
                {
                    for (int i = 0; i < thisproc.Length; i++)
                    {
                        thisproc[i].Kill(); //强制关闭
                    }
                }
            }
            catch //出现异常，表明 kill 进程失败
            {
            }
        }
        /// <summary>
        /// 将byte数组转换成对象
        /// </summary>
        /// <param name="buff">被转换byte数组</param>
        /// <returns>转换完成后的对象</returns>
        public static object Bytes2Object(byte[] buff)
        {
            object obj;
            using (MemoryStream ms = new MemoryStream(buff))
            {
                IFormatter iFormatter = new BinaryFormatter();
                obj = iFormatter.Deserialize(ms);
            }
            return obj;
        }
        #endregion

        #region 窗体载入
        /// <summary>
        /// 机器人控制界面载入
        /// </summary> 
        private void RobotControlUI_Load(object sender, EventArgs e)
        {
            InitParameter();
            _Ini = new IniFiles(Application.StartupPath + "/Config.ini");
            TxtIP = _Ini.ReadString("Config", "SeverTCPIP", "");
            TxtPort = _Ini.ReadString("Config", "SeverTCPPort", "");
            Function(this.label7, null);
            //Function(this.pCnf3, null);
        }
        string TxtIP = "";
        string TxtPort = "";

   
        #endregion
        #region 标题界面
        /// <summary>
        /// 机器人状态表
        /// </summary> 
        public void RobotStatusTable()
        {
            ListViewItem item;
            //添加表格内容
            for (int i = 1; i < 10 + 1; i++)
            {
                item = new ListViewItem();
                item.SubItems.Clear();
                item.SubItems[0].Text = i.ToString();
            }
            //tab控件隐藏标签栏
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(0, 1);
        }
        /// <summary>
        /// 机器人状态界面
        /// </summary> 
        public void RobotStatusPage()
        {
              label4.Text = DateTime.Now.ToString();
         
        }
    
        #endregion

        #region 主菜单事件
        bool _tmepUI = false;
        private void Function(object sender, EventArgs e)
        {
            Control Cols = sender as Control;
            switch (Cols.Text)
            {
                case "系统控制":
                    tabControl1.SelectedIndex = 0;
                    if (tabControl1.TabPages[0].Controls.Count == 0)
                    {
                        if (_tmepUI)
                        {
                            _UIShow = new UIShow();
                            _UIShow._SetModBus = new Action<string>(SetModBus);
                            _UIShow._SetObjModBus = new Action<object>(SendFuntion);
                            _UIShow.Dock = DockStyle.Fill;
                            tabControl1.TabPages[0].Controls.Add(_UIShow);
                        }
                        else
                        {

                            //_UIShowImg = new UIShowImg();
                            //_UIShowImg._SetModBus = new Action<string>(SetModBus);
                            //_UIShowImg._SetObjModBus = new Action<object>(SendFuntion);
                            //_UIShowImg.Dock = DockStyle.Fill;
                            //tabControl1.TabPages[0].Controls.Add(_UIShowImg);


                            _UCKGS = new UCKGS();
                            _UCKGS.Dock = DockStyle.Fill;
                            _UCKGS._SetModBus = new Action<string>(SetModBus);
                            _UCKGS._SetObjModBus = new Action<object>(SendFuntion);
                            tabControl1.TabPages[0].Controls.Add(_UCKGS);

                        }

                        //现场
                    }
                    break;
                case "手动控制":
                    tabControl1.SelectedIndex = 1;
                    if (tabControl1.TabPages[1].Controls.Count == 0)
                    {
                        _UIControl = new UIControl();
                        _UIControl._SetCMD = new Action<string>(SetUpathPoint);
                        _UIControl.Dock = DockStyle.Fill;
                        tabControl1.TabPages[1].Controls.Add(_UIControl);
                    }
                    break;
                case "日志查询"://
                    tabControl1.SelectedIndex = 2;
                    _UCanPanl = new UCanPanl();
                    _UCanPanl._UTask = new Action<string>(SetPLCTask);
                    if (tabControl1.TabPages[2].Controls.Count == 0)
                    {
                        _UCanPanl.Dock = DockStyle.Fill;
                        tabControl1.TabPages[2].Controls.Add(_UCanPanl);
                    }
                    break;
                case "系统设置":
                    tabControl1.SelectedIndex = 3;
                    if (tabControl1.TabPages[3].Controls.Count == 0)
                    {
                        tabControl1.TabPages[3].Controls.Add(new USetPLC());
                    }
                    break;
                case "Cnf5":
                    tabControl1.SelectedIndex = 4;
                      if (tabControl1.TabPages[4].Controls.Count == 0)
                    {
                        //tabControl1.TabPages[4].Controls.Add(new UMeus());
                    }
                    
                    break;
                case "Cnf6":
                    tabControl1.SelectedIndex = 5;
                    break;

                case "关机退出":
                    DialogResult result = MessageBox.Show("您确认要退出系统吗？", "深圳慧尔普温馨提示!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                    if (result == DialogResult.OK)
                    {
                        //执行OK的代码
                        this.Close();
                    }
                    else
                    {
                        //执行Cancel的代码
                    }
                    break;
            }
            
        }
        //设置地点坐标
        public void SetUpathPoint(string Point)
        {
            _queSend.Enqueue(Point);

        }
        public string _plcNew = "";
        /// <summary>
        /// /设置心跳数据
        /// </summary>
        /// <param name="Point"></param>
        public void SetPLCData(string Point)
        {
            if (_plcNew != "")
            {

            }
        }

        /// <summary>
        /// /设置任务
        /// </summary>
        /// <param name="Point"></param>
        public void SetPLCTask(string STask)
        {
            if (STask.Length > 0)
            {
                if (tabControl1.TabPages[1].Controls.Count > 0)
                {
                    foreach (Control obj in tabControl1.TabPages[1].Controls)
                    {
                        if (obj is UState )
                        {
                            ((UState)obj).SetTask(STask);
                        }
                    }
                }

            }
        }

        #endregion

   
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString();
        }

        private void btnLanguage_Click(object sender, EventArgs e)
        {
            //if (btnLanguage.Text=="CN")
            //{
            //    btnLanguage.Text="EN";
            //}
            //else
            //{
            //    btnLanguage.Text="CN";
            //}
        }

        private void uState1_Load(object sender, EventArgs e)
        {

        }


        #region 程序之间收发程序
        public struct CopyDataStruct
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        public const int WM_COPYDATA = 0x004A;
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        //在DLL库中的发送消息函数
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage
            (
            int hWnd,                         // 目标窗口的句柄  
            int Msg,                          // 在这里是WM_COPYDATA
            int wParam,                       // 第一个消息参数
            ref  CopyDataStruct lParam        // 第二个消息参数
           );
        //
        protected override void WndProc(ref System.Windows.Forms.Message e)
        {
            if (e.Msg == WM_COPYDATA)
            {
                CopyDataStruct cds = (CopyDataStruct)e.GetLParam(typeof(CopyDataStruct));
                //将文本信息显示到文本框
                string reuselt = cds.lpData.Trim();
                string[] reustype = reuselt.Split('_');
            }
            base.WndProc(ref e);
        }
        #endregion

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        private void RobotUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_JTQModBus != null)
            {
                _JTQModBus.Close();
            }
        }
        //#region 串口处理信息

        //private void RobotUI_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    //if (serialPort1 != null)
        //    //{
        //    //    if (serialPort1.IsOpen == true)
        //    //    {
        //    //        serialPort1.Close();
        //    //    }
        //    //}
        //}
       // SerialPort serialPort1;

        //private delegate void WritePLData();//写发送数据

        //private delegate void ReadPLData();//读取数据

        //public void initComm(string _Comm)
        //{
        //    try
        //    {
        //        if (serialPort1 == null)
        //        {
        //            serialPort1 = new SerialPort(_Comm, 9600, Parity.None, 8, StopBits.One);
        //            _Que = new Queue();
        //            //关键 为 serialPort1绑定事件句柄
        //            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
        //            serialPort1.Open();
        //            WritePLData _WritePLData = new WritePLData(SendData);
        //            _WritePLData.BeginInvoke(null, null);
        //            ReadPLData _ReadPLData = new ReadPLData(ReadData);
        //            _ReadPLData.BeginInvoke(null, null);
                  
        //        }
        //    }
        //    catch (Exception err)
        //    {
              
        //        MessageBox.Show(err.Message);
        //    }
        //}

        //public delegate void CbDelegate<T>(T obj);
        //private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    //关键 代理
        //    try
        //    {
        //        if (serialPort1.IsOpen == true)
        //        {
        //            byte[] readBuffer = new byte[serialPort1.BytesToRead];
        //            serialPort1.Read(readBuffer, 0, readBuffer.Length);
        //            string readstr = byteToHexStr(readBuffer);
        //            _Que.Enqueue(readstr);
        //            ShowMessage(readstr);

        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }
        //}


        ///// <summary>
        ///// 字节数组转16进制字符串
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <returns></returns>
        //public static string byteToHexStr(byte[] bytes)
        //{
        //    string returnStr = "";
        //    if (bytes != null)
        //    {
        //        for (int i = 0; i < bytes.Length; i++)
        //        {
        //            returnStr += bytes[i].ToString("X2");
        //        }
        //    }
        //    return returnStr;
        //}

        //private void ShowMessage(string msg)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.BeginInvoke(new CbDelegate<string>(this.ShowMessage), msg);
        //    }
        //    else
        //    {
        //        LogManager.WriteLog("FFsend", DateTime.Now.ToString() + "->" + GetPall(msg));
        //    }
        //}

        ////当前路径
        //public void SendFText(int Agv)
        //{
        //    //if (_Fsend)
        //    //{
        //    //    return;
        //    //}
        //    byte[] Creatsend = new byte[21];
        //    for (int i = 0; i < Creatsend.Length; i++)
        //    {
        //        Creatsend[i] = 0x00;
        //    }
        //    Creatsend[0] = 0xFE;
        //    Creatsend[1] = (byte)Agv;//s1AGV识别码
        //    Creatsend[2] = 0x00;//s2AGV装货状态
        //    Creatsend[3] = 0x00;
        //    Creatsend[4] = 0x00;
        //    Creatsend[5] = 0x23;//地标

        //    Creatsend[Creatsend.Length - 1] = 0xFF;
        //    SendComm(Creatsend);
        //}

        //public void SendComm(byte[] sendpy)
        //{
        //    try
        //    {
        //        if (serialPort1 != null)
        //        {
        //            serialPort1.Write(sendpy, 0, sendpy.Length);
        //            string readstr = byteToHexStr(sendpy);
        //            LogManager.WriteLog("FFsend", DateTime.Now.ToString() + ">>" + GetPall(readstr));

        //        }
        //    }
        //    catch (Exception EX)
        //    {
        //        MessageBox.Show(EX.Message, "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //public string GetPall(string ps)
        //{
        //    string Cmdtext = "";
        //    int m = 0;
        //    for (int i = 0; i < ps.Length; i += 2)
        //    {
        //        m++;
        //        Cmdtext += "#" + m.ToString() + "[" + ps.Substring(i, 2) + "]";
        //    }

        //    return Cmdtext.TrimStart('#');

        //}


        //public void SendData()
        //{
        //    while (true)
        //    {
        //        Thread.Sleep(500);
        //        SendFText(1);
        //        Thread.Sleep(500);
        //        SendFText(2);
        //    }
        //}

        //public void ReadData()
        //{
        //    string ReadText = "";
        //    while (true)
        //    {
        //        Thread.Sleep(2000);
        //        ReadText = "";
        //        while (_Que.Count > 0)
        //        {
        //            ReadText += _Que.Dequeue().ToString();
        //            foreach (Control obj in tabControl1.TabPages[0].Controls)
        //            {
        //                if (obj is UIShow)
        //                {
        //                    ((UIShow)obj).WriteText(ReadText);
        //                }
        //            }

        //        }
               
        //    }
        //}
        //public void SendComm(byte[] sendpy)
        //{
        //    try
        //    {
        //        if (serialPort1 != null)
        //        {
        //            serialPort1.Write(sendpy, 0, sendpy.Length);
        //            string readstr = byteToHexStr(sendpy);
        //            LogManager.WriteLog("FFsend", DateTime.Now.ToString() + ">>" + GetPall(readstr));

        //        }
        //    }
        //    catch (Exception EX)
        //    {
        //        MessageBox.Show(EX.Message, "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        //#endregion


        #region 485 串口处理信息

        private delegate void WritePLData();//写发送数据

        private delegate void ReadPLData();//读取数据

        public void initComm(string _Comm)
        {
            try
            {
                if (_JTQModBus == null)
                {
                    _JTQModBus = new JTQModBus();
                    _JTQModBus.Open(_Comm, 9600, 8,Parity.None,StopBits.One);
                  
                    _Que = new Queue();
                    WritePLData _WritePLData = new WritePLData(SendData);
                    _WritePLData.BeginInvoke(null, null);
             
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public delegate void CbDelegate<T>(T obj);
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

     

        public string GetPall(string ps)
        {
            string Cmdtext = "";
            int m = 0;
            for (int i = 0; i < ps.Length; i += 2)
            {
                m++;
                Cmdtext += "#" + m.ToString() + "[" + ps.Substring(i, 2) + "]";
            }
            return Cmdtext.TrimStart('#');

        }

        //设置地点坐标
     

        //设置队列
        Queue _queSend=new Queue();
        public void SetModBus(string Point)
        {
            _queSend.Enqueue(Point);
        }
        public void SendFuntion(string Point)
        {
            byte SendArr = (byte)1;
            Thread.Sleep(100);
            if (Point == "5")
            {
                SetFW();
                return;
            }

            string Result = "";
            ushort ab = 0;
            ushort abs = 1;
            short[] value = new short[1];
            value[0] = 0;
            if (Point == "1")
            {
                value[0] = 9001;
            }
            else if (Point == "2")
            {
                value[0] = 9002;
            }
            else if (Point == "3")
            {
                value[0] = 9003;
            }
            else if (Point == "4")
            {
                value[0] = 9004;
            }
            else if (Point == "6")
            {
                value[0] = 0;
            }
            else if (Point == "7")
            {
                ab = 0;
                abs = 5;
                value = new short[5];
                value[0] = 9001;
                value[1] = 23;
                value[2] = 0;
                value[3] = 0;
                value[4] = 0;
            }
            else if (Point == "11")
            {
                SendArr = (byte)2;
                ab = 0;
                abs = 5;
                value = new short[5];
                value[0] = 9001;
                value[1] = 12;
                value[2] = 04;
                value[3] = 3007;
                value[4] = 0;
            }
            else if (Point == "10")
            {
                for (int i = 2; i < 10; i++)
                {
                    SendArr = (byte)i;
                    ab = 0;
                    abs = 5;
                    value = new short[5];
                    value[0] = (byte)(i + DateTime.Now.Second);
                    value[1] = 15;
                    value[2] = 2004;
                    value[3] = 3007;
                    value[4] = 0;
                    _JTQModBus.SendFc16(SendArr, ab, abs, value, ref Result);
                    Thread.Sleep(10);
                }
                return;
            }
            else if (Point == "12")
            {
                SendArr = (byte)1;
                ab = 1;
                value[0] = 12;
            }
            else if (Point.IndexOf("12_") > -1)//目的地设置
            {
                SendArr = (byte)1;
                ab = 1;
                value[0] = short.Parse(Point.Replace("12_", ""));
            }
            else if (Point.IndexOf("13_") > -1)//装货卸货
            {
                SendArr = (byte)1;
                ab = 9;
                value[0] = short.Parse(Point.Replace("13_", ""));
            }
            _JTQModBus.SendFc16(SendArr, ab, abs, value, ref Result);
            Thread.Sleep(10);

        }

        public void SendFuntion(object  _Point)
        {
            _queSend.Enqueue(_Point);
        }

        public void SetFW()
        {
            byte SendArr = (byte)1;
            string Result = "";
            ushort ab = 0;
            ushort abs = 7;
            short[] value = new short[7];
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = 0;
            }
            _JTQModBus.SendFc16(SendArr, ab, abs, value, ref Result);
            Thread.Sleep(100);
        }
        public void SendData()
        {
            while (true)
            {  
                SendAllID();
            }
        }

        public void SendAllID()
        {
            if (_AGVManager != null)
            {
                try
                {
                    GetModbusItme _GetModbusItme;
                    foreach (AGVManager Temp in _AGVManager)
                    {
                        //优先处理指令
                        if (_queSend.Count > 0)
                        {
                            object _objc = _queSend.Dequeue();
                            Thread.Sleep(10);
                            if (_objc is SetModbusItme)
                            {
                                SetModbusItme _ModBus = (SetModbusItme)_objc;
                                string Results = "";
                                _JTQModBus.SendFc16((byte)_ModBus.DevID, (ushort)_ModBus.Begin, (ushort)_ModBus.EndCount, _ModBus.Reshort, ref Results);
                            }
                            else
                            {
                                SendFuntion(_objc.ToString());
                            }
                        }
                        Thread.Sleep(100);
                        _GetModbusItme = Temp.GModbusItme;
                        byte SendArr = (byte)Temp.AGVID;
                        _GetModbusItme.DevID = Temp.AGVID;
                        string Result = "";
                        _JTQModBus.SendFc03(SendArr, 0, (ushort)_GetModbusItme.Registers, ref _GetModbusItme.ResUshort, ref Result);
                        foreach (Control obj in tabControl1.TabPages[0].Controls)
                        {
                            if (obj is UCKGS)
                            {
                                ((UCKGS)obj).GetContext(_GetModbusItme);
                            }
                            //if (_tmepUI)
                            //{
                            //    if (obj is UIShow)
                            //    {
                            //        ((UIShow)obj).GetContext(_GetModbusItme);
                            //    }
                            //}
                            //else
                            //{
                            //    if (obj is UCKG)
                            //    {
                            //        ((UCKG)obj).GetContext(_GetModbusItme);
                            //    }
                            //}
                        }
                        foreach (Control obj in tabControl1.TabPages[1].Controls)
                        {
                            if (obj is UIControl)
                            {
                                ((UIControl)obj).GetContext(_GetModbusItme);
                            }
                        }
                    }
                }
                catch (Exception el)
                {
                    LogManager.WriteLog("ErrMsg", DateTime.Now.ToString() + ">>" + el.Message.ToString());
                }
            }
        }
        #endregion
    }
}
