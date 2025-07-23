using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net5_10_14.Table.zzsy
{
    public class experimentstandard_ziliudianzushiyan
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
        public double Xiandianzhupinghen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Xdianzhupinghen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Createuser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Ccreateuserdate { get; set; }

        public experimentstandard_ziliudianzushiyan Clone()
        {
            return (experimentstandard_ziliudianzushiyan)this.MemberwiseClone();
        }

    }
}
