using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 铁芯实验.Model
{
    public class UserStandardModel : ObservableObject
    {
        private string productType = string.Empty;
        private string productTuhao = string.Empty;
        private string productStandard = string.Empty;
        private string productStandardUpperimit = string.Empty;
        private string productStandardDownimit = string.Empty;
        private string productCurrentStandard = string.Empty;
        private string productCurrentStandardUpperrimit = string.Empty;
        private string productCurrentStandardDownimit = string.Empty;

        public string ProductType { get => productType; set { SetProperty(ref productType, value); } }
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value); } }
        public string ProductStandard { get => productStandard; set { SetProperty(ref productStandard, value); } }
        public string ProductStandardUpperimit { get => productStandardUpperimit; set { SetProperty(ref productStandardUpperimit, value); } }
        public string ProductStandardDownimit { get => productStandardDownimit; set { SetProperty(ref productStandardDownimit, value); } }
        public string ProductCurrentStandard { get => productCurrentStandard; set { SetProperty(ref productCurrentStandard, value); } }
        public string ProductCurrentStandardUpperrimit { get => productCurrentStandardUpperrimit; set { SetProperty(ref productCurrentStandardUpperrimit, value); } }
        public string ProductCurrentStandardDownimit { get => productCurrentStandardDownimit; set { SetProperty(ref productCurrentStandardDownimit, value); } }

    }
}
