using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using ModelData;
using System.Net.Sockets;
using IniFileRobot;
using System.Collections;
using System.Threading;
using OPCAutomation;
using System.Net;
using WinRobots.Comm;
using WinRobots.UComTral;
using System.IO.Ports;


namespace WinRobots
{
    public partial class UState : UserControl
    {

        #region 私有变量
        /// <summary>
        /// OPCServer Object
        /// </summary>
        OPCServer KepServer;
        /// <summary>
        /// OPCGroups Object
        /// </summary>
        OPCGroups KepGroups;
        /// <summary>
        /// OPCGroup Object
        /// </summary>
        OPCGroup KepGroup;
        /// <summary>
        /// OPCItems Object
        /// </summary>
        OPCItems KepItems;
        /// <summary>
        /// OPCItem Object
        /// </summary>
        OPCItem KepItem;
        /// <summary>
        /// 主机IP
        /// </summary>
        string strHostIP = "";
        /// <summary>
        /// 主机名称
        /// </summary>
        string strHostName = "";
        /// <summary>
        /// 连接状态
        /// </summary>
        bool opc_connected = false;
        /// <summary>
        /// 客户端句柄
        /// </summary>
        int itmHandleClient = 0;
        /// <summary>
        /// 服务端句柄
        /// </summary>
        int itmHandleServer = 0;
        /// <summary>
        /// 服务端端口
        /// </summary>
        int _intPort = 8083;

        /// <summary>
        /// opc名称
        /// </summary>
        string TxtServerName = "";

        /// <summary>
        /// opc地址
        /// </summary>
        string TxtServerIP = "";
        /// <summary>
        /// PLC目录
        /// </summary>
        ArrayList Alist ;
        /// <summary>
        /// 读去OPC数据
        /// </summary>
        private delegate void ReadData();//读OPC

        private delegate void WriteData();//写心跳数据

        private delegate void WritePLData();//写控制流程数据

        private delegate void WriteBackData();//写回源点控制流程
        #endregion


