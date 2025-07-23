using CommunityToolkit.Mvvm.ComponentModel;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PLCNET5_11_9.Data
{
    public class MainModel : ObservableObject
    {

        private bool _plc31, _plc33, _plc35,_plc37;
        private bool _s2000_1z;
        private bool _s7500_1g;
        private bool _s2000_1t;
        private bool _s2000_1tlw;
        private bool _zbsr;
        private bool _zcb;
        //private bool _sy1, _sy2, _sy3, _sy4, _sy5, _sy6, _sy7, _sy8, _sy9, _sy10, _sy11, _sy12, _sy13;
        //private bool _sgw1, _sgw2, _sgw3;
        //private int _ai_data;
        
        private float _biao1_data1 = 9999.99f, _biao1_data2, _biao1_data3;
        private float _biao2_data1 = 9999.99f, _biao2_data2, _biao2_data3;
        private float _biao3_data1 = 9999.99f, _biao3_data2, _biao3_data3;
        private float _biao4_data1 = 9999.99f, _biao4_data2, _biao4_data3;
        private float _biao5_data1 = 9999.99f, _biao5_data2, _biao5_data3;
        private float _biao6_data1 = 9999.99f, _biao6_data2, _biao6_data3;


        private string _plc31Image, _plc33Image, _plc35Image, _plc37Image;
        private string _s2000_1zImage;
        private string _s7500_1gImage;
        private string _s2000_1tImage;
        private string _s2000_1tlwImage;
        private string _zbsrImage;
        private string _zcbImage;
        //private string _sy1Image, _sy2Image, _sy3Image, _sy4Image, _sy5Image, _sy6Image, _sy7Image, _sy8Image, _sy9Image, _sy10Image, _sy11Image, _sy12Image, _sy13Image;
        //private string _sgw1Image, _sgw2Image, _sgw3Image;

        //public int Ai_data { get { return _ai_data; } set { SetProperty(ref _ai_data, value); } }

        public float Biao1_data1 { get { return _biao1_data1; } set { SetProperty(ref _biao1_data1, value); } }
        public float Biao2_data1 { get { return _biao2_data1; } set { SetProperty(ref _biao2_data1, value); } }
        public float Biao3_data1 { get { return _biao3_data1; } set { SetProperty(ref _biao3_data1, value); } }
        public float Biao4_data1 { get { return _biao4_data1; } set { SetProperty(ref _biao4_data1, value); } }
        public float Biao5_data1 { get { return _biao5_data1; } set { SetProperty(ref _biao5_data1, value); } }
        public float Biao6_data1 { get { return _biao6_data1; } set { SetProperty(ref _biao6_data1, value); } }

        public float Biao1_data2 { get { return _biao1_data2; } set { SetProperty(ref _biao1_data2, value); } }
        public float Biao2_data2 { get { return _biao2_data2; } set { SetProperty(ref _biao2_data2, value); } }
        public float Biao3_data2 { get { return _biao3_data2; } set { SetProperty(ref _biao3_data2, value); } }
        public float Biao4_data2 { get { return _biao4_data2; } set { SetProperty(ref _biao4_data2, value); } }
        public float Biao5_data2 { get { return _biao5_data2; } set { SetProperty(ref _biao5_data2, value); } }
        public float Biao6_data2 { get { return _biao6_data2; } set { SetProperty(ref _biao6_data2, value); } }

        public float Biao1_data3 { get { return _biao1_data3; } set { SetProperty(ref _biao1_data3, value); } }
        public float Biao2_data3 { get { return _biao2_data3; } set { SetProperty(ref _biao2_data3, value); } }
        public float Biao3_data3 { get { return _biao3_data3; } set { SetProperty(ref _biao3_data3, value); } }
        public float Biao4_data3 { get { return _biao4_data3; } set { SetProperty(ref _biao4_data3, value); } }
        public float Biao5_data3 { get { return _biao5_data3; } set { SetProperty(ref _biao5_data3, value); } }
        public float Biao6_data3 { get { return _biao6_data3; } set { SetProperty(ref _biao6_data3, value); } }



        public bool Plc31 { get { return _plc31; } set { SetProperty(ref _plc31, value); string _imagesource = Plc31Image; Plc31Image = _imagesource; } }
        public bool Plc33 { get { return _plc33; } set { SetProperty(ref _plc33, value); string _imagesource = Plc33Image; Plc33Image = _imagesource; } }
        public bool Plc35 { get { return _plc35; } set { SetProperty(ref _plc35, value); string _imagesource = Plc35Image; Plc35Image = _imagesource; } }
        public bool Plc37 { get { return _plc37; } set { SetProperty(ref _plc37, value); string _imagesource = Plc37Image; Plc37Image = _imagesource; } }


        //2000 中频
        public bool S2000_1z { get { return _s2000_1z; } set { SetProperty(ref _s2000_1z, value); string _imagesource = S2000_1zImage; S2000_1zImage = _imagesource; } }
    
        //2000 调压器
        public bool S2000_1t { get { return _s2000_1t; } set { SetProperty(ref _s2000_1t, value); string _imagesource = S2000_1tImage; S2000_1tImage = _imagesource; } }
        //7500 工频
        public bool S7500_1g { get { return _s7500_1g; } set { SetProperty(ref _s7500_1g, value); string _imagesource = S7500_1gImage; S7500_1gImage = _imagesource; } }

        //2000 调压器零位
        public bool S2000_1tlw { get { return _s2000_1tlw; } set { SetProperty(ref _s2000_1tlw, value); string _imagesource = S2000_1tlwImage; S2000_1tlwImage = _imagesource; } }

        //中间变
        public bool SZBSR { get { return _zbsr; } set { SetProperty(ref _zbsr, value); string _imagesource = SZBSR_Image; SZBSR_Image = _imagesource; } }

        //支持变
        public bool ZCB { get { return _zcb; } set { SetProperty(ref _zcb, value); string _imagesource = ZCB_Image; ZCB_Image = _imagesource; } }



        public string Plc31Image { get { return _plc31 ? "pack://application:,,,/Asset/an7.png" : "pack://application:,,,/Asset/fd3.png"; } set { SetProperty(ref _plc31Image, value); } }

        public string Plc33Image { get { return _plc33 ? "pack://application:,,,/Asset/an7.png" : "pack://application:,,,/Asset/fd3.png"; } set { SetProperty(ref _plc33Image, value); } }

        public string Plc35Image { get { return _plc35 ? "pack://application:,,,/Asset/an7.png" : "pack://application:,,,/Asset/fd3.png"; } set { SetProperty(ref _plc35Image, value); } }

        public string Plc37Image { get { return _plc37 ? "pack://application:,,,/Asset/an7.png" : "pack://application:,,,/Asset/fd3.png"; } set { SetProperty(ref _plc37Image, value); } }


        //7500故障..
        public string S7500_1gImage { get { return _s7500_1g ? "pack://application:,,,/Asset/an8.png" : "pack://application:,,,/Asset/fd3.png"; } set { SetProperty(ref _s7500_1gImage, value); } }
        //2000故障
        public string S2000_1zImage { get { return _s2000_1z ? "pack://application:,,,/Asset/an8.png" : "pack://application:,,,/Asset/fd3.png"; } set { SetProperty(ref _s2000_1zImage, value); } }
        //2000t故障
        public string S2000_1tImage { get { return _s2000_1t ? "pack://application:,,,/Asset/an8.png" : "pack://application:,,,/Asset/fd3.png"; } set { SetProperty(ref _s2000_1tImage, value); } }


        //2000t零位
        public string S2000_1tlwImage { get { return _s2000_1tlw ? "pack://application:,,,/Asset/hs3.png" : "pack://application:,,,/Asset/fd3.png"; } set { SetProperty(ref _s2000_1tlwImage, value); } }



        //中变输入故障
        public string SZBSR_Image { get { return _zbsr ? "pack://application:,,,/Asset/an8.png" : "pack://application:,,,/Asset/fd3.png"; } set { SetProperty(ref _zbsrImage, value); } }

        //支持故障
        public string ZCB_Image { get { return _zcb ? "pack://application:,,,/Asset/an8.png" : "pack://application:,,,/Asset/fd3.png"; } set { SetProperty(ref _zcbImage, value); } }

    }
}
