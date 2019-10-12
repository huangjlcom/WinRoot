using System;
using System.Collections.Generic;
using System.Text;

namespace WinRobots.Comm
{
    /// <summary>
    /// AGVControl
    /// </summary>
    public class ClsAGVControl
    {
        /// <summary>
        /// 
        /// </summary>
        ///方向值
        ///
        public string CMDFX;
        /// <summary>
        /// 位置编号
        /// </summary>
        public string IDCmd;
        /// <summary>
        /// 
        /// </summary>
        public string ClientCode;
        /// <summary>
        /// 
        /// </summary>
        public string PointCode;
        //
        public string ID;
        //

        public string SendText(string Fsend)
        {
            string ReTVal="";
            switch (Fsend)
            {
                case "0":
                    ReTVal = "停止";
                    break;
                case "1":
                    ReTVal = "前进";
                    break;
                case "2":
                    ReTVal = "左转";
                    break;
                case "3":
                    ReTVal = "右转";
                    break;
                case "4":
                    ReTVal = "回退";
                    break;
            }
            return ReTVal;
        }

        public string GetRead()
        {
            return "";
        }
        public void GetCall()
        {
         
        }
        public void SendCall(string Call)
        {
        }
    }
}
