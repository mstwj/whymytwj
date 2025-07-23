using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net5_10_14.Table.bbsy
{
    public class bianbishiyan_report
    {
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Reportnumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Rreportdate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Reportuser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Reportcheckuser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ReportcheckStartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ReportcheckEndTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ReportcheckWendu { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ReportcheckShidu { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ReportcheckDaqiya { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReportcheckProNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Reportlocate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ReportHpressure { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ReportLpressure { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ReportEdingbianbi { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ReportABwucha { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ReportBCwucha { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ReportCAwucha { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RecportconnectNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RecportTestName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RecportTestSecnumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Recportzonghoujielun { get; set; }
        public bianbishiyan_report Clone()
        {
            return (bianbishiyan_report)this.MemberwiseClone();
        }
    }

}
