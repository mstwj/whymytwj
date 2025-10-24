using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 无功功率补偿
{
    
    class Capacitancecompensation
    {
        //視在功率(用户输入)
        //private float apparentpower = 300.0f;

        //現狀功率因數(用户输入)
        //private float powerfactor = 0.85f;

        //目標功率因數(用户输入)
        //private float targetpowerfactor = 0.99f;

        //得到有功功率 参数(視在功率, 現狀功率因數)
        public float GetActivePower(float apparentpower, float powerfactor)
        {
            return apparentpower * powerfactor;
        }

        //得到无功功率 参数(視在功率, 現狀功率因數)
        public float GetReactivePower(float apparentpower, float powerfactor)
        {            
            float phiRadians = (float)Math.Acos(powerfactor);            
            float sinPhi = (float)Math.Sin(phiRadians);// 计算sinφ
            return apparentpower * sinPhi;            
        }

        //得到目标角度..参数(目标功率因素)
        public float GetTargetPhaseAngle(float targetpowerfactor)
        {
            float radians = (float)Math.Acos(targetpowerfactor); 
            return (float)(radians * (180 / Math.PI));
        }

        //得到目标无功功率  参数(有功功率,目标功率因素)
        public float GetTargetWugonglv(float activepower , float targetpowerfactor)
        {
            float radians = (float)Math.Acos(targetpowerfactor);            
            radians = (float)Math.Tan(radians);            
            return activepower * radians;
        }

        //需要補償的無功容量 参数（无功功率,目標无功功率）
        public float GetCompensateReactivePower(float reactivepower, float targetwugonglv)
        {
            return reactivepower - targetwugonglv;
        }
    }
}
