using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 空载负载.Model
{
    public class UserInductionRecordModel : ObservableObject
    {
        private float voltage;
        private float rMSvalue;
        private float frequency;
        private string times;
        private string qualifiedJudgment = "不合格";

        //三相电压平均值
        public float Voltage { get => voltage; set { SetProperty(ref voltage, value); } }

        //三相电压有效值 
        public float RMSvalue { get => rMSvalue; set { SetProperty(ref rMSvalue, value); } }

        //功率
        public float Frequency { get => frequency; set { SetProperty(ref frequency, value); } }


        //时间
        public string Times { get => times; set { SetProperty(ref times, value); } }
        //空载损耗

        //合格
        public string QualifiedJudgment { get => qualifiedJudgment; set { SetProperty(ref qualifiedJudgment, value); } }

    }
}
