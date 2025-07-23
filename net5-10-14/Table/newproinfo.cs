using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net5_10_14.Table
{
    public class newproinfo
    {
        public int Id { get; set; } //产品ID..
        public string ProName { get; set; } //变压器名
        public string GuigeXinhao { get; set; } // 规格型号
        public int Tuhao { get; set; } // 图号
        public string ProType { get; set; } //变压器类型
        public string ProTypeItem { get; set; }// 变压器类别
        public string ProOhefanshi { get; set; }// 变压器类别
        public string ProXiangShu { get; set; } //相数
        public string ProWaijyuancaizhi { get; set; } //-- 绕组外绝缘介质
        public string ProColdFunc { get; set; } //-- 冷却方式
        public string ProOldloop { get; set; } //-- 油循环
        public string ProLaozhu { get; set; } //-- 绕组数
        public string ProTiaoyafashi { get; set; }// -- 调压方式
        public string ProXianQCaizi { get; set; } //-- 线圈导线材质，
        public string ProTianxCaizi { get; set; } //-- 铁心材质
        public string ProTeshuytu { get; set; } //-- 特殊用途或特殊结构
        public int EdingRonglang { get; set; } //-- 额定容量 INT
        public double Taoyabili { get; set; } //-- 调压比例 FLOAT
        public double GaoyaedingDianya { get; set; }// -- 高压额定电压FLOAT
        public double GaoyaedingDianliu { get; set; } //-- 高压额定电流FLOAT
        public double ZhongyaedingDianya { get; set; } //-- 中压额定电压FLOAT
        public double ZhongyaedingDianliu { get; set; } //-- 中压额定电流FLOAT
        public double DiyaedingDianya { get; set; } //-- 低压额定电压FLOAT
        public double DiyaedingDianliu { get; set; } //-- 低压额定电压FLOAT

        public string BiaoHao { get; set; } //--标号...N/N
        public string ZhuHao { get; set; } //--组号...0 --- 11
        public int GaoRaozhuNumber { get; set; } //--高绕组数量
        public int DiRaozhuNumber { get; set; } //--地绕组数量        
        public int DanWeiNumber { get; set; } //--挡位数量.

        public newproinfo Clone()
        {
            return (newproinfo)this.MemberwiseClone();
        }

    }

}