        #region 设置OPC 
        /// <summary>
        /// 枚举本地OPC服务器
        /// </summary>
        private void GetLocalServer()
        {
            //获取本地计算机IP,计算机名称
            IPHostEntry IPHost = Dns.Resolve(Environment.MachineName);
            if (IPHost.AddressList.Length > 0)
            {
                strHostIP = IPHost.AddressList[0].ToString();
            }
            else
            {
                return;
            }
            //通过IP来获取计算机名称，可用在局域网内
            IPHostEntry ipHostEntry = Dns.GetHostByAddress(strHostIP);
            strHostName = ipHostEntry.HostName.ToString();
            //获取本地计算机上的OPCServerName
            try
            {
                KepServer = new OPCServer();
                object serverList = KepServer.GetOPCServers(strHostName);
                ArrayList _list = new ArrayList();
                foreach (string turn in (Array)serverList)
                {
                    _list.Add(turn);
                }
                TxtServerName = _list[0].ToString();//虚拟
            }
            catch (Exception err)
            {
                MessageBox.Show("枚举本地OPC服务器出错：" + err.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        /// <summary>
        /// 创建组
        /// </summary>
        private bool CreateGroup()
        {
            try
            {
                KepGroups = KepServer.OPCGroups;
                KepGroup = KepGroups.Add("OPCDOTNETGROUP");
                SetGroupProperty();
                KepItems = KepGroup.OPCItems;
            }
            catch (Exception err)
            {
                MessageBox.Show("创建组出现错误：" + err.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置组属性
        /// </summary>
        private void SetGroupProperty()
        {
            KepServer.OPCGroups.DefaultGroupIsActive = true;
            KepServer.OPCGroups.DefaultGroupDeadband =0;
            KepGroup.UpdateRate = 250;
            KepGroup.IsActive = true;
            KepGroup.IsSubscribed = true;
        }

        /// <summary>
        /// 列出OPC服务器中所有节点
        /// </summary>
        /// <param name="oPCBrowser"></param>
        private void RecurBrowse(OPCBrowser oPCBrowser)
        {
            //展开分支
            oPCBrowser.ShowBranches();
            //展开叶子
            oPCBrowser.ShowLeafs(true);
            Alist = new ArrayList();
            foreach (object turn in oPCBrowser)
            {
                if (turn.ToString().IndexOf("H100") > -1)
                {
                    continue;
                }
                Alist.Add(turn.ToString());
            }
        }
        //初始化OPC
        public void initOpcConn()
        {
            try
            {
                GetLocalServer();
                TxtServerIP = _IniConfig.ReadString("Config", "SeverOPCIP", "");
                if (!ConnectRemoteServer(TxtServerIP, TxtServerName))
                {
                    return;
                }
                Thread.Sleep(100);
                opc_connected = true;
                RecurBrowse(KepServer.CreateBrowser());
                Thread.Sleep(100);
                if (!CreateGroup())
                {
                    return;
                }

                ReadData _ReadData = new ReadData(OpenUpdate);
                _ReadData.BeginInvoke(null, null);


                WritePLData _WritePLData = new WritePLData(WriteLCData);
                _WritePLData.BeginInvoke(null, null);


                WriteBackData _WriteBackData = new WriteBackData(BackPoint);
                _WriteBackData.BeginInvoke(null, null);
               // initopc();
                InitComDataSet();
                if (_isCommOrOPC)
                {
                    WriteData _WriteData = new WriteData(ValSet);
                    _WriteData.BeginInvoke(null, null);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("初始化出错：" + err.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void ReadTcpObj(object obj)
        {
            if (obj is RobotData)
            {
                RobotData _RobotData = obj as RobotData;
                if (_RobotData.ListVal.Length > 1)
                {
                    this.SetDataItemValue(_RobotData.ListVal[0], _RobotData.ListVal[1]);
                }
            }
        }

        int Cuplc = 0;
        public void ValSet()
        {
            Thread.Sleep(500);
            while (true)
            {
                Thread.Sleep(300);
                Cuplc++;
                this.SetDataItemValue("MWSMART.AGV.H31PC心跳", Cuplc.ToString());
                if (Cuplc > 20)
                {
                    Cuplc = 0;
                }
                
            }
        }

        /// <summary>
        /// 连接OPC服务器
        /// </summary>
        /// <param name="remoteServerIP">OPCServerIP</param>
        /// <param name="remoteServerName">OPCServer名称</param>
        private bool ConnectRemoteServer(string remoteServerIP, string remoteServerName)
        {
            try
            {
                KepServer.Connect(remoteServerName, remoteServerIP);
                if (KepServer.ServerState == (int)OPCServerState.OPCRunning)
                {
                    this.Text = "已连接到-" + KepServer.ServerName + "   ";
                }
                else
                {
                    //这里你可以根据返回的状态来自定义显示信息，请查看自动化接口API文档
                    this.Text = "状态：" + KepServer.ServerState.ToString() + "   ";
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("连接远程服务器出现错误：" + err.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
             
                return false;
            }
            return true;
        }
        //设置参数
        public void SetDataItemValue(string ItemID, object value)
        {
            if (opc_connected)
            {
                try
                {
                    OPCItem item = KepGroup.OPCItems.AddItem(ItemID, 1);
                    item.Write(value);
                    Array removeServerHandle = (Array)(new int[2] { 0, item.ServerHandle });
                    Array removeErrors;
                    KepGroup.OPCItems.Remove(1, ref removeServerHandle, out removeErrors);

                }
                catch (Exception)
                {

                }
            }
        }


        public void OpenUpdate()
        {
            while (true)
            {
                try
                {
                    RobotData _RobotData = new RobotData();
                    _RobotData.ListVal = new string[strcode.Length];
                    int i = 0;
                    int f = 0;
                    foreach (string strl in Alist)
                    {
                        if (GetisFun(strl))
                        {

                            object ltem = GetDataItemValue(strl).DataValue;
                            if (ltem != null)
                            {
                                _RobotData.ListVal[i] = strl.Trim() + "#" + ltem.ToString();
                            }
                            else
                            {
                                f++;
                            }
                            i++;
                            Thread.Sleep(10);

                        }
                    }
                    if (f > 4)
                    {
                     
                        continue;
                    }

                    UpdateData(_RobotData);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message.ToString());
                }
            }
        }

        public string[] strcode = new string[] { "H62", "H63", "H70", "H39", "H37", "H19", "H31", "H46", "H15", "H48", "H22", "H27", "H28", "H106" };

        public bool GetisFun(string Val)
        {
            foreach (string strlc in strcode)
            {
                if (Val.IndexOf(strlc) > -1)
                {
                    return true;
                }
            }
            return false;
        }
        //初始化写入
        public void initopc()
        {
            _sendobj.ListVal = new string[2];
            foreach (string skey in _DataHashtable.Keys)
            {
                _sendobj.ListVal[0] = _DataHashtable[skey].ToString();
                _sendobj.ListVal[1] = skey;
                SendCmd();
                Thread.Sleep(100);
            }
        }

        public OPCDataItemValue GetDataItemValue(string ItemID)
        {
            if (!opc_connected) return null;
            try
            {
                OPCItem item = KepGroup.OPCItems.AddItem(ItemID, 1);
                Object value;
                Object quality;
                Object timestamp;
                item.Read((short)OPCDataSource.OPCDevice, out value, out quality, out timestamp);
                OPCDataItemValue itemValue = new OPCDataItemValue();
                itemValue.DataValue = value;
                itemValue.TimeStamp = (DateTime)timestamp;
                itemValue.Quality = Convert.ToInt32(quality);

                Array removeServerHandle = (Array)(new int[2] { 0, item.ServerHandle });
                Array removeErrors;
                KepGroup.OPCItems.Remove(1, ref removeServerHandle, out removeErrors);

                return itemValue;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public class OPCDataItemValue
        {
            object _DataValue;
            DateTime _TimeStamp;
            int _Quality;

            public object DataValue
            {
                get { return _DataValue; }
                set { _DataValue = value; }
            }
            public DateTime TimeStamp
            {
                get { return _TimeStamp; }
                set { _TimeStamp = value; }
            }
            public int Quality
            {
                get { return _Quality; }
                set { _Quality = value; }
            }
        }
        #endregion

        DataTable _DataTable;
        public Action<object> _SendData;
        frmShow _frmShow;
        Hashtable _Hashtable;
        Hashtable _DataHashtable;
        IniFiles _IniFiles;
        IniFiles _IniConfig;
        IniFiles _InitDataConfig;


        public Action<string> _SetPoint;

        public Action<string> _SetPLC;

        public bool _isCommOrOPC = false;//ture OPC flase comm;
        public UState() 
        {
            InitializeComponent();
           
            initTable();
            initHT();
            //_DataTable = dataGridView1.DataSource as DataTable;
            _sendobj = new RobotData();
            _IniFiles = new IniFiles(Application.StartupPath + "/CCmd.ini");
            _IniConfig = new IniFiles(Application.StartupPath + "/Config.ini");
            _InitDataConfig = new IniFiles(Application.StartupPath + "/CCmdData.ini");
            _Hashtable = _IniFiles.ReadSectionValues("Config");
            _DataHashtable = _InitDataConfig.ReadSectionValues("Config");
            SetColor();

            //运行状态
            this.comboBox2.SelectedIndex = 0;
        }


        private void robot1_Click(object sender, EventArgs e)
        {

        }

        private void UState_Load(object sender, EventArgs e)
        {
            //if (_isCommOrOPC)
            //{
            //    initOpcConn();
            //    //utcPclient1.initConnt();
            //    //utcPclient1._WriteList = new Action<string>(SetIPlist);
            //    //utcPclient1._ReadMsg = new Action<object>(ReadTcpObj);
            //}
            //else
            //{
            //    initComm();
            //}

        }

        public void SetIPlist(string IP)
        {
            string[] tempip = IP.Split('_');
            //if (tempip[0] == "B")
            //{
            //    this.listBox2.Items.Add(tempip[1]);
            //}
            //else
            //{
            //    this.listBox2.Items.Remove(tempip[1]);
            //}
        }
        //che
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
        public void initTable()
        {
            //DataTable _datatable = new DataTable();
            //_datatable.Columns.Add("直行速度");
            //_datatable.Columns.Add("弯道速度");  
            //_datatable.Columns.Add("机器人当前指令");
            //_datatable.Columns.Add("使能指令");
            //_datatable.Columns.Add("电池电压");
            //_datatable.Columns.Add("PC心跳");
            //_datatable.Columns.Add("PLC心跳");
            //_datatable.Columns.Add("目的地地标");
            //_datatable.Columns.Add("当前坐标");
            //_datatable.Columns.Add("当前方位");

            
            //DataRow Dr = _datatable.NewRow();
            //Dr["弯道速度"] = "0";
            //Dr["直行速度"] = "0";
            //Dr["使能指令"] = "0";
            //Dr["机器人当前指令"] = "0";
            //Dr["电池电压"] = "0";
            //Dr["PC心跳"] = "0";
            //Dr["PLC心跳"] = "0";
            //Dr["目的地地标"] = "0";
            //Dr["当前坐标"] = "0";
            //Dr["当前方位"] = "0";
            //_datatable.Rows.Add(Dr);
            //this.dataGridView1.DataSource = _datatable;
        }

        //设置任务型
        public void SetTask(string Task)
        {
            string[] temps = Task.Split('_');
            string sll = temps[temps.Length - 1];
            txtgopoint.Text = _Fhts[sll].ToString();
            if (Task.IndexOf("14_9_6") > -1)
            {
                if (!_isCommOrOPC)
                {
                    if (this.txtdqfw.Text.Trim() == "1")
                    {
                        this.SetComm(1,"3041_1017", _Fhts[sll].ToString());
                    }
                    else if (this.txtdqfw.Text.Trim() == "4")
                    {
                        this.SetComm(4, "3041_4017", _Fhts[sll].ToString());
                    }
                    return;
                }
                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                _sendobj.ListVal[1] = "1";
                SendCmd();

                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("目的地地标");
                _sendobj.ListVal[1] = _Fhts[sll].ToString();
                SendCmd();
                if (this.txtdqfw.Text.Trim() == "1")
                {
                   
                    SendCmdText("前进");
                    Thread.Sleep(10);
                    SetCKCall("3041_1017");
                }
                else if (this.txtdqfw.Text.Trim() == "4")
                {
                    SendCmdText("后退");
                    Thread.Sleep(10);
                    SetCKCall("3041_4017");
                }
            }
            else if (Task.IndexOf("6_9_14") > -1)
            {
                if (!_isCommOrOPC)
                {
                    if (this.txtdqfw.Text.Trim() == "1" || this.txtdqfw.Text.Trim() == "3")
                    {
                        this.SetComm(1,"2017_1041", _Fhts[sll].ToString());
                    }
                    else if (this.txtdqfw.Text.Trim() == "4" || this.txtdqfw.Text.Trim() == "2")
                    {
                        this.SetComm(4,"2017_4041", _Fhts[sll].ToString());
                    }
                    return;
                }
                fxVal = false;
                Thread.Sleep(100);

                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                _sendobj.ListVal[1] = "1";
                SendCmd();

                _begin = true;
                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("目的地地标");
                _sendobj.ListVal[1] = _Fhts[sll].ToString();
                SendCmd();
                if (this.txtdqfw.Text.Trim() == "1" || this.txtdqfw.Text.Trim() == "3")
                {
                    SendCmdText("前进");
                    Thread.Sleep(10);
                    SetCKCall("2017_1041");
                }
                else if (this.txtdqfw.Text.Trim() == "4" || this.txtdqfw.Text.Trim() == "2")
                {
                    SendCmdText("后退");
                    Thread.Sleep(10);
                    SetCKCall("2017_4041");
                }
                //if(
            }
            else if (Task.IndexOf("9_14") > -1)
            {
                if (!_isCommOrOPC)
                {
                    if (this.txtdqfw.Text == "1" || this.txtdqfw.Text.Trim() == "2")
                    {
                        this.SetComm(1,"3026_1041", _Fhts[sll].ToString());
                    }
                    else if (this.txtdqfw.Text == "4" || this.txtdqfw.Text.Trim() == "3")
                    {
                        this.SetComm(4,"3026_4041", _Fhts[sll].ToString());
                    }
                    return;
                }
                fxVal = false;
                Thread.Sleep(100);
                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                _sendobj.ListVal[1] = "1";
                SendCmd();
               
                _begin = true;
                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("目的地地标");
                _sendobj.ListVal[1] = _Fhts[sll].ToString();
                SendCmd();
                if (this.txtdqfw.Text == "1" || this.txtdqfw.Text.Trim() == "2")
                {
                    SendCmdText("前进");
                    SetCKCall("3026_1041");
                }
                else if (this.txtdqfw.Text == "4" || this.txtdqfw.Text.Trim() == "3")
                {
                    SendCmdText("后退");
                    SetCKCall("3026_4041");
                }
                //if(
            }
            else if (Task.IndexOf("6_9") > -1)
            {
                if (!_isCommOrOPC)
                {
                    if (this.txtdqfw.Text.Trim() == "3" || this.txtdqfw.Text.Trim() == "1")
                    {
                        this.SetComm(1,"3017_1026", _Fhts[sll].ToString());
                    }
                    else if (this.txtdqfw.Text.Trim() == "2" || this.txtdqfw.Text.Trim() == "4")
                    {
                        this.SetComm(4, "3017_4026", _Fhts[sll].ToString());
                    }
                    return;
                }
                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                _sendobj.ListVal[1] = "1";
                SendCmd();
                // sll = sll+"2";

                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("目的地地标");
                _sendobj.ListVal[1] = _Fhts[sll].ToString();
                SendCmd();
                if (this.txtdqfw.Text.Trim() == "3" || this.txtdqfw.Text.Trim() == "1")
                {
                    SendCmdText("前进");
                    SetCKCall("3017_1026");
                }
                else if (this.txtdqfw.Text.Trim() == "2" || this.txtdqfw.Text.Trim() == "4")
                {
                    SendCmdText("后退");
                    SetCKCall("3017_4026");
                }

            }
            else if (Task.IndexOf("9_6") > -1)
            {
                if (!_isCommOrOPC)
                {
                    if (this.txtdqfw.Text.Trim() == "1" || this.txtdqfw.Text.Trim() == "2")
                    {
                        this.SetComm(1,"2026_1017", _Fhts[sll].ToString());
                    }
                    else if (this.txtdqfw.Text.Trim() == "4" || this.txtdqfw.Text.Trim() == "3")
                    {
                        this.SetComm(4,"2026_4017", _Fhts[sll].ToString());
                    }
                    return;
                }
                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                _sendobj.ListVal[1] = "1";
                SendCmd();
               // sll = sll+"2";
             
                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("目的地地标");
                _sendobj.ListVal[1] = _Fhts[sll].ToString();
                SendCmd();
                if (this.txtdqfw.Text.Trim() == "1" || this.txtdqfw.Text.Trim() == "2")
                {
                    SendCmdText("前进");
                    Thread.Sleep(10);
                    SetCKCall("2026_1017");
                }
                else if (this.txtdqfw.Text.Trim() == "4" || this.txtdqfw.Text.Trim() == "3")
                {
                    SendCmdText("后退");
                    Thread.Sleep(10);
                    SetCKCall("2026_4017");
                }

            }
            else if (Task.IndexOf("6_14") > -1)
            {
                if (!_isCommOrOPC)
                {
                    if (this.txtdqfw.Text.Trim() == "1" || this.txtdqfw.Text.Trim() == "2")
                    {
                        this.SetComm(1,"3026_1041", _Fhts[sll].ToString());
                    }
                    else if (this.txtdqfw.Text.Trim() == "4" || this.txtdqfw.Text.Trim() == "3")
                    {
                        this.SetComm(4,"3026_4041", _Fhts[sll].ToString());
                    }
                    return;
                }
                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                _sendobj.ListVal[1] = "1";
                SendCmd();
                // sll = sll+"2";

                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("目的地地标");
                _sendobj.ListVal[1] = _Fhts[sll].ToString();
                SendCmd();
                if (this.txtdqfw.Text.Trim() == "1" || this.txtdqfw.Text.Trim() == "2")
                {
                    SendCmdText("前进");
                    Thread.Sleep(10);
                    SetCKCall("3026_1041");
                }
                else if (this.txtdqfw.Text.Trim() == "4" || this.txtdqfw.Text.Trim() == "3")
                {
                    SendCmdText("后退");
                    Thread.Sleep(10);
                    SetCKCall("3026_4041");
                }

            }
            else if (Task.IndexOf("14_9") > -1)
            {
                if (!_isCommOrOPC)
                {
                    if (this.txtdqfw.Text == "1" || this.txtdqfw.Text.Trim() == "2")
                    {
                        this.SetComm(1,"2041_1026", _Fhts[sll].ToString());
                    }
                    else if (this.txtdqfw.Text == "4" || this.txtdqfw.Text.Trim() == "3")
                    {
                        this.SetComm(4, "2041_4026", _Fhts[sll].ToString());
                    }
                    return;
                }
                fxVal = false;
                Thread.Sleep(100);

                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                _sendobj.ListVal[1] = "1";
                SendCmd();

                _begin = true;
             
                _sendobj.ListVal = new string[2];
                _sendobj.ListVal[0] = getMuesID("目的地地标");
                _sendobj.ListVal[1] = _Fhts[sll].ToString();
                SendCmd();
                if (this.txtdqfw.Text == "1")
                {
                    SendCmdText("前进");
                    SetCKCall("2041_1026");
                }
                else if (this.txtdqfw.Text == "4")
                {
                    SendCmdText("后退");
                    SetCKCall("2041_4026");
                }
            } 

        }

        public void UpdateData(RobotData _sobj)
        {
            if (_sobj.ListVal.Length > 5)
            {
                try
                { 
                 //   utcPclient1.SendAllClient(_sobj);
                    UpdateDataOpc(_sobj.ListVal);
                    //GetUpdateData(_sobj.ListVal);
                }
                catch (Exception el)
                {
                    string sl = el.Message.ToString();
                    sl += "";
                }
            }

        }
    
        public bool blset = true;//更新一次数据库
        public void GetUpdateData(string[] Val)
        {
            if (blset)
            {
                blset = false;

                foreach (string tempstr in Val)
                {
                    if (tempstr == null) continue;
                    string[] tempstrl = tempstr.Split('#');
                    if (tempstrl.Length > 1)
                    {
                        _IniFiles.WriteString("Config", tempstrl[0].Replace("MWSMART.AGV.H", "").Trim(), tempstrl[0]);
                    }
                }
            }

        }
        System.Text.RegularExpressions.Regex rex =
        new System.Text.RegularExpressions.Regex(@"^\d+$");
        public string GetStrLen(string CRead)
        {
            string tempStr = CRead.Remove(0, 1);
            tempStr = tempStr.Substring(0, 2);
            if (rex.IsMatch(tempStr))
            {
                return CRead.Remove(0, 3);
            }
            else
            {
                return CRead.Remove(0, 2);
            }
        }

     


        public void SetProText(string skey, string val)
        {
            if (skey.IndexOf("H46") > -1)
            {
                txtWD.Text = val;
                return;
            }
            else if (skey.IndexOf("H15") > -1)
            {
                // txtjqrwz.Text = val;
                return;
            }
            else if (skey.IndexOf("H27") > -1)
            {
                TxtDLA.Text = val;
                return;
            }
            else if (skey.IndexOf("H28") > -1)
            {
                TxtDLB.Text = val;
                return;
            }
            else if (skey.IndexOf("H48") > -1)
            {

                txtZXing.Text = val;
                return;
            }
            else if (skey.IndexOf("H22") > -1)
            {

                txtPower.Text = val;
                return;
            }
            else if (skey.IndexOf("H31") > -1)
            {

                txtPC.Text = val;
                return;
            }
            else if (skey.IndexOf("H19") > -1)
            {
                if (_SetPLC != null)
                {
                    _SetPLC(val);
                }
                txtPLC.Text = val;
                return;
            }
            else if (skey.IndexOf("H37") > -1)
            {

                txtDQCMD.Text = val;
                return;
            }
            else if (skey.IndexOf("H39") > -1)
            {

                txtgopoint.Text = val;
                return;
            }
            else if (skey.IndexOf("H70") > -1)
            {

                txtdqfw.Text = val;
                return;
            }
            else if (skey.IndexOf("H62") > -1)
            {

                txtPoint.Text = val;
                return;
            }
            else if (skey.IndexOf("H10障碍物") > -1)
            {

                btnzzw.Text = bool.Parse(val) ? "障碍物无效" : "障碍物有效";
                return;
            }
            else if (skey.IndexOf("H63") > -1)
            {

                btnAuto.Text = bool.Parse(val) ?   "手动":"自动";
                return;
            }
            else if (skey.IndexOf("H106") > -1)
            {

                this.comboBox2.SelectedIndex = int.Parse(val);
                return;
            }
         
        }
      
        public void UpdateDataOpc(string[] Val)
        {
            foreach (string tempstr in Val)
            {
                if (tempstr == null) continue;
                string[] tempstrl = tempstr.Split('#');
                if (tempstrl.Length > 1)
                {
                    SetProText(tempstrl[0], tempstrl[1]);
                }
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
        #region 命令集

        RobotData _sendobj = null;
        private void SetFunction(object sender, EventArgs e)
        {
            Button _Control = sender as Button;
            _sendobj.RobotName = "robot1";
            SetColor();
            _Control.BackColor = System.Drawing.Color.LightCoral;
            if (_Control.Name != "btnAuto")
            {
                if (_Control.Text.Trim() == "手动")
                {
                    SendCmdText("手动");
                }
            }
            SendCmdText(_Control.Text);
        }

        public void PearModel(string Val)
        {
            switch (Val)
            {
                case "1"://停止
                    SendCmdText("左转");
                    break;
                case "2"://停止
                    SendCmdText("前进");
                    break;
                case "3"://停止
                    SendCmdText("右转");
                    break;
                case "4"://停止
                    SendCmdText("停止");
                    break;
                case "5"://停止
                    SendCmdText("停止");
                    break;
                case "6"://停止
                    SendCmdText("断使能");
                    break;
                case "7"://停止
                    SendCmdText("右转90度");
                    break;
                case "8"://停止
                    SendCmdText("后退");
                    break;
                case "9"://停止
                    SendCmdText("左转90度");
                    break;
            }
        }
          

        public void SendCmdText(string cmd)
        {
            switch (cmd)
            {
                case "暂停"://停止
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("手动暂停");
                    _sendobj.ListVal[1] = "1";
                    // sleepwait.Text = "开启";
                    break;

                case "开启"://停止
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("手动暂停");
                    _sendobj.ListVal[1] = "0";
                    //  sleepwait.Text = "暂停";
                    break;
                case "停止"://停止
                    if (_isCommOrOPC)
                    {
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("机器人当前指令");
                        _sendobj.ListVal[1] = "0";
                    }
                    else
                    {
                        SetCMD(0);
                        CmdInt = 0;
                    }
                    break;
                case "前进"://前进
                    if (_isCommOrOPC)
                    {
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("机器人当前指令");
                        _sendobj.ListVal[1] = "9001";
                    }
                    else
                    {
                        SetCMD(1);
                        CmdInt = 1;
                    }
                    break;
                case "左转"://后退
                    if (_isCommOrOPC)
                    {
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("机器人当前指令");
                        _sendobj.ListVal[1] = "9002";
                    }
                    else
                    {
                        SetCMD(2);
                        CmdInt = 2;
                    }
                    break;
                case "右转"://左转90
                    if (_isCommOrOPC)
                    {
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("机器人当前指令");
                        _sendobj.ListVal[1] = "9003";
                    }
                    else
                    {
                        SetCMD(3);
                        CmdInt = 3;
                    }
                    break;
                case "后退"://右转90
                    if (_isCommOrOPC)
                    {
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("机器人当前指令");
                        _sendobj.ListVal[1] = "9004";
                    }
                    else
                    {
                        SetCMD(4);
                        CmdInt = 4;
                    }

                    break;
                case "回零"://停止
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("机器人当前指令");
                    _sendobj.ListVal[1] = "9000";


                    break;
                case "左调头"://左调头
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("机器人当前指令");
                    _sendobj.ListVal[1] = "9005";
                    break;
                case "右调头"://右调头
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("机器人当前指令");
                    _sendobj.ListVal[1] = "9006";
                    break;
                case "左旋转"://左旋转
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("机器人当前指令");
                    _sendobj.ListVal[1] = "9007";
                    break;
                case "右旋转"://右旋转
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("机器人当前指令");
                    _sendobj.ListVal[1] = "9008";
                    break;

                case "开使能"://右旋转
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("使能指令");
                    _sendobj.ListVal[1] = "1";
                    break;

                case "断使能"://右旋转
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("使能指令");
                    _sendobj.ListVal[1] = "0";
                    break;

                case "方位"://右旋转
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("H70方位");
                    _sendobj.ListVal[1] = txtFw.Text.Trim();
                    break;

                case "正常速度设定"://右旋转
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("正常速度设定");
                    _sendobj.ListVal[1] = "500";
                    break;
                case "关机"://右旋转
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("远程关机");
                    _sendobj.ListVal[1] = "1";
                    break;
                case "复位"://右旋转
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("复位指令");
                    _sendobj.ListVal[1] = "1";
                    break;

                case "障碍物有效":
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("障碍物停机有效");
                    _sendobj.ListVal[1] = "1";

                    break;
                case "障碍物无效":
                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("障碍物停机有效");
                    _sendobj.ListVal[1] = "0";

                    break;
                case "设置":

                    _sendobj.ListVal = new string[2];
                    _sendobj.ListVal[0] = getMuesID("弯道速度指令");
                    _sendobj.ListVal[1] = txtwdsd.Text.Trim();
                    SendCmd();
                    Thread.Sleep(100);
                    _sendobj.ListVal[0] = getMuesID("H94档位的选择");
                    if (this.comboBox3.Text.Trim().Length==0)
                    {
                        this.comboBox3.SelectedIndex = 0;
                    }
                    _sendobj.ListVal[1] = this.comboBox3.Text.Trim();
                    break;

                case "进仓库":
                 
                    break;
                case "手动":
                     _sendobj.ListVal = new string[2];
                     _sendobj.ListVal[0] = getMuesID("H63手动自动");
                     _sendobj.ListVal[1] ="0";
                    break;
                case "自动":
                     _sendobj.ListVal = new string[2];
                     _sendobj.ListVal[0] = getMuesID("H63手动自动");
                     _sendobj.ListVal[1] ="1";
                    break;
            }
            if (_isCommOrOPC)
            {
                SendCmd();
            }
        
        }

        



        public void SetColor()
        {
            foreach (Control Con in this.Controls)
            {
                if (Con is Button)
                {
                    Con.BackColor = System.Drawing.Color.RoyalBlue;
                }
            }
        }

        public void initRead()
        {
            //_IniFiles.ReadSectionValues("Config",NameValueCollection 
        }

        public string getMuesID(string ID)
        {
            return _IniFiles.ReadString("Config", ID, "");
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
          //  dataGridView1.Update();
        }
     
        private void button8_Click(object sender, EventArgs e)
        {
            if (_obj == null)
            {
                return;
            }
            _frmShow = new frmShow() ;
            if (_frmShow.ShowDialog() == DialogResult.OK)
            {
                ((Control)_obj).Text = _frmShow.Text;
            }
        }
        object _obj;
        private void hightspeed_MouseDown(object sender, MouseEventArgs e)
        {
            _obj = sender;
        }

        private void CFunction(object sender, EventArgs e)
        {

            Button _Control = sender as Button;
            if (_sendobj != null)
            {
                _sendobj.RobotName = "robot1";
                SetColor();
                _Control.BackColor = System.Drawing.Color.LightCoral;
                switch (_Control.Text)
                {
                    case "设置"://停止
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("弯道速度指令");
                        _sendobj.ListVal[1] = bend.Text.Trim();
                        SendCmd();
                        Thread.Sleep(100);
                        _sendobj.ListVal[0] = getMuesID("正常速度指令");
                        _sendobj.ListVal[1] = comboBox1.Text.Trim();
                        break;
                    case "方位1"://停止
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("激光光束指向指令");
                        _sendobj.ListVal[1] = "1";
                        break;
                    case "方位2"://停止
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("激光光束指向指令");
                        _sendobj.ListVal[1] = "2";
                        break;
                    case "方位3"://停止
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("激光光束指向指令");
                        _sendobj.ListVal[1] = "3";
                        break;
                    case "方位4"://停止
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("激光光束指向指令");
                        _sendobj.ListVal[1] = "4";
                        break;
                        //AGV控制
                    case "呼叫"://呼叫
                      
                        break;

                    case "生成命令集"://生成命令集
                    
                        return;
                    case "发送命令集"://生成命令集

                        return;

                }
                SendCmd();
            }

        }

        public void CreateCmd()
        {

        }
        public void SendComCMD()
        {

        }

        public void SendCmd()
        {
            this.SetDataItemValue(_sendobj.ListVal[0],_sendobj.ListVal[1]);
        }
            
      

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _sendobj.ListVal = new string[2];
            string sl = getMuesID("激光旋转速度");
            _sendobj.ListVal[0] = sl;
            _sendobj.ListVal[1] = txtxzsd.Text.Trim();
            SendCmd();
            Thread.Sleep(50);

            _sendobj.ListVal[0] = getMuesID("激光指向方位1的位置");
            _sendobj.ListVal[1] = txtfw1.Text.Trim();
            SendCmd();
            Thread.Sleep(50);
            _sendobj.ListVal[0] = getMuesID("激光指向方位2的位置");
            _sendobj.ListVal[1] = txtfw2.Text.Trim();
            SendCmd();
            Thread.Sleep(50);
            _sendobj.ListVal[0] = getMuesID("激光指向方位3的位置");
            _sendobj.ListVal[1] = txtfw3.Text.Trim();
            SendCmd();
            Thread.Sleep(50);
            _sendobj.ListVal[0] = getMuesID("激光指向方位4的位置");
            _sendobj.ListVal[1] = txtfw4.Text.Trim();
            SendCmd();
            Thread.Sleep(50);


            _sendobj.ListVal[0] = getMuesID("X坐标方位");
            _sendobj.ListVal[1] = "3";
            SendCmd();
            Thread.Sleep(50);
            _sendobj.ListVal[0] = getMuesID("Y坐标方位");
            _sendobj.ListVal[1] = "1";
            SendCmd();
            Thread.Sleep(50);

            _sendobj.ListVal[0] = getMuesID("激光旋转命令触发");
            _sendobj.ListVal[1] = "1";
            SendCmd();
            Thread.Sleep(50);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // string tell = GetDataItemValue(getMuesID("激光旋转命令触发")).DataValue.ToString();
            _sendobj.ListVal = new string[2];
            _sendobj.ListVal[0] = getMuesID("激光旋转命令触发");
            _sendobj.ListVal[1] = "1";
            SendCmd();

        }
        //
        private void button10_Click(object sender, EventArgs e)
        {
            _sendobj.ListVal = new string[2];
            _sendobj.ListVal[0] = getMuesID("激光示教相对值");
            _sendobj.ListVal[1] = txtjg.Text.Trim();
            SendCmd();
        }
        //保存值

        private void button11_Click(object sender, EventArgs e)
        {
            _sendobj.ListVal = new string[2];
            _sendobj.ListVal[0] = getMuesID("激光指向方位1的位置");
            _sendobj.ListVal[1] = txtBjdj.Text.Trim();
            SendCmd();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            _sendobj.ListVal = new string[2];
            _sendobj.ListVal[0] = getMuesID("激光指向方位2的位置");
            _sendobj.ListVal[1] = txtBjdj.Text.Trim();
            SendCmd();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            _sendobj.ListVal = new string[2];
            _sendobj.ListVal[0] = getMuesID("激光指向方位3的位置");
            _sendobj.ListVal[1] = txtBjdj.Text.Trim();
            SendCmd();
         
        }

        private void button14_Click(object sender, EventArgs e)
        {
            _sendobj.ListVal = new string[2];
            _sendobj.ListVal[0] = getMuesID("激光指向方位4的位置");
            _sendobj.ListVal[1] = txtBjdj.Text.Trim();
            SendCmd();
          
        }
      
        //保存方向值
        private void button15_Click(object sender, EventArgs e)
        {
            string sdate = DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            string sqlfx = "INSERT INTO 坐标编号  VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
         

            LogManager.WriteLog("sql" + sdate, sqlfx);
            MessageBox.Show("保存成功！");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            _sendobj.ListVal = new string[2];
            _sendobj.ListVal[0] = getMuesID("示教");
            _sendobj.ListVal[1] = "1";
            SendCmd();
        }

        //x方位
        private void button19_Click(object sender, EventArgs e)
        {
            _sendobj.ListVal = new string[2];
            _sendobj.ListVal[0] = getMuesID("X坐标方位");
            // _sendobj.ListVal[1] = txtXfw.Text;
            SendCmd();
        }
        //y方位
        private void button18_Click(object sender, EventArgs e)
        {
            _sendobj.ListVal = new string[2];
            _sendobj.ListVal[0] = getMuesID("Y坐标方位");
            SendCmd();

        }

        string Localpoint;

        private void SetFuncitons(object sender, EventArgs e)
        {
            Button _Control = sender as Button;
            if (_sendobj != null)
            {
                _sendobj.RobotName = "robot1";

                switch (_Control.Text)
                {
                    case "机器人位置编号"://停止
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("机器人位置编号");
                        // _sendobj.ListVal[1] = txtjqrwz.Text.Trim();
                        SendCmd();
                        break;
                    case "目的地设置"://停止
                        string tempsend = txtmdd.Text.Trim();
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("目的地地标");
                        _sendobj.ListVal[1] = txtmdd.Text.Trim();
                        SendCmd();
                        //SendCmdText("前进");
                        break;

                    case "命令执行"://停止
                        string ben = "";
                        if (_isCommOrOPC)
                        {
                            ben = GetPLCData(getMuesID("H62RFID地标编码"));
                            txtdqfw.Text = GetPLCData(getMuesID("H70方位"));
                        }
                        else
                        {
                            ben = txtPoint.Text.Trim();
                        }
                        Localpoint = "0";
                        if (listBox1.SelectedIndex != -1)
                        {

                            if (listBox1.SelectedIndex == 0)
                            {
                                Localpoint = "12";
                            }
                            else if (listBox1.SelectedIndex == 1)
                            {
                                Localpoint = "10";
                            }
                            else if (listBox1.SelectedIndex == 2)
                            {
                                Localpoint = "5";
                            }

                            if (ben == "5" && Localpoint == "12")
                            {
                                if (txtdqfw.Text == "4")
                                {
                                    fxVal = false;
                                }
                                _Alist.Enqueue("0");
                            }
                            else if (ben == "12" && Localpoint == "5")
                            {
                                if (txtdqfw.Text == "3")
                                {
                                    fxVal = false;
                                }
                                _Alist.Enqueue("1");
                            }
                            else if (ben == "12" && Localpoint == "10")
                            {
                                if (txtdqfw.Text == "3")
                                {
                                    fxVal = false;
                                }
                                _Alist.Enqueue("2");
                            }
                            else if (ben == "5" && Localpoint == "10")
                            {
                                if (txtdqfw.Text == "4")
                                {
                                    fxVal = false;
                                }
                                _Alist.Enqueue("3");
                            }
                            else if (ben == "10" && Localpoint == "12")
                            {
                                if (txtdqfw.Text == "4")
                                {
                                    fxVal = false;
                                }
                                _Alist.Enqueue("4");
                            }
                            else if (ben == "10" && Localpoint == "5")
                            {
                                if (txtdqfw.Text == "4")
                                {
                                    fxVal = false;
                                }
                                _Alist.Enqueue("5");
                            }

                            if (_isCommOrOPC)
                            {
                                _begin = true;
                                _sendobj.ListVal = new string[2];
                                _sendobj.ListVal[0] = getMuesID("目的地地标");
                                _sendobj.ListVal[1] = Localpoint;
                                SendCmd();

                              
                            }
                            else
                            {
                                //string sl = _Alist.Dequeue().ToString();
                                //SetComm(lentr[int.Parse(sl)], Localpoint);

                            }

                        }

                        break;

                    case "清理执行"://停止
                        SetClier();
                        break;

                    case "流水线1到流水线2":  
                        fxVal = false;
                        Thread.Sleep(100);

                          _sendobj.ListVal = new string[2];
                                _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                                _sendobj.ListVal[1] ="1";
                                SendCmd();

                        _Alist.Enqueue("0");
                     

                        if (_isCommOrOPC)
                        {
                            _begin = true;
                            _sendobj.ListVal = new string[2];
                            _sendobj.ListVal[0] = getMuesID("目的地地标");
                            _sendobj.ListVal[1] = _Fhts["8"].ToString();
                            SendCmd();

                            SendCmdText("前进");


                        }
                        else
                        {

                          //  SetComm(lentr[1], Localpoint);

                        }
                    
                        break;
                    case "流水线2到流水线1":
                       

                        if (_isCommOrOPC)
                        {
                            fxVal = false;

                            _sendobj.ListVal = new string[2];
                            _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                            _sendobj.ListVal[1] = "1";
                            SendCmd();

                            _sendobj.ListVal = new string[2];
                            _sendobj.ListVal[0] = getMuesID("目的地地标");
                            _sendobj.ListVal[1] = _Fhts["13"].ToString();
                            SendCmd();
                            Thread.Sleep(100);
                            SendCmdText("后退");
                            _Alist.Enqueue("1");
                            _begin = true;
                        }
                        else
                        {
                          //  SetComm(lentr[0], Localpoint);
                        }
                        break;


                    case "成品区到流水线2":
                        fxVal = false;
                        Thread.Sleep(100);

                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                        _sendobj.ListVal[1] = "1";
                        SendCmd();

                        _Alist.Enqueue("3");
                        _begin = true;
                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("目的地地标");
                        _sendobj.ListVal[1] = _Fhts["8"].ToString();
                        SendCmd();

                        SendCmdText("前进");

                        break;
                    case "流水线2到成品区":
                        fxVal = false;

                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("AGV的运动状态");
                        _sendobj.ListVal[1] = "1";
                        SendCmd();

                        _sendobj.ListVal = new string[2];
                        _sendobj.ListVal[0] = getMuesID("目的地地标");
                        _sendobj.ListVal[1] = _Fhts["1"].ToString();
                        SendCmd();
                        Thread.Sleep(100);
                        SendCmdText("后退");
                        _Alist.Enqueue("2");
                        _begin = true;
                        break;


                    case "回源点"://停止
                        string Sendben = GetPLCData(getMuesID("H62RFID地标编码"));

                        txtdqfw.Text = GetPLCData(getMuesID("H70方位"));
                        if (txtdqfw.Text == "4")
                        {
                            SendCmdText("前进");
                        }
                        if (txtdqfw.Text == "1")
                        {
                            SendCmdText("后退");
                        }
                        if (txtdqfw.Text == "2")
                        {
                            SendCmdText("后退");
                        }
                        if (txtdqfw.Text == "3")
                        {
                            SendCmdText("前进");
                        }
                        _beginBack = false;
                        Sendben = _ComData.GetModelIDs(Sendben);
                        if (Sendben.Trim() != "")
                        {
                            _BackAlist.Enqueue(planner.Paln(_ComData.nodeList, "A", Sendben));
                        }

                        break;
                }
            }


        }

        #region AGV控制

        Queue _Alist = new Queue();
        Queue _BackAlist = new Queue();//回指令
        public bool _begin = true;
        public bool _beginBack = true;
        string _work = "";

        ArrayList aplc = new ArrayList();
        string DQCmd = "";
        Hashtable _hs = new Hashtable();
        Hashtable _Rhs = new Hashtable();
        Hashtable _Rihs = new Hashtable();
        Hashtable _RFhs = new Hashtable();
        Hashtable _Rshs = new Hashtable();

        Hashtable _RFihs = new Hashtable();
        ArrayList _lsa = new ArrayList();

        public bool fxVal = true;//正反方向

        string[] lentr; 
        public void WriteLCData()
        {
            lentr = new string[6];
            _hs.Add("13", "前进");
            _hs.Add("14", "左转");
            _hs.Add("9", "前进");
            _hs.Add("8", "停止");
            _lsa.Add(_hs);

            lentr[0] = "2041_1026";

            _Rhs.Add("8", "后退");
            _Rhs.Add("9", "右转");
            _Rhs.Add("14", "后退");
            _Rhs.Add("13", "停止");
            _lsa.Add(_Rhs);

            lentr[1] = "3026_4041";

            _Rihs.Add("8", "后退");
            _Rihs.Add("9", "左转");
            _Rihs.Add("6", "后退");
            _Rihs.Add("5", "后退");
            _Rihs.Add("4", "后退");
            _Rihs.Add("3", "后退");
            _Rihs.Add("1", "停止");
            _lsa.Add(_Rihs);

            lentr[2] = "2026_4017";

            _RFhs.Add("1", "前进");
            _RFhs.Add("2", "前进");
            _RFhs.Add("3", "前进");
            _RFhs.Add("4", "左转");
            _RFhs.Add("9", "前进");
            _RFhs.Add("13", "停止");
            _lsa.Add(_RFhs);
            lentr[3] = "3017_1026";

            _Rshs.Add("10", "前进");
            _Rshs.Add("9", "左转");
            _Rshs.Add("11", "前进");
            _Rshs.Add("12", "停止");

            _lsa.Add(_Rshs);
            lentr[4] = "2009_1011";

            _RFihs.Add("10", "前进");
            _RFihs.Add("9", "前进");
            _RFihs.Add("7", "右转");
            _RFihs.Add("4", "前进");
            _RFihs.Add("5", "停止");
            _lsa.Add(_RFihs);
            lentr[5] = "1009_3007_1004";

            while (true)
            {
                Thread.Sleep(100);
                if (_Alist.Count > 0)
                {
                    if (_begin)
                    {
                        _begin = false;
                        _work = _Alist.Dequeue().ToString();

                       // SetTestCall(lentr[int.Parse(_work)]);
                        SetCKCall(lentr[int.Parse(_work)]);
                        //
                       
                    }
                }
                //if (!_begin)
                //{
                //    txtPoint.Text = GetPLCData(getMuesID("H62RFID地标编码"));
                //    DQCmd = GetPLCData(getMuesID("机器人当前指令"));
                //    if (((Hashtable)_lsa[int.Parse(_work)]).ContainsKey(txtPoint.Text.Trim()))
                //    {
                //        if (((Hashtable)_lsa[int.Parse(_work)])[txtPoint.Text.Trim()].ToString() != DQCmd)
                //        {
                //            string fxcmd = ((Hashtable)_lsa[int.Parse(_work)])[txtPoint.Text.Trim()].ToString();
                //            if (fxVal)
                //            {
                //                fxcmd = GetFx(fxcmd);
                //            }
                //            SendCmdText(fxcmd);
                //            if (fxcmd == "停止")
                //            {
                //                _begin = true;
                //                SetClear();
                //            }
                //        }
                //    }

                //}
            }
        }

        public void SetTestCall(string sVar)
        {
            string[] SetVals = sVar.Split('_');
            _sendobj.ListVal = new string[2];

            for (int i = 0; i < 5; i++)
            {
                if (SetVals.Length > i)
                {
                    _sendobj.ListVal[0] = getMuesID("路径代码" + (i + 1).ToString());
                    if (!fxVal)
                    {
                        if (SetVals[i].Substring(0, 1) == "1" || SetVals[i].Substring(0, 1) == "4")
                        {
                            _sendobj.ListVal[1] = GetFx(SetVals[i].Substring(0, 1)) + SetVals[i].Remove(0, 1);
                        }
                        else
                        {
                            _sendobj.ListVal[1] = SetVals[i];
                        }
                    }

                }
                else
                {
                    _sendobj.ListVal[0] = getMuesID("路径代码" + (i + 1).ToString());
                    _sendobj.ListVal[1] = "0";

                }
                SendCmd();
            }

        }
        //
        public void SetCKCall(string sVar)
        {
            string[] SetVals = sVar.Split('_');
            _sendobj.ListVal = new string[2];

            for (int i = 0; i < 10; i++)
            {
                if (SetVals.Length > i)
                {
                    Thread.Sleep(10);
                    _sendobj.ListVal[0] = getMuesID("路径代码" + (i + 1).ToString());
                    //if (!fxVal)
                    //{
                    //    if (SetVals[i].Substring(0, 1) == "1" || SetVals[i].Substring(0, 1) == "4")
                    //    {
                    //        _sendobj.ListVal[1] = GetFx(SetVals[i].Substring(0, 1)) + SetVals[i].Remove(0, 1);
                    //    }
                    //    else
                    //    {
                            _sendobj.ListVal[1] = SetVals[i];
                    //    }
                    //}

                }
                else
                {
                    _sendobj.ListVal[0] = getMuesID("路径代码" + (i + 1).ToString());
                    _sendobj.ListVal[1] = "0";
                }
              
                SendCmd();
            }
            Thread.Sleep(10);
            if (sVar == "0")
            {
                _sendobj.ListVal[0] = getMuesID("路径代码11");
                _sendobj.ListVal[1] = "0";
                SendCmd();
                return;
            }
            _sendobj.ListVal[0] = getMuesID("路径代码11");
            _sendobj.ListVal[1] = "254";
            SendCmd();
        }

        //清理
        public void SetClear()
        {
            _sendobj.ListVal = new string[2];
            for (int i = 0; i < 5; i++)
            {
                _sendobj.ListVal[0] = getMuesID("路径代码" + (i + 1).ToString());
                _sendobj.ListVal[1] = "0";
                SendCmd();
            }
        }
        public string GetFx(string sw)
        {
            string fsw = "";
            if (sw == "1")
            {
                fsw = "4";
            }
            else if(sw=="4")
            {
                fsw = "1";
            }
            if (sw == "前进")
            {
                fsw = "后退";
            }
            else if (sw == "后退")
            {
                fsw = "前进";
            }
            else
            {
                fsw = sw;
            }
            return fsw;
        }
        //计划路程
        public string GetDataCmd(string scmd)
        {
            string Retext = "";
            if (scmd == "9001")
            {
                Retext = "前进";
            }
            else if (scmd == "9002")
            {
                Retext = "左转";
            }
            else if (scmd == "9003")
            {
                Retext = "右转";
            }
            else if (scmd == "9004")
            {
                Retext = "后退";
            }
            return Retext;
        }


        public void SetCall()
        {
            this.SetDataItemValue("MWSMART.AGV.H31PC心跳", Cuplc.ToString());
        }

        public string GetPLCData(string PID)
        {
            object ltem = GetDataItemValue(PID).DataValue;
            if (ltem != null)
            {
                return ltem.ToString();
            }
            else
            {
                return "0";
            }
        }
    
        public void GetAlic()
        {
            
        }

        ComData _ComData;
        RoutePlanner planner = new RoutePlanner();
        public void InitComDataSet()
        {
            _ComData = new ComData();
            _ComData.initNode();
          // _ComData.initFNode();
        }

        public void BackPoint()
        { 
            RoutePlanResult Tresult = null;
            string _pathPiont = "";
            string _Piont = "";
            while (true)
            {
                Thread.Sleep(100);
                if (_BackAlist.Count > 0)
                { 
                    _beginBack = false;
                    Tresult = ((RoutePlanResult)_BackAlist.Dequeue());
                    foreach (string strln in Tresult.getPassedNodeIDs())
                    {
                        _pathPiont += strln;
                    }
                }

                if (!_beginBack)
                {
                    txtPoint.Text = GetPLCData(getMuesID("H62RFID地标编码"));
                    if (getMuesID("机器人当前指令").Trim() == "0")
                    {
                        _beginBack = true;
                    }
                    if (txtPoint.Text == "4")
                    {
                        SendCmdText("左转");
                    }
                    else if (txtPoint.Text == "8")
                    {
                        SendCmdText("右转");
                    }
                    else if (txtPoint.Text == "9")
                    {
                        SendCmdText("左转");
                    }
                    if (txtPoint.Text == "12")
                    {
                        SendCmdText("停止");
                    }
                }

            }
        }
  
        #endregion

        #region  串口设置


        private delegate void ReadCommData();//读串口
        private delegate void myDelegate();

        public SerialPort serialPort1;
        Queue _Que;

        public void SetComm(int isQT,string sVar,string MLocal)
        {
            string[] SetVals = sVar.Split('_');
            byte[] Creatsend = new byte[21];

            for (int n = 0; n < Creatsend.Length; n++)
            {
                Creatsend[n] = 0x00;
            }
            Creatsend[0] = 0xFE;
            Creatsend[1] = 0x02;
            Creatsend[2] = (byte)isQT;
            //fxVal = txtdqfw.Text =="1"? true :false;
            //if (fxVal)
            //{
            //    Creatsend[2] = 0x01;
            //}
            //else
            //{
            //    Creatsend[2] = 0x04;
            //}
            Creatsend[3] = (byte)int.Parse(MLocal);
            int i=4;
            foreach (string strl in SetVals)
            {
                int Temp = int.Parse(strl.Substring(0, 1));

                //if (fxVal)
                //{
                //    if (Temp == 4 || Temp == 1)
                //        Temp = 1;
                //}
                //else
                //{
                //    if (Temp == 4 || Temp == 1)
                //        Temp = 4;
                //}
                Creatsend[i++] = (byte)Temp;
                

                Creatsend[i++] = (byte)int.Parse(strl.Substring(1, 3));
            }
            Creatsend[Creatsend.Length-1] = 0xFF;
            SendSS = Creatsend;
            SendComm(SendSS);
            
        }

        public void SetClier()
        {
            if (_isCommOrOPC)
            {
                SetCKCall("0");
            }
            else
            {
                byte[] Creatsend = new byte[21];
                for (int i = 0; i < Creatsend.Length; i++)
                {
                    Creatsend[i] = 0x00;
                }
                Creatsend[0] = 0xFE;

                Creatsend[Creatsend.Length - 1] = 0xFF;
                SendComm(Creatsend);
            }
        }

        public void SetCMD(int cmd)
        {

            //byte[] Creatsend = new byte[21];
            //for (int i = 0; i < Creatsend.Length; i++)
            //{
            //    Creatsend[i] = 0x00;
            //}
            //Creatsend[0] = 0xFE;
            //Creatsend[1] = 0x01;//s1AGV识别码
            //Creatsend[2] = 0x00;//s2AGV装货状态
            //Creatsend[3] = 0x01;
            //Creatsend[4] = (byte)cmd;
            //Creatsend[Creatsend.Length - 1] = 0xFF;
            //SendSS = Creatsend;
            //SendComm(SendSS);

            

        }

        //功能设置
        public void SendFunction(int Cmd)
        {
            switch (Cmd)
            {
                case 1://
                    break;
                case 2://
                    break;
                case 3://
                    break;
                case 4://
                    break;
            }
        }

        public bool isFOpenCom = false;
        public void initComm()
        {
            try
            {
                serialPort1 = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
                _Que = new Queue();
                //关键 为 serialPort1绑定事件句柄
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
             
                serialPort1.Open();
                ReadCommData _ReadCommData = new ReadCommData(ReadComm);
                _ReadCommData.BeginInvoke(null, null);

                //WritePLData _WritePLData = new WritePLData(WriteLCData);
                //_WritePLData.BeginInvoke(null, null);
                //this.timer1.Enabled = true;
                isFOpenCom = true;

                SendCmdText("停止");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
         
        }
        int Recn = 0;
        byte[] SendSS;

        public void ReadComm()
        {
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                   

                    //string[] send = FS.Split('_');

                    //if (send.Length > 2)
                    //{

                    //    foreach (string temps in send)
                    //    {
                    //        if (temps.Trim() != "")
                    //        {
                    //            // this.richTextBox2.Text += "_" + temps;
                    //            byte[] by = strToToHexByte(temps);
                    //            Recn++;
                    //            this.txtPLC.Text = Recn.ToString();
                    //            if (Recn > 50)
                    //                Recn = 0;

                    //            if (by.Length > 4)
                    //            {
                    //                this.txtPoint.Text = ((int)by[1]).ToString();
                    //                this.txtdqfw.Text = ((int)by[3]).ToString();
                    //                this.txtPower.Text = ((int)by[4]).ToString();
                    //                this.txtDQCMD.Text = ((int)by[2]).ToString();
                    //            }

                    //        }
                    //    }

                    //    FS = "";
                    //}
                   
                    

                    if (SendSS != null)
                    {
                        if (CmdInt == 0)
                        {
                            GetComData();
                        }
                        else
                        {
                            SendComm(SendSS);
                        }
                    }
                }
                catch (Exception el)
                {
                    LogManager.WriteLog(el.Message.ToString());
                }
                finally
                {
                    // GetComData();
                }

            }

        }
        public string GetPall(string ps)
        {
            string Cmdtext = "";
            int m = 0;
            for (int i = 0; i < ps.Length; i+=2)
            {
                m++;
                Cmdtext +="#["+m.ToString()+ "_" + ps.Substring(i,2)+"]";
            }

            return Cmdtext.TrimStart('#');

        }
        int CmdInt = 0;

        public void GetComData()
        {

            byte[] Creatsend = new byte[21];
            for (int i = 0; i < Creatsend.Length; i++)
            {
                Creatsend[i] = 0x00;
            }
            Creatsend[0] = 0xFE;
            Creatsend[1] = 0x01;//s1AGV识别码
            Creatsend[2] = 0x00;//s2AGV装货状态
            Creatsend[3] = 0x00;
            Creatsend[4] = 0x00;
            Creatsend[5] = 0x23;//地标

            Creatsend[Creatsend.Length - 1] = 0xFF;
            SendComm(Creatsend);
            
        }



        public void SendComm(byte[] sendpy)
        {
            try
            {
                serialPort1.Write(sendpy, 0, sendpy.Length);
           
               string tempstr =  byteToHexStr(sendpy);

               LogManager.WriteLog("Excalp", DateTime.Now.ToString() + "->>" + GetPall(tempstr));
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message, "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string FS = "";
        bool isComPort = false;
        //串口数据到达时的事件
        byte[] byFs = null;
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //关键 代理
            try
            {
                if (serialPort1.IsOpen == true)
                {
                    byte[] readBuffer = new byte[serialPort1.BytesToRead];
                    serialPort1.Read(readBuffer, 0, readBuffer.Length);
                    string readstr = byteToHexStr(readBuffer);

                    byFs = readBuffer;
                    FS += readstr;
                    if (FS.Length > 500)
                    {
                        FS = "";
                    }
                  //  richTextBox2.Text = FS.Replace("FE","_");

                    isComPort = true;

                    if (FS.IndexOf("FF") > -1)
                    {
                        string Fsps = FS.Replace("FF", "_");
                        string[] fsp = Fsps.Split('_');  
                        string writ = "";
                        writ = GetPall(fsp[0]);
                        foreach (string sp in fsp)
                        {
                            FS = sp;
                        } 
                        LogManager.WriteLog("Excalp", DateTime.Now.ToString() + "->" + writ);
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
        }
      
       
       
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

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {

            int sl = hexString.Length % 2;
            if (sl == 1)
            {
              hexString = hexString.Remove(hexString.Length - 1);
            }
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        #endregion 
        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                byte[] SendBuffer = new byte[3];
                SendBuffer[0] = 0xFE;
                SendBuffer[1] = 0x00;
                SendBuffer[2] = 0xFF;
                serialPort1.Write(SendBuffer, 0, SendBuffer.Length);
            }
        }

        private void txtPoint_TextChanged(object sender, EventArgs e)
        {
            if (_SetPoint != null)
            {
                foreach (string ksy in _Fhts.Keys)
                {
                    if (_Fhts[ksy].ToString().Trim() == txtPoint.Text.Trim())
                    {
                        _SetPoint(ksy);
                        break;
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_sendobj.ListVal = new string[2];
            //_sendobj.ListVal[0] = getMuesID("AGV的运动状态");
            //_sendobj.ListVal[1] = this.comboBox2.SelectedIndex.ToString();
            //SendCmd();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
        //    string[] send = FS.Split("FE".ToCharArray());
        //    if (send.Length > 2)
        //    {
                
        //        foreach (string temps in send)
        //        {
        //            if (temps.Trim() != "")
        //            {
        //                // this.richTextBox2.Text += "_" + temps;
                     
        //                byte[] by = strToToHexByte(temps);
        //                Recn++;
        //                this.txtPLC.Text = Recn.ToString();
        //                if (Recn > 50)
        //                    Recn = 0;

        //                if (by.Length > 5)
        //                {
        //                    this.txtPoint.Text = ((int)by[1]).ToString();
        //                    this.txtdqfw.Text = ((int)by[3]).ToString();
        //                    this.txtPower.Text = ((int)by[4]).ToString();
        //                    this.txtDQCMD.Text = ((int)by[2]).ToString();
        //                }

        //            }
        //        }
        //    }

        //    if (SendSS != null)
        //    {

        //        if (this.txtPoint.Text.Trim() != this.txtgopoint.Text.Trim())
        //        {
        //            SendComm(SendSS);
        //        }
        //        else
        //        {
        //            GetComData();
        //        }
        //    }
        //    FS = "";


        }


    }
}
