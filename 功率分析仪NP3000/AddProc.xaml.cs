using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using 功率分析仪NP3000.Base;
using 功率分析仪NP3000.Model;
using 功率分析仪NP3000.Table;
using static 功率分析仪NP3000.Base.DbContextExtensions;


namespace 功率分析仪NP3000
{
    /// <summary>
    /// AddProc.xaml 的交互逻辑
    /// </summary>
    public partial class AddProc : Window
    {
        public table_proinfo m_Current_tableinfo = null;
        public table_proinfo? m_newproinfo { get; set; } = null;        

        public List<string> ComboBoxYanPingBianhao { get; set; } = new List<string>();

        public List<string> ComboBoxXiangShu { get; set; } = new List<string>();
        public List<string> ComboBoxLaozhu { get; set; } = new List<string>();
        public List<string> ComboBoxCaiZhi { get; set; } = new List<string>();
        public List<string> ComboBoxBiaoHao { get; set; } = new List<string>();
        public List<string> ComboBoxZhuHao { get; set; } = new List<string>();
        public AddProc()
        {
            InitializeComponent();

            this.DataContext = this;


            ComboBoxXiangShu.Add("D-单相");
            ComboBoxXiangShu.Add("S-三相");

            ComboBoxLaozhu.Add("双绕组");
            ComboBoxLaozhu.Add("S-三绕组");

            ComboBoxCaiZhi.Add("铜");
            ComboBoxCaiZhi.Add("L-铝");

            ComboBoxBiaoHao.Add("无");
            ComboBoxZhuHao.Add("Y/D");
            ComboBoxZhuHao.Add("D/Y");
            ComboBoxZhuHao.Add("Y/Y");
            ComboBoxBiaoHao.Add("D/D");
            ComboBoxBiaoHao.Add("Zn/D");
            ComboBoxBiaoHao.Add("YN/d");
            ComboBoxBiaoHao.Add("D/yn");
            ComboBoxBiaoHao.Add("YN/y");
            ComboBoxBiaoHao.Add("Y/yn");
            ComboBoxBiaoHao.Add("YN/yn");

            ComboBoxZhuHao.Add("00");
            ComboBoxZhuHao.Add("01");
            ComboBoxZhuHao.Add("02");
            ComboBoxZhuHao.Add("03");
            ComboBoxZhuHao.Add("04");
            ComboBoxZhuHao.Add("05");
            ComboBoxZhuHao.Add("06");
            ComboBoxZhuHao.Add("07");
            ComboBoxZhuHao.Add("08");
            ComboBoxZhuHao.Add("09");
            ComboBoxZhuHao.Add("10");
            ComboBoxZhuHao.Add("11");

            using (var context = new MyDbContext())
            {
                try
                {
                    // 获取单个实体（第一个匹配）                    
                    var listNames = context.newproinfo.ToList();
                    foreach (var proname in listNames)
                    {
                        //刷新..
                        ComboBoxYanPingBianhao.Add(proname.YanPingBianhao);
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    MessageBox.Show(message);
                }
            }

        }


        private void DoSetDataToUi()
        {
            MessageBox.Show("调用了");
            //设置..到UI界面..
            YanPingBianhao.Text = m_newproinfo.YanPingBianhao;
            GuigeXinhao.Text = m_newproinfo.GuigeXinhao;
            Tuhao.Text = m_newproinfo.Tuhao.ToString();
            XiangShu.Text = m_newproinfo.XiangShu;
            Laozhu.Text = m_newproinfo.Laozhu;
            GaoYaLaozhu.Text = m_newproinfo.GaoYaLaozhu.ToString();
            DiYaLaozhu.Text = m_newproinfo.DiYaLaozhu.ToString();
            DanWei.Text = m_newproinfo.DanWei.ToString();
            CaiZhi.Text = m_newproinfo.CaiZhi.ToString();
            EDingLongLiang.Text = m_newproinfo.EDingLongLiang.ToString();
            TiaoYaBili.Text = m_newproinfo.TiaoYaBili.ToString();
            GaoYaEDingDianYa.Text = m_newproinfo.GaoYaEDingDianYa.ToString();
            DiYaEDingDianYa.Text = m_newproinfo.DiYaEDingDianYa.ToString();
            BiaoHao.Text = m_newproinfo.BiaoHao.ToString();
            ZhuHao.Text = m_newproinfo.ZhuHao;
        }
        

        private bool UiToSetData()
        {
            //设置..到UI界面..
            m_newproinfo.YanPingBianhao = YanPingBianhao.Text;
            m_newproinfo.GuigeXinhao = GuigeXinhao.Text;            
            m_newproinfo.XiangShu = XiangShu.Text;
            m_newproinfo.Laozhu = Laozhu.Text;            
            m_newproinfo.CaiZhi = CaiZhi.Text;            

            double num;
            int inum;
            if (double.TryParse(GaoYaEDingDianYa.Text, out num)) m_newproinfo.GaoYaEDingDianYa = num;
            else { MessageBox.Show("GaoYaEDingDianYa Error"); return false; }

            if (double.TryParse(DiYaEDingDianYa.Text, out num)) m_newproinfo.DiYaEDingDianYa = num;
            else { MessageBox.Show("DiYaEDingDianYa Error"); return false; }

            if (double.TryParse(TiaoYaBili.Text, out num)) m_newproinfo.TiaoYaBili = num;
            else { MessageBox.Show("TiaoYaBili Error"); return false; }

            if (int.TryParse(EDingLongLiang.Text, out inum)) m_newproinfo.EDingLongLiang = inum;
            else { MessageBox.Show("EDingLongLiang Error"); return false; }

            if (int.TryParse(DanWei.Text, out inum)) m_newproinfo.DanWei = inum;
            else { MessageBox.Show("DanWei Error"); return false; }

            if (int.TryParse(DiYaLaozhu.Text, out inum)) m_newproinfo.DiYaLaozhu = inum;
            else { MessageBox.Show("DiYaLaozhu Error"); return false; }

            if (int.TryParse(GaoYaLaozhu.Text, out inum)) m_newproinfo.GaoYaLaozhu = inum;
            else { MessageBox.Show("GaoYaLaozhu Error"); return false; }

            if (int.TryParse(Tuhao.Text, out inum)) m_newproinfo.Tuhao = inum;
            else { MessageBox.Show("Tuhao Error"); return false; }

            m_newproinfo.BiaoHao = BiaoHao.Text;
            m_newproinfo.ZhuHao = ZhuHao.Text;
            return true;
        }

        private async void ClickCommand1(object sender, RoutedEventArgs e)
        {
            using (var context = new MyDbContext())
            {
                table_proinfo? firstEntity = context.newproinfo.FirstOrDefault(e => e.YanPingBianhao == YanPingBianhao.Text);

                // 更新已有数据                
                if (firstEntity != null)
                {
                    m_newproinfo = new table_proinfo();
                    //不包括ID设置...
                    if (!UiToSetData()) return;
                    m_newproinfo.Id = firstEntity.Id;
                    //注意这里不是比较，是赋值了..(有数据就是修改了.)
                    ObjectComparer.CompareObjects(firstEntity, m_newproinfo);
                    
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        // 数据添加成功 -- 这里不能试验CLONE
                        MessageBox.Show("样品选择完成-更新");
                        ObjectComparer.CompareObjects(m_Current_tableinfo, m_newproinfo);
                    }
                    else
                    {
                        MessageBox.Show("样品选择完成");
                        ObjectComparer.CompareObjects(m_Current_tableinfo, m_newproinfo);
                    }
                    return;
                }
                else
                {
                    m_newproinfo = new table_proinfo();
                    if (!UiToSetData()) return;
                    context.newproinfo.Add(m_newproinfo);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        // 数据添加成功
                        MessageBox.Show("添加成功");
                        ObjectComparer.CompareObjects(m_Current_tableinfo, m_newproinfo);
                    }
                    else
                    {
                        MessageBox.Show("添加失败");
                    }
                }
            }
        }

        private async void ClickCommand2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //int i = 19;
        }

        private void BoxDropDownClose(object sender, EventArgs e)
        {
            //这里选择后已经有值了..           
            using (var context = new MyDbContext())
            {
                var firstEntity = context.newproinfo.FirstOrDefault(e => e.YanPingBianhao == YanPingBianhao.Text);
                //m_newproinfo = m_newproinfo.Clone();
                // 更新已有数据
                //var person = context.newproinfo.Find(Data1);
                if (firstEntity != null)
                {
                    m_newproinfo = firstEntity.Clone();
                    //注意这里不是比较，是赋值了..(有数据就是修改了.)
                    DoSetDataToUi();
                }
            }


        }


    }
}
