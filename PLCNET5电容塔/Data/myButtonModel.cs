using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCNET5电容塔.Data
{
    public class myButtonModel : ObservableObject
    {
        private bool _data,_shu;        
        private string _imagedata;

        public bool Data { get { return _data; } set { SetProperty(ref _data, value); string _imagesource = DataImage; DataImage = _imagesource; } }
        public bool Shu { get { return _shu; } set { SetProperty(ref _shu, value); string _imagesource = DataImage; DataImage = _imagesource; } }
        //
        public string DataImage { get 
            {
                if (_shu == false) return _data ? "pack://application:,,,/Asset/zhixian2.png" : "pack://application:,,,/Asset/xiexian2.png";
                else  return _data ? "pack://application:,,,/Asset/zhixian.png" : "pack://application:,,,/Asset/xiexian.png";
            }
            set
            {
                SetProperty(ref _imagedata, value); 
            }
        }


    }
}
