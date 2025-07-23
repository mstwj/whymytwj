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
using 空载_负载.Model;

namespace 空载_负载.View
{
    /// <summary>
    /// NoloadPage.xaml 的交互逻辑
    /// </summary>
    public partial class NoloadPage : UserControl
    {
        public Noload model { get; set; } = new Noload();
        public NoloadPage()
        {
            InitializeComponent();
            this.DataContext = model;

            //1.设置所有模块..
            model.SetModelData(UserDevcel.model, 
                               UserPlcDevcel.Model, 
                               UserRecordControl.model,                                
                               UserInfoControl.model
                               );
            //2.初始化数据..
            //model.Initiate();
        }

       
    }
}
