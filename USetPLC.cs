using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using System.IO;
using IniFileRobot;
using System.Net;
using OPCAutomation;
using System.Collections;

namespace WinRobots
{
    public partial class USetPLC : UserControl
    { 
     

        /// <summary>
        /// 主机IP
        /// </summary>
        string strHostIP = "";
        /// <summary>
        /// 主机名称
        /// </summary>
        string strHostName = "";
        /// <summary>
        /// 
        /// <summary>
        /// OPCServer Object
        /// </summary>
        OPCServer KepServer;

        frmShow _frmShow;
        
        public USetPLC()
        {
            InitializeComponent();

            foreach (Control Con in this.Controls)
            {
                if (Con is Button)
                {
                    Con.BackColor = System.Drawing.Color.RoyalBlue;
                }
            }
        }
     
        private void button2_Click(object sender, EventArgs e)
        {
          
            ini.WriteString("Config", "SeverIP", TxtIP.Text);
            ini.WriteString("Config", "SeverPort", TxtPort.Text);
         
        }

          
        public void getDataList(List<string> values)
        {

        }
        public void ReDataInfo(Dictionary<string, string> DataVal)
        {

        }

        private void USetPLC_Load(object sender, EventArgs e)
        {
            initConfig();
        }
        IniFiles ini;
        public void initConfig()
        {
            GetLocalServer();
            string FileIni = System.Windows.Forms.Application.StartupPath + @"/Config.ini";
            if (File.Exists(FileIni))
            {
                ini = new IniFiles(FileIni);
            }
            else
            {
                return;
            }
            TxtIP.Text = ini.ReadString("Config", "SeverIP", "");
            TxtPort.Text = ini.ReadString("Config", "SeverPort", "");
        

        }

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
             
            }
            catch (Exception err)
            {
                MessageBox.Show("枚举本地OPC服务器出错：" + err.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ini.WriteString("Config", "SeverPort", TxtPort.Text);
            ini.WriteString("Config", "SeverIP", TxtPort.Text);
           
        }

        object _obj;
        private void SetFuntion(object sender, EventArgs e)
        {
            Button _Control = sender as Button;
            switch (_Control.Text)
            {
                case "打开语音"://停止

                    break;
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
            }


        }

        private void hightspeed_MouseDown(object sender, MouseEventArgs e)
        {
            _obj = sender;
        }

    }
}
