using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 空载_负载.Model
{
    public class UserInfoModel : ObservableObject
    {
        private string productNumber;
        private string productType;
        private string productTuhao;
        private string highpressure;
        private string highcurrent;
        private string lowpressure;
        private string lowcurrent;
        public string ProductNumber { get => productNumber; set { SetProperty(ref productNumber, value); } }
        //产品型号
        public string ProductType { get => productType; set { SetProperty(ref productType, value); } }
        //图号
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value); } }

        //容量
        public string Highpressure { get => highpressure; set { SetProperty(ref highpressure, value); } }
        //额定电压
        public string Highcurrent { get => highcurrent; set { SetProperty(ref highcurrent, value); } }
        //相数
        public string Lowpressure { get => lowpressure; set { SetProperty(ref lowpressure, value); } }
        public string Lowcurrent { get => lowcurrent; set { SetProperty(ref lowcurrent, value); } }

    }
}
