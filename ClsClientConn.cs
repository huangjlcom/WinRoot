using System;
using System.Collections.Generic;
using System.Text;
using StriveEngine.Tcp.Passive;
using StriveEngine;
using StriveEngine.Core;
using IniFileRobot;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.NetworkInformation;

namespace WinRobots
{
   public class ClsClientConn
    {
       private ITcpPassiveEngine tcpPassiveEngine;

       IniFiles ini;
       string TxtIP;
       string TxtPort;
       string txtRemoteServerIP;

       public Action<string> _ReadMsg;

       bool _SyBl = false;
       public ClsClientConn()
       { 
           string FileIni = System.Windows.Forms.Application.StartupPath + @"/Config.ini";
           if (File.Exists(FileIni))
           {
               ini = new IniFiles(FileIni);
           }
           else
           {
               return;
           }
           TxtIP = ini.ReadString("Config", "SeverIP", "");
           TxtPort = ini.ReadString("Config", "SeverPort", "");
           txtRemoteServerIP = ini.ReadString("Config", "SeverOPCIP", "");
       }
       public void initConnt()
       {
           try
           {
               if (TxtIP != null && TxtPort != null)
               {
                   bool online = false; //是否在线  
                   Ping ping = new Ping();
                   PingReply pingReply = ping.Send(TxtIP);
                   if (pingReply.Status != IPStatus.Success)
                   {
                       return;
                   }  
                   this.tcpPassiveEngine = NetworkEngineFactory.CreateTextTcpPassiveEngine(TxtIP, int.Parse(TxtPort), new DefaultTextContractHelper("\0"));
                   this.tcpPassiveEngine.MessageReceived += new CbDelegate<System.Net.IPEndPoint, byte[]>(tcpPassiveEngine_MessageReceived);
                   this.tcpPassiveEngine.AutoReconnect = true;//启动掉线自动重连                
                   this.tcpPassiveEngine.ConnectionInterrupted += new CbDelegate(tcpPassiveEngine_ConnectionInterrupted);
                   this.tcpPassiveEngine.ConnectionRebuildSucceed += new CbDelegate(tcpPassiveEngine_ConnectionRebuildSucceed);
                   this.tcpPassiveEngine.Initialize();
               }
           }
           catch (Exception ee)
           {
           }
       }

       public void SendMsg(object _send)
       {
           string msg =  "\0";// "\0" 表示一个消息的结尾
           byte[] Sendby = Object2Bytes(_send) ;
           byte[] bMsg = System.Text.Encoding.UTF8.GetBytes(msg);//消息使用UTF-8编码
           Array.Resize(ref Sendby, Sendby.Length+bMsg.Length);
           bMsg.CopyTo(Sendby, Sendby.Length - bMsg.Length);
           if (this.tcpPassiveEngine.Connected)
           {
               this.tcpPassiveEngine.SendMessageToServer(Sendby);
           }
       }

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

       /// <summary>
       /// 将byte数组转换成对象
       /// </summary>
       /// <param name="buff">被转换byte数组</param>
       /// <returns>转换完成后的对象</returns>
       public object Bytes2Object(byte[] buff)
       {
           object obj;
           using (MemoryStream ms = new MemoryStream(buff))
           {
               IFormatter iFormatter = new BinaryFormatter();
               obj = iFormatter.Deserialize(ms);
           }
           return obj;
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
           this.ShowMessage(msg);
           if (_ReadMsg != null)
           {
               _ReadMsg(msg);
           }
       }

       private void ShowMessage(string msg)
       {
          
       }
    }
}
