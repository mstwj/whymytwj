using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace 无功功率补偿
{
    
    class Capacitancecompensation
    {
        //这里是负载功率是不变的，也就是说 -- 有功功率不会改变，我们只是 无功补偿..
        //那有功功率是多少呢？
        //(用户输入)A相 电压
        public float UserAAvoltage;
        //(用户输入)A相 电流
        public float UserAAelectric;

        //(用户输入)B相 电压
        public float UserBAvoltage;
        //(用户输入)B相 电流
        public float UserBAelectric;

        //(用户输入)C相 电压
        public float UserCAvoltage;
        //(用户输入)C相 电流
        public float UserCAelectric;

        //负载功率因數(用户输入)就是初始有功功率.
        public float oldpowerfactor = 0.60f;

        //目標功率因數(就是0.99)
        private float targetpowerfactor = 0.99f;

        //温度..
        public float DRWD = 21.0f;

        //产生一个小波动的值..
        public float GetRandom(double Number1min, double Number2max)
        {
            Random random = new Random();
            double minValue = Number1min;
            double maxValue = Number2max;
            double range = maxValue - minValue;

            // 生成一个[0, 1)区间的随机浮点数
            double randomFloat = random.NextDouble();

            // 将这个随机浮点数缩放到[0, range]区间
            double scaledFloat = randomFloat * range;

            // 加上最小值得到最终结果
            double finalValue = scaledFloat + minValue;

            return (float)finalValue;
        }

        public void JIsuanInitializing()
        {
            //电压 ABC
            UserAAvoltage = UserAAvoltage + GetRandom(-0.5, 0.5);
            UserBAvoltage = UserBAvoltage + GetRandom(-0.5, 0.5);
            UserCAvoltage = UserCAvoltage + GetRandom(-0.5, 0.5);
            //电流
            UserAAelectric = UserAAelectric + GetRandom(-10, 10);
            UserBAelectric = UserBAelectric + GetRandom(-10, 10);
            UserCAelectric = UserCAelectric + GetRandom(-10, 10);

            if (UserAAvoltage < 200) UserAAvoltage = 200;
            if (UserBAvoltage < 200) UserBAvoltage = 200;
            if (UserCAvoltage < 200) UserCAvoltage = 200;

            if (UserAAvoltage > 240) UserAAvoltage = 240;
            if (UserBAvoltage > 240) UserBAvoltage = 240;
            if (UserCAvoltage > 240) UserCAvoltage = 240;

            if (UserAAelectric < 200) UserAAelectric = 200;
            if (UserBAelectric < 200) UserBAelectric = 200;
            if (UserCAelectric < 200) UserCAelectric = 200;

            if (UserAAelectric > 270) UserAAelectric = 270;
            if (UserBAelectric > 270) UserBAelectric = 270;
            if (UserCAelectric > 270) UserCAelectric = 270;

           
            if (DRWD < 32)
            {
                DRWD += GetRandom(0.2, 0.8);
            }

        }


        //得到视在功率 = 有功功率 + 无功功率..
        public (float, float, float) GetApparentpower(float newpowerfactor)
        {
            var result = GetActivePower();
            float szgl1 = result.Item1 / newpowerfactor;
            float szgl2 = result.Item2 / newpowerfactor;
            float szgl3 = result.Item3 / newpowerfactor;
            return (szgl1,szgl2,szgl3);
        }

        //得到有功功率这个也不会改变.... 
        public (float, float, float) GetActivePower()
        {
            float PA = (float)(UserAAvoltage * UserAAelectric ) * oldpowerfactor;
            float PB = (float)(UserBAvoltage * UserBAelectric ) * oldpowerfactor;
            float PC = (float)(UserCAvoltage * UserCAelectric ) * oldpowerfactor;

            return (PA / 1000, PB / 1000, PC / 1000);
        }

        /// <summary>
        /// 计算功率因数对应的tanφ
        /// </summary>
        /// <param name="cosPhi">功率因数（范围：0 ~ 1）</param>
        /// <returns>tanφ值</returns>
        public static double CalculateTanPhi(double cosPhi)
        {
            // 校验功率因数范围（必须在0~1之间）
            if (cosPhi < 0 || cosPhi > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(cosPhi), "功率因数必须在0到1之间");
            }

            // 计算相位角φ（弧度）：φ = arccos(cosφ)
            double phiRadians = Math.Acos(cosPhi);

            // 计算tanφ
            double tanPhi = Math.Tan(phiRadians);

            return tanPhi;
        }


        //得到无功功率 (提升功率因数，改变无功功率) Q = S平方 - P平方
        public (float, float, float) GetReactivePower(float newpowerfactor)
        {
            // 示例1：功率因数0.6对应的tanφ
            double cosPhi1 = newpowerfactor;
            double tanPhi1 = CalculateTanPhi(cosPhi1);
            var result = GetActivePower();
            return ((float)(result.Item1 * tanPhi1), (float)(result.Item2 * tanPhi1), (float)(result.Item3 * tanPhi1));
        }




        //返回新的电流 -- 这个会改变..
        public (float, float, float) GetAelectric(float newpowerfactor)
        {
            //公式为 初始有功功率. (得到老的P有功)                        
            float PA = UserAAvoltage * UserAAelectric * oldpowerfactor;
            float PB = UserBAvoltage * UserBAelectric * oldpowerfactor;
            float PC = UserCAvoltage * UserCAelectric * oldpowerfactor;
            return (PA / (UserAAvoltage * newpowerfactor), PB / (UserBAvoltage * newpowerfactor), PC / (UserCAvoltage * newpowerfactor));
        }       

        //A,B,C的谐波畸变..
        public (float,float,float) HarmonicVoltageRate(float newpowerfactor)
        {
            return ((1 - newpowerfactor) * 10, (1 - newpowerfactor) * 10, (1 - newpowerfactor) * 10);
        }


        //碳排放..1 2 3 (火)
        public (float, float, float)Carbon123(float newpowerfactor)
        {
            var result = GetAelectric(newpowerfactor);


            return ((float)((result.Item1 * UserAAvoltage ) * 0.8) / 1000, (float)(((result.Item2 * UserBAvoltage ) * 0.8) / 1000), (float)((result.Item3 * UserCAvoltage) * 0.8) / 1000);
        }

        //碳排放..4 5 6 (风) 0.03
        public (float, float, float) Carbon456(float newpowerfactor)
        {
            var result = GetApparentpower(newpowerfactor);
            return ((float)((result.Item1 * UserAAvoltage ) * 0.03)/1000, (float)(((result.Item2 * UserBAvoltage) * 0.03) / 1000), (float)((result.Item3 * UserCAvoltage) * 0.03) / 1000);
        }


        //碳排放..7 8 9 (光伏) 0.05
        public (float, float, float) Carbon789(float newpowerfactor)
        {
            var result = GetApparentpower(newpowerfactor);
            return ((float)((result.Item1 * UserAAvoltage ) * 0.05) / 1000, (float)(((result.Item2 * UserBAvoltage) * 0.05) / 1000), (float)((result.Item3 * UserCAvoltage) * 0.05) / 1000);
        }

        //碳排放..10 11 12 （水利） 0.03
        public (float, float, float) Carbon101112(float newpowerfactor)
        {
            var result = GetApparentpower(newpowerfactor);
            return ((float)((result.Item1 * UserAAvoltage ) * 0.03) / 1000, (float)(((result.Item2 * UserBAvoltage) * 0.03) / 1000), (float)((result.Item3 * UserCAvoltage) * 0.03) / 1000);
        }



    }
}
