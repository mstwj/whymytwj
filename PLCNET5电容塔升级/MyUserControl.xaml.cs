using PLCNET5电容塔.Data;
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

namespace PLCNET5电容塔
{
    /// <summary>
    /// MyUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class MyUserControl : UserControl
    {
        public UserModel m_userMode { get; set; } = new UserModel();

        

        public MyUserControl()
        {            
            InitializeComponent();
            this.DataContext = m_userMode;
        }

        public void SetTextData(string data)
        {
            q1_1.Text = "AQ" + data + "-1";
            q1_2.Text = "AQ" + data + "-2"; ;
            q1_3.Text = "AQ" + data + "-3"; ;
            q1_4.Text = "AQ" + data + "-4"; ;
            q1_5.Text = "AQ" + data + "-5"; ;
            q1_6.Text = "AQ" + data + "-6"; ;

            q2_1.Text = "BQ" + data + "-1"; ;
            q2_2.Text = "BQ" + data + "-2"; ;
            q2_3.Text = "BQ" + data + "-3"; ;
            q2_4.Text = "BQ" + data + "-4"; ;
            q2_5.Text = "BQ" + data + "-5"; ;
            q2_6.Text = "BQ" + data + "-6"; ;

            q3_1.Text = "CQ" + data + "-1"; ;
            q3_2.Text = "CQ" + data + "-2"; ;
            q3_3.Text = "CQ" + data + "-3"; ;
            q3_4.Text = "CQ" + data + "-4"; ;
            q3_5.Text = "CQ" + data + "-5"; ;
            q3_6.Text = "CQ" + data + "-6"; ;

            return;
        }


    }
}
