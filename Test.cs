using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace WinRobots
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private delegate void myDelegate(string s);

        private void SetText(string s)
        {

            richTextBox1.Text = richTextBox1.Text + s;

        }
        SerialPort serialPort1;
       //打开端口
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1 = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
                //关键 为 serialPort1绑定事件句柄
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                serialPort1.Open();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
 
        //串口数据到达时的事件
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //关键 代理
            myDelegate md = new myDelegate(SetText);
            try
            {
                if (serialPort1.IsOpen == true)
                {
                    byte[] readBuffer = new byte[serialPort1.ReadBufferSize];
                    serialPort1.Read(readBuffer, 0, readBuffer.Length);
                    string readstr = byteToHexStr(readBuffer);
 
                    Invoke(md, readstr);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
 
 
        }

        private string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                result += "%" + Convert.ToString(b[i], 16);
            }
            return result;
        }

        private string HexStringToString(string hs, Encoding encode)
        {
            //以%分割字符串，并去掉空字符
            string[] chars = hs.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(chars[i], 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
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

        private void Test_Load(object sender, EventArgs e)
        {
           
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            { 

                serialPort1.Write(tbSend.Text);
                byte[] ss = new byte[3];
                ss[0] = 0x01;
                ss[1] = 0x23;
                ss[2] = 0x23;

                serialPort1.Write(ss, 0, 3);
               // serialPort1.WriteLine(tbSend.Text);
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message, "出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
