using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 空载_负载.Base;
using 空载_负载.Model;

namespace 空载_负载
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewMdoel mdoel { get; set; } = new MainViewMdoel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = mdoel;
        }

        private void DoBtnMenu1_1Click(object sender, RoutedEventArgs e)
        {
            AddProc addProc = new AddProc();
            addProc.ShowDialog();
            if (addProc.state == true)
            {                
                m_sampleinformation.m_tableProductInfo.ProductType = addProc.ProductType;
                mdoel.m_tableProductInfo.ProductTuhao = addProc.ProductTuhao;
                mdoel.m_tableProductInfo.Highpressure = addProc.Highpressure;
                mdoel.m_tableProductInfo.Highcurrent = addProc.Highcurrent;
                mdoel.m_tableProductInfo.Lowpressure = addProc.Lowpressure;
                mdoel.m_tableProductInfo.Lowcurrent = addProc.Lowcurrent;
            }            

        }

        private void DoBtnMenu1_2Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DoBtnMenu2_1Click(object sender, RoutedEventArgs e)
        {
            if (mdoel.m_tableProductInfo.ProductNumber.Length == 0)
            {
                MessageBox.Show("请先配置样品!");
                return;
            }
            
            try
            {
                using (var context = new MyDbContext())
                {
                    var firstEntity = context.NoloadStandardInfo.FirstOrDefault(e => e.ProductType == mdoel.m_tableProductInfo.ProductType && e.ProductTuhao == mdoel.m_tableProductInfo.ProductTuhao);
                    if (firstEntity == null)
                    {
                        MessageBox.Show("配置产品,在空载标准库里,没有找到这个标准!");
                        return ;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ;
            }
            mdoel.DoBtnCommandStart("NoloadPage");
        }

        private void DoBtnMenu2_2Click(object sender, RoutedEventArgs e)
        {
            mdoel.DoBtnCommandStart("LoadPage");
        }


        private void DoBtnMenu2_3Click(object sender, RoutedEventArgs e)
        {
            mdoel.DoBtnCommandStart("Induction");
        }

        private void DoBtnMenu2_4Click(object sender, RoutedEventArgs e)
        {
            ///mdoel.DoBtnCommandStart("Induction");
        }

        private void DoBtnMenu3_1Click(object sender, RoutedEventArgs e)
        {
            TableNoloadRecordInfo tableNoloadRecordInfo = new TableNoloadRecordInfo();
            tableNoloadRecordInfo.ShowDialog();
        }

        private void DoBtnMenu3_2Click(object sender, RoutedEventArgs e)
        {

        }

        private void DoBtnMenu3_3Click(object sender, RoutedEventArgs e)
        {

        }


        private void DoBtnMenu3_4Click(object sender, RoutedEventArgs e)
        {
            TableNoloadStandardInfo tableNoloadStandardInfo = new TableNoloadStandardInfo();
            tableNoloadStandardInfo.ShowDialog();
        }

        private void DoBtnMenu3_5Click(object sender, RoutedEventArgs e)
        {

        }

        private void DoBtnMenu3_6Click(object sender, RoutedEventArgs e)
        {

        }

        private void DoBtnMenu3_7Click(object sender, RoutedEventArgs e)
        {
            TableProductInfo tableProductInfo = new TableProductInfo();
            tableProductInfo.ShowDialog();
        }


        private void DoBtnMenu4_1Click(object sender, RoutedEventArgs e)
        {
            DeveclWindow deveclWindow = new DeveclWindow();
            deveclWindow.ShowDialog();
        }


    }
}