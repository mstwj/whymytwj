using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 维通利智耐压台.Model
{
    public class UserMainControlModel : ObservableObject
    {
        private bool _imageshebei1 = false;
        public bool Image1 { get => _imageshebei1; set { SetProperty(ref _imageshebei1, value); ImageShebei1 = "0"; } }

        private string imageshebei1;
        public string ImageShebei1 { get => _imageshebei1 ? imageshebei1 = "pack://application:,,,/Asset/1.png" : imageshebei1 = "pack://application:,,,/Asset/2.png"; set { SetProperty(ref imageshebei1, value); } }

        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>

        private bool _imageshebei2 = false;
        public bool Image2 { get => _imageshebei2; set { SetProperty(ref _imageshebei2, value); ImageShebei2 = "0"; } }

        private string imageshebei2;
        public string ImageShebei2 { get => _imageshebei2 ? imageshebei2 = "pack://application:,,,/Asset/1.png" : imageshebei2 = "pack://application:,,,/Asset/2.png"; set { SetProperty(ref imageshebei2, value); } }

        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>

        private bool _imageshebei3 = false;
        public bool Image3 { get => _imageshebei3; set { SetProperty(ref _imageshebei3, value); ImageShebei3 = "0"; } }

        private string imageshebei3;
        public string ImageShebei3 { get => _imageshebei3 ? imageshebei3 = "pack://application:,,,/Asset/1.png" : imageshebei3 = "pack://application:,,,/Asset/2.png"; set { SetProperty(ref imageshebei3, value); } }


        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>

        private bool _imageshebei4 = false;
        public bool Image4 { get => _imageshebei4; set { SetProperty(ref _imageshebei4, value); ImageShebei4 = "0"; } }

        private string imageshebei4;
        public string ImageShebei4 { get => _imageshebei4 ? imageshebei4 = "pack://application:,,,/Asset/1.png" : imageshebei4 = "pack://application:,,,/Asset/2.png"; set { SetProperty(ref imageshebei4, value); } }


        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>

        private bool _imageshebei5 = false;
        public bool Image5 { get => _imageshebei5; set { SetProperty(ref _imageshebei5, value); ImageShebei5 = "0"; } }

        private string imageshebei5;
        public string ImageShebei5 { get => _imageshebei5 ? imageshebei5 = "pack://application:,,,/Asset/1.png" : imageshebei5 = "pack://application:,,,/Asset/2.png"; set { SetProperty(ref imageshebei5, value); } }


        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>

        private bool _imageshebei6 = false;
        public bool Image6 { get => _imageshebei6; set { SetProperty(ref _imageshebei6, value); ImageShebei6 = "0"; } }

        private string imageshebei6;
        public string ImageShebei6 { get => _imageshebei6 ? imageshebei6 = "pack://application:,,,/Asset/1.png" : imageshebei6 = "pack://application:,,,/Asset/2.png"; set { SetProperty(ref imageshebei6, value); } }


        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>

        private bool _imageshebei7 = false;
        public bool Image7 { get => _imageshebei7; set { SetProperty(ref _imageshebei7, value); ImageShebei7 = "0"; } }

        private string imageshebei7;
        public string ImageShebei7 { get => _imageshebei7 ? imageshebei7 = "pack://application:,,,/Asset/1.png" : imageshebei7 = "pack://application:,,,/Asset/2.png"; set { SetProperty(ref imageshebei7, value); } }


        /// <summary>
        /// ////////////////////////////////////////////////////
        /// </summary>

        private bool _imageshebei0 = false;
        public bool Image0 { get => _imageshebei0; set { SetProperty(ref _imageshebei0, value); ImageShebei0 = "0"; } }

        private string imageshebei0;
        public string ImageShebei0 { get => _imageshebei0 ? imageshebei0 = "pack://application:,,,/Asset/1.png" : imageshebei0 = "pack://application:,,,/Asset/2.png"; set { SetProperty(ref imageshebei0, value); } }



        private bool _imageshebei8 = false;
        public bool Image8 { get => _imageshebei8; set { SetProperty(ref _imageshebei8, value); ImageShebei8 = "0"; } }

        private string imageshebei8;
        public string ImageShebei8 { get => _imageshebei8 ? imageshebei8 = "pack://application:,,,/Asset/up1.png" : imageshebei8 = "pack://application:,,,/Asset/up2.png"; set { SetProperty(ref imageshebei8, value); } }


        private bool _imageshebei9 = false;
        public bool Image9 { get => _imageshebei9; set { SetProperty(ref _imageshebei9, value); ImageShebei9 = "0"; } }

        private string imageshebei9;
        public string ImageShebei9 { get => _imageshebei9 ? imageshebei9 = "pack://application:,,,/Asset/down1.png" : imageshebei9 = "pack://application:,,,/Asset/down2.png"; set { SetProperty(ref imageshebei9, value); } }



        private float highVoltage;
        public float HighVoltage { get => highVoltage; set { SetProperty(ref highVoltage, value); } }
        private float highCurrent;
        public float HighCurrent { get => highCurrent; set { SetProperty(ref highCurrent, value); } }

        private float lowVoltage;
        public float LowVoltage { get => lowVoltage; set { SetProperty(ref lowVoltage, value); } }

        private float lowCurrent;
        public float LowCurrent { get => lowCurrent; set { SetProperty(ref lowCurrent, value); } }

        private float partialNumber;
        public float PartialNumber { get => partialNumber; set { SetProperty(ref partialNumber, value); } }

        private int isTimer = 0;
        public int IsTimer { get => isTimer; set { SetProperty(ref isTimer, value); } }


        private float plchighVoltage;
        public float PlcHighVoltage { get => plchighVoltage; set { SetProperty(ref plchighVoltage, value); } }


        //private string gaoyadianyaselect = ConfigurationManager.AppSettings["DEVICE_GAOYADIANYA"];
        //public string GaoyadianyaSelect { get => gaoyadianyaselect; set { SetProperty(ref gaoyadianyaselect, value); } }
        public string GaoyadianyaSelect { get; set; } = ConfigurationManager.AppSettings["DEVICE_GAOYADIANYA"];

        private string gaoyadianliuselect = ConfigurationManager.AppSettings["DEVICE_GAOYADIANLIU"];
        public string GaoyadianliuSelect { get => gaoyadianliuselect; set { SetProperty(ref gaoyadianliuselect, value); } }

        private string diyadianyaSelect = ConfigurationManager.AppSettings["DEVICE_DIYADIANYA"];
        public string DiyadianyaSelect { get => diyadianyaSelect; set { SetProperty(ref diyadianyaSelect, value); } }

        private string diyadianliuselect = ConfigurationManager.AppSettings["DEVICE_DIYADIANLIU"];
        public string DiyadianliuSelect { get => diyadianliuselect; set { SetProperty(ref diyadianliuselect, value); } }



        public int islevel1timeover;
        public int IsLevel1TimeOver { get => islevel1timeover; set { SetProperty(ref islevel1timeover, value); } }

        public int islevel2timeover;
        public int IsLevel2TimeOver { get => islevel2timeover; set { SetProperty(ref islevel2timeover, value); } }

        public int islevel3timeover;
        public int IsLevel3TimeOver { get => islevel3timeover; set { SetProperty(ref islevel3timeover, value); } }

    }
}
