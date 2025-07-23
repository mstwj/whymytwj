using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 空载负载.Model
{
    public  class UserLoadHeadModel : ObservableValidator
    {

        private string productType = string.Empty;
        private string productTuhao = string.Empty;
        private string loadloss = string.Empty;
        private string loadlossUpperimit = string.Empty;
        private string loadlossDownimit = string.Empty;
        private string loadMainReactance = string.Empty;
        private string loadMainReactanceUpperrimit = string.Empty;
        private string loadMainReactanceDownimit = string.Empty;

        public string ProductType { get => productType; set { SetProperty(ref productType, value); } }
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value); } }
        public string Loadloss { get => loadloss; set { SetProperty(ref loadloss, value); } }
        public string LoadlossUpperimit { get => loadlossUpperimit; set { SetProperty(ref loadlossUpperimit, value); } }
        public string LoadlossDownimit { get => loadlossDownimit; set { SetProperty(ref loadlossDownimit, value); } }
        public string LoadMainReactance { get => loadMainReactance; set { SetProperty(ref loadMainReactance, value); } }
        public string LoadMainReactanceUpperrimit { get => loadMainReactanceUpperrimit; set { SetProperty(ref loadMainReactanceUpperrimit, value); } }
        public string LoadMainReactanceDownimit { get => loadMainReactanceDownimit; set { SetProperty(ref loadMainReactanceDownimit, value); } }

    }
}
