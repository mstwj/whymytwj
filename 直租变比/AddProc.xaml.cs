using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using 直租变比.Model;


namespace 直租变比
{
    /// <summary>
    /// AddProc.xaml 的交互逻辑
    /// </summary>
    public partial class AddProc : Window
    {
        /*
        newproinfotable? m_newproinfo = null;//= new newproinfotable();
        */
        //public AddModeProc m_AddModeProc { get; set; } = new AddModeProc();
        public AddProc()
        {
            InitializeComponent();

            this.DataContext = this;

            /*

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
                        ComboBoxYanPingBianhao.Add(proname.YanPingBianhao);
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    MessageBox.Show(message);
                }
            }
            */

        }


        private void DoSetDataToUi()
        {
            //设置..(刷不上去..)
            /*
            m_AddModeProc.YanPingBianhao = m_newproinfo.YanPingBianhao;
            m_AddModeProc.GuigeXinhao = m_newproinfo.GuigeXinhao;
            m_AddModeProc.Tuhao = m_newproinfo.Tuhao;
            m_AddModeProc.XiangShu = m_newproinfo.XiangShu;
            m_AddModeProc.Laozhu = m_newproinfo.Laozhu;
            m_AddModeProc.GaoYaLaozhu = m_newproinfo.GaoYaLaozhu;
            m_AddModeProc.DiYaLaozhu = m_newproinfo.DiYaLaozhu;
            m_AddModeProc.DanWei = m_newproinfo.DanWei;
            m_AddModeProc.CaiZhi = m_newproinfo.CaiZhi;
            m_AddModeProc.EDingLongLiang = m_newproinfo.EDingLongLiang;
            m_AddModeProc.TiaoYaBili = m_newproinfo.TiaoYaBili;
            m_AddModeProc.GaoYaEDingDianYa = m_newproinfo.GaoYaEDingDianYa;
            m_AddModeProc.DiYaEDingDianYa = m_newproinfo.DiYaEDingDianYa;
            m_AddModeProc.BiaoHao = m_newproinfo.BiaoHao;
            m_AddModeProc.ZhuHao = m_newproinfo.ZhuHao;
            */
        }

        private List<TextBox> TextBoxList = new List<TextBox>();
        private bool SacnError()
        {
            foreach (var item in TextBoxList)
            {
                if (Validation.GetHasError(item))
                {
                    MessageBox.Show("无法提交,输入有误");
                    return false;
                }
            }
            return true;
        }

        private bool UiToSetData()
        {
            /*
            if (!SacnError()) { MessageBox.Show("无法提交,数据有错误!"); return false; }
            //设置..
            if (m_AddModeProc.YanPingBianhao == null) { MessageBox.Show("无法提交,样品编号没有填写!"); return false; }
            m_newproinfo.YanPingBianhao = m_AddModeProc.YanPingBianhao.Trim();
            if (m_newproinfo.YanPingBianhao.Length <= 3) { MessageBox.Show("无法提交,样品编号有错误!"); return false; }

            if (m_AddModeProc.GuigeXinhao == null) { MessageBox.Show("无法提交,规格型号没有填写!"); return false; }
            m_newproinfo.GuigeXinhao = m_AddModeProc.GuigeXinhao.Trim();
            if (m_newproinfo.GuigeXinhao.Length <= 3) { MessageBox.Show("无法提交,规格型号错误!"); return false; }

            m_newproinfo.Tuhao = m_AddModeProc.Tuhao;

            m_newproinfo.XiangShu = m_AddModeProc.XiangShu;
            if (m_newproinfo.XiangShu == null) { MessageBox.Show("无法提交,请选择相数!"); return false; }

            m_newproinfo.Laozhu = m_AddModeProc.Laozhu;
            if (m_newproinfo.Laozhu == null) { MessageBox.Show("无法提交,请选择挠组!"); return false; }

            m_newproinfo.GaoYaLaozhu = m_AddModeProc.GaoYaLaozhu;
            m_newproinfo.DiYaLaozhu = m_AddModeProc.DiYaLaozhu;
            m_newproinfo.DanWei = m_AddModeProc.DanWei;

            m_newproinfo.CaiZhi = m_AddModeProc.CaiZhi;
            if (m_newproinfo.CaiZhi == null) { MessageBox.Show("无法提交,请选择材质!"); return false; }

            m_newproinfo.EDingLongLiang = m_AddModeProc.EDingLongLiang;
            m_newproinfo.TiaoYaBili = m_AddModeProc.TiaoYaBili;
            m_newproinfo.GaoYaEDingDianYa = m_AddModeProc.GaoYaEDingDianYa;
            m_newproinfo.DiYaEDingDianYa = m_AddModeProc.DiYaEDingDianYa;

            m_newproinfo.BiaoHao = m_AddModeProc.BiaoHao;
            if (m_newproinfo.BiaoHao == null) { MessageBox.Show("无法提交,请选择标组!"); return false; }

            m_newproinfo.ZhuHao = m_AddModeProc.ZhuHao;
            if (m_newproinfo.ZhuHao == null) { MessageBox.Show("无法提交,请选择组号!"); return false; }
            */
            return true;
        }

        private async void ClickCommand1(object sender, RoutedEventArgs e)
        {
            /*
            using (var context = new MyDbContext())
            {
                newproinfotable? firstEntity = context.newproinfo.FirstOrDefault(e => e.YanPingBianhao == m_newproinfo.YanPingBianhao);

                // 更新已有数据
                //var person = context.newproinfo.Find(Data1);
                if (firstEntity != null)
                {
                    m_newproinfo = new newproinfotable();
                    //不包括ID设置...
                    if (!UiToSetData()) return;
                    m_newproinfo.Id = firstEntity.Id;
                    //注意这里不是比较，是赋值了..(有数据就是修改了.)
                    ObjectComparer.CompareObjects(firstEntity, m_newproinfo);

                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        // 数据添加成功
                        MessageBox.Show("更新成功");
                    }
                    else
                    {
                        MessageBox.Show("样品更新失败");
                    }
                    return;
                }
                else
                {
                    m_newproinfo = new newproinfotable();
                    if (!UiToSetData()) return;
                    context.newproinfo.Add(m_newproinfo);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        // 数据添加成功
                        MessageBox.Show("添加成功");
                    }
                    else
                    {
                        MessageBox.Show("添加失败");
                    }
                }
            }
            */
        }

        private async void ClickCommand2(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //这里选择后已经有值了..
            /*
            using (var context = new MyDbContext())
            {
                var firstEntity = context.newproinfo.FirstOrDefault(e => e.YanPingBianhao == m_AddModeProc.YanPingBianhao);
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
            */

            //int i = 19;
        }
    }
}
