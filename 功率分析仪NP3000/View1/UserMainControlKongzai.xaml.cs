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
    /// UserMainControlKongzai.xaml 的交互逻辑
    /// </summary>
    public partial class UserMainControlKongzai : UserControl
    {
        public UserMainControlModel userMainControlModel { get; set; } = new UserMainControlModel();
        public UserMainControlKongzai()
        {
            InitializeComponent();

            comboBox1.SelectedItem = comboBox1.Items[0]; // 选择第二个项（索引从0开始
            comboBox2.SelectedItem = comboBox2.Items[0]; // 选择第二个项（索引从0开始
            comboBox3.SelectedItem = comboBox3.Items[0]; // 选择第二个项（索引从0开始

            comboBox1.SelectionChanged += (s, e) =>
            {
                var selectedItem = comboBox1.SelectedItem;
                // 执行你需要的操作
                userMainControlModel.comboBox1str = selectedItem.ToString();
            };

            comboBox2.SelectionChanged += (s, e) =>
            {
                var selectedItem = comboBox2.SelectedItem;
                // 执行你需要的操作
                userMainControlModel.comboBox2str = selectedItem.ToString();
            };

            comboBox3.SelectionChanged += (s, e) =>
            {
                var selectedItem = comboBox3.SelectedItem;
                // 执行你需要的操作
                userMainControlModel.comboBox3str = selectedItem.ToString();
            };

            this.DataContext = userMainControlModel;
        }

        
    }
}
