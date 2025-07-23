using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 铁芯实验.Model
{
    public class PowerStateModel : ObservableObject
    {
        //电源状态 -- 0 1 2 3 4
        private string powerstate;
        public string Powerstate { get => powerstate; set { SetProperty(ref powerstate, value); } }

        //电源高低
        private string powergaodi;
        public string Powergaodi { get => powergaodi; set { SetProperty(ref powergaodi, value); } }

    }
}
