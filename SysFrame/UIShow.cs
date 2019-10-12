using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WinRobots.SysFrame
{
    public partial class UIShow : UserControl
    {
        public UIShow()
        {
            InitializeComponent();
            initButton();
            SetColor();
            _SetModbusItme = new SetModbusItme();
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

        int sl = 0;
        void Conn_MouseClick(object sender, MouseEventArgs e)
        {
            if (_SetModBus != null)
            {
                switch (((Button)sender).Text)
                {
                    case "10":
                        _SetModBus("11");
                        Thread.Sleep(100);
                        _SetModBus("10");
                        return;
                }
            }
            if (_SetObjModBus != null)
            {
                SetColor();
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
                            S[0] = 9004;
                        }
                        else
                        {
                            S[0] = 9001;
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

        public void SetColor()
        {
            foreach (Control Conn in this.Controls)
            {
                if (Conn is PictureBox)
                {
                    Conn.BackColor = System.Drawing.Color.Transparent;

                }
            }

        }
        public delegate void CbDelegate<T>(T obj);

        public void WriteText(string VText)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbDelegate<string>(this.WriteText), VText);
            }
            else
            {  
                this.richTextBox1.Text = VText;
            }
        }

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
                    SetPoint(VText.ResUshort[26].ToString());
                    LocalFW = VText.ResUshort[25].ToString();

                }
                else if (VText.DevID.ToString() == "2")
                {
                    this.richTextBox2.Text = "设备编号：" + VText.DevID.ToString() + GetStringTo(VText.ResUshort);
                }
                else if (VText.DevID.ToString() == "3")
                {
                    this.richTextBox3.Text = "设备编号：" + VText.DevID.ToString() + GetStringTo(VText.ResUshort);
                }
             
            }
        }
        public void SetPoint(string _Point)
        {
            foreach (Control Conn in this.Controls)
            {
                if (Conn is Button)
                {
                    if (Conn.Text.Trim() == _Point)
                    {
                        LocalPoint = _Point;
                        pictureBox2.Location = new Point(Conn.Location.X + 30, Conn.Location.Y);
                    }
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
                        Tempstr += "\n弯道速度："+Val[i].ToString();
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
        //更新设计
        public void AGVUpdate(string Val)
        {
            string[] TempReadData = Val.Split('_');
            string ShowText = "";
            byte[] Tempby;
            foreach (string tempData in TempReadData)
            {
              Tempby =  strToToHexByte(tempData);
            }
            this.richTextBox1.Text = ShowText;
            
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

        private void SetFunction(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox _pic = sender as PictureBox;
                _pic.BackColor = Color.Blue;
            }
        }
    }
}
