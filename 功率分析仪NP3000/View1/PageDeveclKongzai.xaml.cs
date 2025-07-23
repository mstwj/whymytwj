using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 功率分析仪NP3000.Model;

namespace 功率分析仪NP3000.View1
{
    /// <summary>
    /// PageDeveclKongzai.xaml 的交互逻辑
    /// </summary>
    public partial class PageDeveclKongzai : Page
    {        
        public PageDeveclKongzaiModelcs pageDeveclKongzaiModelcs = null;
        public PageDeveclKongzai()
        {
            InitializeComponent();
            pageDeveclKongzaiModelcs  = new PageDeveclKongzaiModelcs(
                UserMainC.userMainControlModel, 
                UserMainL,
                UserPanel.userPanelKongzaiModel,
                UserInfo.userProInfoControlModel
                );
        }

        
    }
}
