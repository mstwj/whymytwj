using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 空载_负载.Model
{
    public class UserStandardModel : ObservableObject
    {
        
        private string productType;
        private string productTuhao;
        private string lossStandard;
        private string lossStandardUp;
        private string lossStandardDown;

        private string noloadCurrentStandard;
        private string noloadCurrentStandardUp;
        private string noloadCurrentStandardDown;

        private string impedanceStandard;
        //产品型号
        public string ProductType { get => productType; set { SetProperty(ref productType, value); } }
        //图号
        public string ProductTuhao { get => productTuhao; set { SetProperty(ref productTuhao, value); } }

        //损耗..
        public string LossStandard { get => lossStandard; set { SetProperty(ref lossStandard, value); } }
        //额定电压
        public string LossStandardUp { get => lossStandardUp; set { SetProperty(ref lossStandardUp, value); } }
        //损耗..上限
        public string LossStandardDown { get => lossStandardDown; set { SetProperty(ref lossStandardDown, value); } }
        ////损耗..下限
        public string NoloadCurrentStandard { get => noloadCurrentStandard; set { SetProperty(ref noloadCurrentStandard, value); } }
        ///百分比彪子
        public string NoloadCurrentStandardUp { get => noloadCurrentStandardUp; set { SetProperty(ref noloadCurrentStandardUp, value); } }
        ///百分比上
        public string NoloadCurrentStandardDown { get => noloadCurrentStandardDown; set { SetProperty(ref noloadCurrentStandardDown, value); } }
        ///百分比下..
        public string ImpedanceStandard { get => impedanceStandard; set { SetProperty(ref impedanceStandard, value); } }

    }
}
