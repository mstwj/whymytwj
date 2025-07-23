using CommunityToolkit.Mvvm.ComponentModel;
using S7.Net.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PLCNET5电容塔.Data
{
    public class MainModel : ObservableObject
    {
        private bool _plc39, _plc41;
        private string _plcImage39, _qlcImage41;

        public bool IsPlc39 { get { return _plc39; } set { SetProperty(ref _plc39, value); string _imagesource = Plc39BackgroundImage; Plc39BackgroundImage = _imagesource; } }

        public bool IsPlc41 { get { return _plc41; } set { SetProperty(ref _plc41, value); string _imagesource = Plc41BackgroundImage; Plc41BackgroundImage = _imagesource; } }

        public string Plc39BackgroundImage { get { return _plc39 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _plcImage39, value); } }

        public string Plc41BackgroundImage { get { return _plc41 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _qlcImage41, value); } }

    }
}
