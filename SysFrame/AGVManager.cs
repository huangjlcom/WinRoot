using System;
using System.Collections.Generic;
using System.Text;

namespace WinRobots.SysFrame
{
   public class AGVManager
    {
       int _AGVID = 0;//编号
       public string Name = ""; //名称
       public string State = "";//状态
       public string Context = "";

       public string SetLocal = "";//当前地标
       public GetModbusItme GModbusItme;
       public SetModbusItme SModbusItme;
       public void initAGV()
       {

       }

       public int AGVID
       {
           get
           {
               return this._AGVID;
           }
           set
           {
               this._AGVID = value;
           }
       }
       public AGVManager()
       {
           GModbusItme = new GetModbusItme();
           GModbusItme.DevID = AGVID;
           GModbusItme.initUshort(40);//初始化个数
           SModbusItme = new SetModbusItme();
           SModbusItme.DevID = AGVID;
       }
       //plc指令集
       public void SendCmd(string Smd)
       {
           switch (Smd)
           {
               case "":
                   break;
           }
       }
    }
}
