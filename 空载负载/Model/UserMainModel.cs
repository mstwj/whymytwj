using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 空载负载.Model
{
    public class UserMainModel : ObservableObject
    {
        private float umn_ab, umn_bc, umn_ca;
        private float urms_ab, urms_bc, urms_ca;
        private float irms_ab, irms_bc, irms_ca;
        private float p_ab, p_bc, p_ca;
        private float urms_vc, irms_ic, is_pv, pow_loss, flt_ukIopercent, flt_cosq;

        public float Umn_ab { get => umn_ab; set { SetProperty(ref umn_ab, value); } }
        public float Umn_bc { get => umn_bc; set { SetProperty(ref umn_bc, value); } }
        public float Umn_ca { get => umn_ca; set { SetProperty(ref umn_ca, value); } }

        public float Urms_ab { get => urms_ab; set { SetProperty(ref urms_ab, value); } }
        public float Urms_bc { get => urms_bc; set { SetProperty(ref urms_bc, value); } }
        public float Urms_ca { get => urms_ca; set { SetProperty(ref urms_ca, value); } }

        public float Irms_ab { get => irms_ab; set { SetProperty(ref irms_ab, value); } }
        public float Irms_bc { get => irms_bc; set { SetProperty(ref irms_bc, value); } }
        public float Irms_ca { get => irms_ca; set { SetProperty(ref irms_ca, value); } }

        public float P_ab { get => p_ab; set { SetProperty(ref p_ab, value); } }
        public float P_bc { get => p_bc; set { SetProperty(ref p_bc, value); } }
        public float P_ca { get => p_ca; set { SetProperty(ref p_ca, value); } }


        public float Urms_vc { get => urms_vc; set { SetProperty(ref urms_vc, value); } }
        public float Irms_ic { get => irms_ic; set { SetProperty(ref irms_ic, value); } }
        public float Is_pv { get => is_pv; set { SetProperty(ref is_pv, value); } }
        public float Pow_loss { get => pow_loss; set { SetProperty(ref pow_loss, value); } }

        //百分比..
        public float flt_UkIoPercent { get => flt_ukIopercent; set { SetProperty(ref flt_ukIopercent, value); } }


        public float flt_CosQ { get => flt_cosq; set { SetProperty(ref flt_cosq, value); } }

    }
}
