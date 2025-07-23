using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCNET5电容塔.Data
{
    public class graphicPath
    {
        //(闭)
        public int X1B { get; set; } = 0;
        public int Y1B { get; set; } = 0;
        public int X2B { get; set; } = 0;
        public int Y2B { get; set; } = 0;

        //(开)
        public int X1K { get; set; } = 0;
        public int Y1K { get; set; } = 0;
        public int X2K { get; set; } = 0;
        public int Y2K { get; set; } = 0;


        public graphicPath(int x1,int y1,int length) 
        {            
            X1B = x1;
            Y1B = y1;
            X2B = x1;
            Y2B = y1 + length;

            X1K = x1-30;
            Y1B = y1;
            X2B = x1;
            Y2B = y1 + length;


        }
    }
}
