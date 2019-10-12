using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WinRobots.SysFrame
{
   public  class RunTask
    {

       //运行线程
       Thread _Run;
       public  Action<string[]> RunStr;
       public string _StrTask="";//路径代码
       public string _DevID = "";//设备编号
       public void runThread(string StrTask,string DevID)
       {
           _StrTask = StrTask;
           _DevID = DevID;
           _Run = new Thread(new ThreadStart(RunExe));
           _Run.Start();

       }
       public void RunExe()
       {
           while (true)
           {
               if (_StrTask != "")
               {
                   string[] strl = _StrTask.Replace("A", "").Split('_');
                   if (strl.Length == 0) continue;
                   string[] setVal = new string[2];
                   setVal[0] = _DevID;
                  // 预计是否可以通过
                   if (isRun())
                   {
                       Thread.Sleep(500);
                       continue;
                   }

                   foreach (string tempstr in strl)
                   {
                       Thread.Sleep(500);
                       setVal[1] = tempstr;
                       if (RunStr != null)
                       {
                           RunStr(setVal);
                       }
                   }
                   Thread.Sleep(2000);
                   if (_Run != null)
                   {
                       _Run.Abort();
                   }
               }

           }
       }

       public bool isRun()
       {

           if (UCKGS._PublicHash.Count > 0)
           {
               foreach (string skey in UCKGS._PublicHash.Keys)
               {
                   //判断另外AGV是否在故障点
                   if (skey.Trim() != _DevID.Trim())
                   {
                       string Tempstr = UCKGS._PublicHash[skey].ToString();
                       int tempint = SumPath(Tempstr.Replace("A", ""));
                       if (tempint == SumPath(_StrTask.Replace("A", "")))
                       {
                           //判断是否在前后三个坐标点
                       }

                   }
               }
           }
           else
           {
               UCKGS._PublicHash.Add(_DevID, _StrTask);
               return true;
           }
           return false;
       }

       public int SumPath(string PathVal)
       {
           if (PathVal.IndexOf("14_9") > -1)
           {
               return 1;
           }
           else if (PathVal.IndexOf("9_14") > -1)
           {
               return 2;
           }
           else if (PathVal.IndexOf("9_6") > -1)
           {
               return 3;
           }
           else if (PathVal.IndexOf("6_9") > -1)
           {
               return 4;
           }
           else if (PathVal.IndexOf("6_9_14") > -1)
           {
               return 5;
           }
           else if (PathVal.IndexOf("14_9_6") > -1)
           {
               return 6;
           }

           return 0;
       }
    }
}
