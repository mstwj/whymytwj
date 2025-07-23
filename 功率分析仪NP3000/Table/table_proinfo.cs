using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 功率分析仪NP3000.Table
{
    public class table_proinfo
    {
        public int Id { get; set; } //产品ID..
        public string YanPingBianhao { get; set; } //产品ID..
        public string GuigeXinhao { get; set; } //产品ID..
        public int Tuhao { get; set; } //产品ID..
        public string XiangShu { get; set; } //产品ID..
        public string Laozhu { get; set; } //产品ID..
        public int GaoYaLaozhu { get; set; } //产品ID..
        public int DiYaLaozhu { get; set; } //产品ID..
        public int DanWei { get; set; } //产品ID..
        public string CaiZhi { get; set; } //产品ID..
        public int EDingLongLiang { get; set; } //产品ID..
        public double TiaoYaBili { get; set; } //产品ID..
        public double GaoYaEDingDianYa { get; set; } //产品ID..
        public double DiYaEDingDianYa { get; set; } //产品ID..
        public string BiaoHao { get; set; } //产品ID..                                     
        public string ZhuHao { get; set; } //产品ID..

        public table_proinfo Clone()
        {
            return (table_proinfo)this.MemberwiseClone();
        }

    }
}
