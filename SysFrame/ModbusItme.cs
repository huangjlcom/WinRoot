using System;
using System.Collections.Generic;
using System.Text;

namespace WinRobots.SysFrame
{
    public class SetModbusItme
    {
        public string ItemName;//名称
        public int DevID = 0;//设备标识
        public int Begin;//开始标识
        public int EndCount;//个数
        public string SendText;//
        public short[] Reshort;

    }

    public class GetModbusItme
    {
        public string ItemName="";//名称
        public int DevID = 0;//设备标识
        public int Begin = 0;//开始标识
        public int Registers = 0;//数据长度（寄存器数量）
        public string GetText = "";//
        public ushort[] ResUshort;
        public void initUshort(int ReadCount)
        {
            Registers = ReadCount;
            ResUshort = new ushort[ReadCount];
        }
    }
}
