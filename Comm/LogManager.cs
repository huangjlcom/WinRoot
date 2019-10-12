using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace WinRobots.Comm
{
    public class LogManager
    {
        private static bool isDebug = true;
        private static string logPath = string.Empty;
        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    logPath = AppDomain.CurrentDomain.BaseDirectory;
                }
                return logPath;
            }
            set { logPath = value; }
        }

        private static string logFilePrefix = string.Empty;
        /// <summary>
        /// 日志文件前缀
        /// </summary>
        public static string LogFilePrefix
        {
            get { return logFilePrefix; }
            set { logFilePrefix = value; }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static bool WriteLog(string logFile, string msg)
        {
            try
            {
                if (isDebug)
                {
                    System.IO.StreamWriter sw = System.IO.File.AppendText(
                        LogPath + LogFilePrefix + logFile + " " +
                        DateTime.Now.ToString("yyyyMMdd") + ".Log"
                        );
                    sw.WriteLine(/*DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + */msg);
                    sw.Close();
                    return true;
                }
            }
            catch
            { return false; }

            return false;
        }

        /// <summary>
        /// 写日志 默认错误日志
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteLog(string Msg)
        {
            WriteLog(LogFile.Error, Msg);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        public static void WriteLog (Exception ex)
        {
            WriteLog(LogFile.Error, ex.ToString ());
        }

        /// <summary>
        /// 写日志
        /// </summary>
        public static bool WriteLog(LogFile logFile, string msg)
        {
            return WriteLog(logFile.ToString(), msg);
        }

        public static void WriteLogDeubg(string msg, string path = "")
        {
            try
            {
                if (isDebug)
                {
                    if (path == "")
                        path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Debug.log";

                    System.IO.StreamWriter sw = System.IO.File.AppendText(path);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + msg + Environment.NewLine);
                    sw.Close();
                }
            }
            catch
            { }
        }
        public void Read(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line.ToString());
            }
        }
        public void Write(string path,string Text)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(Text);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogFile
    {
        Trace,
        Warning,
        Error,
        SQL,
        Info,
        摄像机ID
    }



}
