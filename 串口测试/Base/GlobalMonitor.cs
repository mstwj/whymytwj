using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace 串口测试.Base
{
    public class GlobalMonitor
    {

        public static SerializationInfo 

        static bool isRunning = true;
        static Task mainTask = null;

        public static void Start(Action successAction, Action<string> failureAction)
        {
            mainTask = Task.Run(() =>
            {
                var si = bll.InitserialInfo();

                if (si.State)
                    SerializationInfo = si.Data;
                else
                {
                    failureAction(si.Message);
                    return;
                }

                while (isRunning)
                {

                }

            });
        }

        public static void Dispose()
        {
            isRunning = false;
        }
    }
}
