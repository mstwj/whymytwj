using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 铁芯实验.Model
{
    public class UserInfoModel : ObservableObject
    {
        private string productNumber;
        //产品型号
        private string productType;
        //图号
        private string productTuhao;
        //容量
        private string productCapacity;
        //额定电压
        private string ratedVoltage;
        //相数
        private string phaseNumber;
        public string ProductNumber { get => productNumber; set { SetProperty(ref productNumber, value); } }
        //产品型号
        public string ProductType { get => productType; set { SetProperty(ref productType, value); } }
        //图号
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value); } }

        //容量
        public string ProductCapacity { get => productCapacity; set { SetProperty(ref productCapacity, value); } }
        //额定电压
        public string RatedVoltage { get => ratedVoltage; set { SetProperty(ref ratedVoltage, value); } }
        //相数
        public string PhaseNumber { get => phaseNumber; set { SetProperty(ref phaseNumber, value); } }



    }
}
