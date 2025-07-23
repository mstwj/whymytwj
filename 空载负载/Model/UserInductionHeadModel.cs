using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 空载负载.Model
{
    public class UserInductionHeadModel : ObservableObject
    {
        private string productType = string.Empty;
        private string productTuhao = string.Empty;
        private string voltage = string.Empty;
        private string frequency = string.Empty;
        private string times = string.Empty;

        public string ProductType { get => productType; set { SetProperty(ref productType, value); } }
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value); } }
        public string Voltage { get => voltage; set { SetProperty(ref voltage, value); } }
        public string Frequency { get => frequency; set { SetProperty(ref frequency, value); } }
        public string Times { get => times; set { SetProperty(ref times, value); } }



    }
}
