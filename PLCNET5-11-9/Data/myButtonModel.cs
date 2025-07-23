using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCNET5_11_9.Data
{
    public class myButtonModel : ObservableObject
    {
        private bool _data;
        private int _shu = 0;        
        private string _imagedata;

        public bool Data { get { return _data; } set { SetProperty(ref _data, value); string _imagesource = DataImage; DataImage = _imagesource; } }        
        public int Shu { get { return _shu; } set { SetProperty(ref _shu, value); string _imagesource = DataImage; DataImage = _imagesource; } }
    

        //
        public string DataImage { get 
            {
                if (_shu == 0) return _data ? "pack://application:,,,/Asset/zhixian2.png" : "pack://application:,,,/Asset/xiexian2.png";
                if (_shu == 1) return _data ? "pack://application:,,,/Asset/zhixian.png" : "pack://application:,,,/Asset/xiexian.png";
                if (_shu == 2) return _data ? "pack://application:,,,/Asset/zhixian3.png" : "pack://application:,,,/Asset/xiexian3.png";                
                return _data ? "pack://application:,,,/Asset/zhixian2.png" : "pack://application:,,,/Asset/xiexian2.png";
            }
            set
            {
                SetProperty(ref _imagedata, value); 
            }
        }


    }
}
