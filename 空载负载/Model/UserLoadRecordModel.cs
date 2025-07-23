using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 空载负载.Model
{
    public class UserLoadRecordModel : ObservableObject
    {
        private float aTPVoltage;
        private float rMSvalue;
        private float iRMSvalue;
        private float fHzSvalue;
        

        private float fltUkIoPercent;
        private float fltUkt;
        private float fltZt;       

        private string qualifiedJudgment = "不合格";

        //三相电压平均值
        public float ATPVoltage { get => aTPVoltage; set { SetProperty(ref aTPVoltage, value); } }

        //三相电压有效值 
        public float RMSvalue { get => rMSvalue; set { SetProperty(ref rMSvalue, value); } }

        //三相电流有效值
        public float IRMSvalue { get => iRMSvalue; set { SetProperty(ref iRMSvalue, value); } }

        public float FHzSvalue { get => fHzSvalue; set { SetProperty(ref fHzSvalue, value); } }


        public float FltUkIoPercent { get => fltUkIoPercent; set { SetProperty(ref fltUkIoPercent, value); } }
        public float FltUkt { get => fltUkt; set { SetProperty(ref fltUkt, value); } }
        public float FltZt { get => fltZt; set { SetProperty(ref fltZt, value); } }        

        //合格
        public string QualifiedJudgment { get => qualifiedJudgment; set { SetProperty(ref qualifiedJudgment, value); } }
    }
}
