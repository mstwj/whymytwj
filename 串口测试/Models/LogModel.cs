using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 串口测试.Models
{
    public enum LogType
    {
        Info = 0, Warn = 1, Fault = 2
    }

    public class LogModel
    {
        public int RowNumber { get; set; }
        public string DeviceName { get; set; }
        public string LogInfo { get; set; }
        public string Message { get; set; }

        public LogType LogType { get; set; }
    }
}
