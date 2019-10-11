using System;
using System.Collections.Generic;

using System.Text;

namespace WinRobots
{
    public class RobotIPMember
    {
        //private String item;
        private String value;
        public RobotIPMember(String value)
        {
            //this.function = function;
            this.value = value;
        }      
        public String GetValue()
        {
            return this.value;
        }
        public void SetValue(String value)
        {
            this.value = value;
        }
     
    }
}
