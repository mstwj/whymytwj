using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 铁芯实验.Model
{
    public class UserRecordModel : ObservableObject
    {
        private float aTPVoltage;
        private float rMSvalue;
        private float iRMSvalue;
        private float noloadCurrent2 = 0.00f;  
        private float noloadCurrent = 0.00f; 
        private float percentageNoloadCurrent;
        private float noloadLoss;
        private string qualifiedJudgment = "不合格";

        //三相电压平均值
        public float ATPVoltage { get => aTPVoltage; set { SetProperty(ref aTPVoltage, value); } }

        //三相电压有效值 
        public float RMSvalue { get => rMSvalue; set { SetProperty(ref rMSvalue, value); } }

        //三相电流有效值
        public float IRMSvalue { get => iRMSvalue; set { SetProperty(ref iRMSvalue, value); } }

        //额定电流 
        public float NoloadCurrent { get => noloadCurrent; set { SetProperty(ref noloadCurrent, value); } }

        //空载电流 
        public float NoloadCurrent2 { get => noloadCurrent2; set { SetProperty(ref noloadCurrent2, value); } }

        //空载电流% 
        public float PercentageNoloadCurrent { get => percentageNoloadCurrent; set { SetProperty(ref percentageNoloadCurrent, value); } }
        //空载损耗
        public float NoloadLoss { get => noloadLoss; set { SetProperty(ref noloadLoss, value); } }

        //合格
        public string QualifiedJudgment { get => qualifiedJudgment; set { SetProperty(ref qualifiedJudgment, value); } }
    }
}
