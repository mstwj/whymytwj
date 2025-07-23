using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net5_10_14.Table.bbsy
{
    public class experimentstandard_bianbishiyan
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Guigexinhao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Tuhao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Bianbiwucha { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Createuser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ccreateuserdate { get; set; }

        public experimentstandard_bianbishiyan Clone()
        {
            return (experimentstandard_bianbishiyan)this.MemberwiseClone();
        }
    }
}
