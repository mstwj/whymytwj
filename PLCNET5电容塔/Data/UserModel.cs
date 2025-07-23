using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCNET5电容塔.Data
{
    public class UserModel : ObservableObject
    {
        private bool _q1_1, _q1_2, _q1_3, _q1_4, _q1_5, _q1_6;
        private bool _q2_1, _q2_2, _q2_3, _q2_4, _q2_5, _q2_6;
        private bool _q3_1, _q3_2, _q3_3, _q3_4, _q3_5, _q3_6;
        private int myw = 20,myh = 20,myfs=15;

        public int Myw { get { return myw; } set { myw = value; } } 
        public int Myh { get { return myh; } set { myh = value; } } 

        public int MyFS { get { return myfs; } set { myh = value; } }

        string _q1_1Image, _q1_2Image, _q1_3Image, _q1_4Image, _q1_5Image, _q1_6Image;
        string _q2_1Image, _q2_2Image, _q2_3Image, _q2_4Image, _q2_5Image, _q2_6Image;
        string _q3_1Image, _q3_2Image, _q3_3Image, _q3_4Image, _q3_5Image, _q3_6Image;

        public bool Q1_1 { get { return _q1_1; } set { SetProperty(ref _q1_1, value); string _imagesource = Q1_1Image; Q1_1Image = _imagesource; } }
        public bool Q1_2 { get { return _q1_2; } set { SetProperty(ref _q1_2, value); string _imagesource = Q1_2Image; Q1_2Image = _imagesource; } }
        public bool Q1_3 { get { return _q1_3; } set { SetProperty(ref _q1_3, value); string _imagesource = Q1_3Image; Q1_3Image = _imagesource; } }
        public bool Q1_4 { get { return _q1_4; } set { SetProperty(ref _q1_4, value); string _imagesource = Q1_4Image; Q1_4Image = _imagesource; } }
        public bool Q1_5 { get { return _q1_5; } set { SetProperty(ref _q1_5, value); string _imagesource = Q1_5Image; Q1_5Image = _imagesource; } }
        public bool Q1_6 { get { return _q1_6; } set { SetProperty(ref _q1_6, value); string _imagesource = Q1_6Image; Q1_6Image = _imagesource; } }

        public bool Q2_1 { get { return _q2_1; } set { SetProperty(ref _q2_1, value); string _imagesource = Q2_1Image; Q2_1Image = _imagesource; } }
        public bool Q2_2 { get { return _q2_2; } set { SetProperty(ref _q2_2, value); string _imagesource = Q2_2Image; Q2_2Image = _imagesource; } }
        public bool Q2_3 { get { return _q2_3; } set { SetProperty(ref _q2_3, value); string _imagesource = Q2_3Image; Q2_3Image = _imagesource; } }
        public bool Q2_4 { get { return _q2_4; } set { SetProperty(ref _q2_4, value); string _imagesource = Q2_4Image; Q2_4Image = _imagesource; } }
        public bool Q2_5 { get { return _q2_5; } set { SetProperty(ref _q2_5, value); string _imagesource = Q2_5Image; Q2_5Image = _imagesource; } }
        public bool Q2_6 { get { return _q2_6; } set { SetProperty(ref _q2_6, value); string _imagesource = Q2_6Image; Q2_6Image = _imagesource; } }

        public bool Q3_1 { get { return _q3_1; } set { SetProperty(ref _q3_1, value); string _imagesource = Q3_1Image; Q3_1Image = _imagesource; } }
        public bool Q3_2 { get { return _q3_2; } set { SetProperty(ref _q3_2, value); string _imagesource = Q3_2Image; Q3_2Image = _imagesource; } }
        public bool Q3_3 { get { return _q3_3; } set { SetProperty(ref _q3_3, value); string _imagesource = Q3_3Image; Q3_3Image = _imagesource; } }
        public bool Q3_4 { get { return _q3_4; } set { SetProperty(ref _q3_4, value); string _imagesource = Q3_4Image; Q3_4Image = _imagesource; } }
        public bool Q3_5 { get { return _q3_5; } set { SetProperty(ref _q3_5, value); string _imagesource = Q3_5Image; Q3_5Image = _imagesource; } }
        public bool Q3_6 { get { return _q3_6; } set { SetProperty(ref _q3_6, value); string _imagesource = Q3_6Image; Q3_6Image = _imagesource; } }


        public string Q1_1Image { get { return _q1_1 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q1_1Image, value); } }
        public string Q1_2Image { get { return _q1_2 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q1_2Image, value); } }
        public string Q1_3Image { get { return _q1_3 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q1_3Image, value); } }
        public string Q1_4Image { get { return _q1_4 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q1_4Image, value); } }
        public string Q1_5Image { get { return _q1_5 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q1_5Image, value); } }
        public string Q1_6Image { get { return _q1_6 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q1_6Image, value); } }

        public string Q2_1Image { get { return _q2_1 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q2_1Image, value); } }
        public string Q2_2Image { get { return _q2_2 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q2_2Image, value); } }
        public string Q2_3Image { get { return _q2_3 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q2_3Image, value); } }
        public string Q2_4Image { get { return _q2_4 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q2_4Image, value); } }
        public string Q2_5Image { get { return _q2_5 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q2_5Image, value); } }
        public string Q2_6Image { get { return _q2_6 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q2_6Image, value); } }

        public string Q3_1Image { get { return _q3_1 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q3_1Image, value); } }
        public string Q3_2Image { get { return _q3_2 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q3_2Image, value); } }
        public string Q3_3Image { get { return _q3_3 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q3_3Image, value); } }
        public string Q3_4Image { get { return _q3_4 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q3_4Image, value); } }
        public string Q3_5Image { get { return _q3_5 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q3_5Image, value); } }
        public string Q3_6Image { get { return _q3_6 ? "pack://application:,,,/Asset/mypass.png" : "pack://application:,,,/Asset/mybock.png"; } set { SetProperty(ref _q3_6Image, value); } }



    }
}
