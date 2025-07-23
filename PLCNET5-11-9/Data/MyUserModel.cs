using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCNET5_11_9.Data
{
    class MyUserModel : ObservableObject
    {        
        private float _data1 = 0.5f, _data2 = 0.8f, _data3 = 1.3f;

        public float Data1 { get { return _data1; } set { SetProperty(ref _data1, value); } }
        public float Data2 { get { return _data2; } set { SetProperty(ref _data2, value); } }
        public float Data3 { get { return _data3; } set { SetProperty(ref _data3, value); } }

    }
}
