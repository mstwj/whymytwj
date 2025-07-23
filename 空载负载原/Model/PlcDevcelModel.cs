using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 空载_负载.Model   
{ 
    public class PlcDevcelModel : ObservableObject
    {
        private bool _plcdic3 ;
        private string _plcdic3Image;
        public bool Plcidic3 { get { return _plcdic3; } set { SetProperty(ref _plcdic3, value); string _imagesource = PlcDIc3Image; PlcDIc3Image = _imagesource; } }
        public string PlcDIc3Image { get { return _plcdic3 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdic3Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdic4;
        private string _plcdic4Image;
        public bool Plcidic4 { get { return _plcdic4; } set { SetProperty(ref _plcdic4, value); string _imagesource = PlcDIc4Image; PlcDIc4Image = _imagesource; } }
        public string PlcDIc4Image { get { return _plcdic4 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdic4Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdic5;
        private string _plcdic5Image;
        public bool Plcidic5 { get { return _plcdic5; } set { SetProperty(ref _plcdic5, value); string _imagesource = PlcDIc5Image; PlcDIc5Image = _imagesource; } }
        public string PlcDIc5Image { get { return _plcdic5 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdic5Image, value); } }


        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdic6;
        private string _plcdic6Image;
        public bool Plcidic6 { get { return _plcdic6; } set { SetProperty(ref _plcdic6, value); string _imagesource = PlcDIc6Image; PlcDIc6Image = _imagesource; } }
        public string PlcDIc6Image { get { return _plcdic6 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdic6Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdic7;
        private string _plcdic7Image;
        public bool Plcidic7 { get { return _plcdic7; } set { SetProperty(ref _plcdic7, value); string _imagesource = PlcDIc7Image; PlcDIc7Image = _imagesource; } }
        public string PlcDIc7Image { get { return _plcdic7 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdic7Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdid0;
        private string _plcdid0Image;
        public bool Plcidid0 { get { return _plcdid0; } set { SetProperty(ref _plcdid0, value); string _imagesource = PlcDId0Image; PlcDId0Image = _imagesource; } }
        public string PlcDId0Image { get { return _plcdid0 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdid0Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdid1;
        private string _plcdid1Image;
        public bool Plcidid1 { get { return _plcdid1; } set { SetProperty(ref _plcdid1, value); string _imagesource = PlcDId1Image; PlcDId1Image = _imagesource; } }
        public string PlcDId1Image { get { return _plcdid1 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdid1Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdid2;
        private string _plcdid2Image;
        public bool Plcidid2 { get { return _plcdid2; } set { SetProperty(ref _plcdid2, value); string _imagesource = PlcDId2Image; PlcDId2Image = _imagesource; } }
        public string PlcDId2Image { get { return _plcdid2 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdid2Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdid3;
        private string _plcdid3Image;
        public bool Plcidid3 { get { return _plcdid3; } set { SetProperty(ref _plcdid3, value); string _imagesource = PlcDId3Image; PlcDId3Image = _imagesource; } }
        public string PlcDId3Image { get { return _plcdid3 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdid3Image, value); } }



        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdid4;
        private string _plcdid4Image;
        public bool Plcidid4 { get { return _plcdid4; } set { SetProperty(ref _plcdid4, value); string _imagesource = PlcDId4Image; PlcDId4Image = _imagesource; } }
        public string PlcDId4Image { get { return _plcdid4 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdid4Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdid5;
        private string _plcdid5Image;
        public bool Plcidid5 { get { return _plcdid5; } set { SetProperty(ref _plcdid5, value); string _imagesource = PlcDId5Image; PlcDId5Image = _imagesource; } }
        public string PlcDId5Image { get { return _plcdid5 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdid5Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdid6;
        private string _plcdid6Image;
        public bool Plcidid6 { get { return _plcdid6; } set { SetProperty(ref _plcdid6, value); string _imagesource = PlcDId6Image; PlcDId6Image = _imagesource; } }
        public string PlcDId6Image { get { return _plcdid6 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdid6Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdid7;
        private string _plcdid7Image;
        public bool Plcidid7 { get { return _plcdid7; } set { SetProperty(ref _plcdid7, value); string _imagesource = PlcDId7Image; PlcDId7Image = _imagesource; } }
        public string PlcDId7Image { get { return _plcdid7 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdid7Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdie0;
        private string _plcdie0Image;
        public bool Plcidie0 { get { return _plcdie0; } set { SetProperty(ref _plcdie0, value); string _imagesource = PlcDIe0Image; PlcDIe0Image = _imagesource; } }
        public string PlcDIe0Image { get { return _plcdie0 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdie0Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdie1;
        private string _plcdie1Image;
        public bool Plcidie1 { get { return _plcdie1; } set { SetProperty(ref _plcdie1, value); string _imagesource = PlcDIe1Image; PlcDIe1Image = _imagesource; } }
        public string PlcDIe1Image { get { return _plcdie1 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdie1Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdie2;
        private string _plcdie2Image;
        public bool Plcidie2 { get { return _plcdie2; } set { SetProperty(ref _plcdie2, value); string _imagesource = PlcDIe2Image; PlcDIe2Image = _imagesource; } }
        public string PlcDIe2Image { get { return _plcdie2 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdie2Image, value); } }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////
        /// </summary>
        private bool _plcdie3;
        private string _plcdie3Image;
        public bool Plcidie3 { get { return _plcdie3; } set { SetProperty(ref _plcdie3, value); string _imagesource = PlcDIe3Image; PlcDIe3Image = _imagesource; } }
        public string PlcDIe3Image { get { return _plcdie3 ? "pack://application:,,,/Asset/green-fd.png" : "pack://application:,,,/Asset/black-fd.png"; } set { SetProperty(ref _plcdie3Image, value); } }

    }
}
