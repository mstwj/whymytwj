using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 空载负载.Model
{
    public class UserCommunicationModel : ObservableObject
    {
        private bool _imageshebei1 = false;

        public bool Image1 { get => _imageshebei1; set { SetProperty(ref _imageshebei1, value); ImageShebei1 = "0"; } }

        private string imageshebei1;
        public string ImageShebei1 { get => _imageshebei1 ? imageshebei1 = "pack://application:,,,/Asset/green-fd.png" : imageshebei1 = "pack://application:,,,/Asset/black-fd.png"; set { SetProperty(ref imageshebei1, value); } }


        private bool _imageshebei2 = false;
        public bool Image2 { get => _imageshebei2; set { SetProperty(ref _imageshebei2, value); ImageShebei2 = "0"; } }

        private string imageshebei2;
        public string ImageShebei2 { get => _imageshebei2 ? imageshebei2 = "pack://application:,,,/Asset/green-fd.png" : imageshebei2 = "pack://application:,,,/Asset/black-fd.png"; set { SetProperty(ref imageshebei2, value); } }

    }
}
