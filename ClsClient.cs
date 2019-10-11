using System;
using System.Collections.Generic;

using System.Text;

using System.Net.Sockets;
using StriveEngine.Core;
using StriveEngine.Tcp.Passive;
using StriveEngine;

namespace WinRobots
{
    public  class ClsClient
    {
        private ITcpPassiveEngine tcpPassiveEngine;
        public  Action<string> _Region;
        public  ClsClient()
        {
            try
            {
               this.tcpPassiveEngine = NetworkEngineFactory.CreateTextTcpPassiveEngine("120.24.156.195", 13567, new DefaultTextContractHelper("\0"));
               this.tcpPassiveEngine.MessageReceived += new CbDelegate<System.Net.IPEndPoint, byte[]>(tcpPassiveEngine_MessageReceived);
               this.tcpPassiveEngine.AutoReconnect = true;//启动掉线自动重连                
               this.tcpPassiveEngine.ConnectionInterrupted += new CbDelegate(tcpPassiveEngine_ConnectionInterrupted);
               this.tcpPassiveEngine.ConnectionRebuildSucceed += new CbDelegate(tcpPassiveEngine_ConnectionRebuildSucceed);
               this.tcpPassiveEngine.Initialize();
            }
            catch (Exception ee)
            {

            }
        }
        void tcpPassiveEngine_ConnectionRebuildSucceed()
        {

        }
        void tcpPassiveEngine_ConnectionInterrupted()
        {

        }

        void tcpPassiveEngine_MessageReceived(System.Net.IPEndPoint serverIPE, byte[] bMsg)
        {
            string msg = System.Text.Encoding.UTF8.GetString(bMsg); //消息使用UTF-8编码
            msg = msg.Substring(0, msg.Length - 1); //将结束标记"\0"剔除
            if (_Region != null)
            {
                _Region(msg);
            }
        }
    }
}
