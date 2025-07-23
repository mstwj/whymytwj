using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 功率分析仪NP3000.Model
{
    public class UserProInfoControlModel : ObservableObject
    {

        private string _yanPingBianhao;//产品ID..
        private string _guigeXinhao;
        private int _tuhao;
        private string _xiangShu;
        private string _laozhu;
        private int _gaoYaLaozhu;
        private int _diYaLaozhu;
        private int _danWei;
        private string _caiZhi;
        private int _eDingLongLiang;
        private double _tiaoYaBili;
        private double _gaoYaEDingDianYa;
        private double _diYaEDingDianYa;
        private string _biaoHao;
        private string _zhuHao;


        
        public string YanPingBianhao { get => _yanPingBianhao; set { SetProperty(ref _yanPingBianhao, value); } } 
        public string GuigeXinhao { get => _guigeXinhao; set { SetProperty(ref _guigeXinhao, value); } }
        public int Tuhao { get => _tuhao; set { SetProperty(ref _tuhao, value); } }
        public string XiangShu { get => _xiangShu; set { SetProperty(ref _xiangShu, value); } }
        public string Laozhu { get => _laozhu; set { SetProperty(ref _laozhu, value); } }
        public int GaoYaLaozhu { get => _gaoYaLaozhu; set { SetProperty(ref _gaoYaLaozhu, value); } }
        public int DiYaLaozhu { get => _diYaLaozhu; set { SetProperty(ref _diYaLaozhu, value); } }
        public int DanWei { get => _danWei; set { SetProperty(ref _danWei, value); } }
        public string CaiZhi { get => _caiZhi; set { SetProperty(ref _caiZhi, value); } }
        public int EDingLongLiang { get => _eDingLongLiang; set { SetProperty(ref _eDingLongLiang, value); } }
        public double TiaoYaBili { get => _tiaoYaBili; set { SetProperty(ref _tiaoYaBili, value); } }
        public double GaoYaEDingDianYa { get => _gaoYaEDingDianYa; set { SetProperty(ref _gaoYaEDingDianYa, value); } }
        public double DiYaEDingDianYa { get => _diYaEDingDianYa; set { SetProperty(ref _diYaEDingDianYa, value); } }
        public string BiaoHao { get => _biaoHao; set { SetProperty(ref _biaoHao, value); } }
        public string ZhuHao { get => _zhuHao; set { SetProperty(ref _zhuHao, value); } }

    }
}
