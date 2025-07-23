using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using net5_10_14.Base;
using net5_10_14.Table;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace net5_10_14.ViewModel
{
    public class YanPingViewModel : INotifyPropertyChanged
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public newproinfo m_newproinfo { get; set; } = new newproinfo();

        public bool Inspection { get; set; } = false;
        
      
        private ObservableCollection<string> items;
        public ObservableCollection<string> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(nameof(Items)); }
        }

        private string selectedItem;
        public string SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); }
        }


        private string _data1;
        public string Data1 { get { return _data1; } set { _data1 = value; OnPropertyChanged(nameof(Data1)); } }

        private string _data2;
        public string Data2 { get { return _data2; } set { _data2 = value; OnPropertyChanged(nameof(Data2)); } }

        private int _data3;
        public int Data3 { get { return _data3; } set { _data3 = value; OnPropertyChanged(nameof(Data3)); } }


        private int _data4;
        public int Data4 { get { return _data4; } set { _data4 = value; OnPropertyChanged(nameof(Data4)); } }

        private double _data5;
        public double Data5 { get { return _data5; } set { _data5 = value; OnPropertyChanged(nameof(Data5)); } }


        private double _data6;
        public double Data6 { get { return _data6; } set { _data6 = value; OnPropertyChanged(nameof(Data6)); } }

        private double _data7;
        public double Data7 { get { return _data7; } set { _data7 = value; OnPropertyChanged(nameof(Data7)); } }

        private double _data8;
        public double Data8 { get { return _data8; } set { _data8 = value; OnPropertyChanged(nameof(Data8)); } }

        private double _data9;
        public double Data9 { get { return _data9; } set { _data9 = value; OnPropertyChanged(nameof(Data9)); } }


        private double _data10;
        public double Data10 { get { return _data10; } set { _data10 = value; OnPropertyChanged(nameof(Data10)); } }


        private double _data11;
        public double Data11 { get { return _data11; } set { _data11 = value; OnPropertyChanged(nameof(Data11)); } }

        private int _data12;
        public int Data12 { get { return _data12; } set { _data12 = value; OnPropertyChanged(nameof(Data12)); } }

        private int _data13;
        public int Data13 { get { return _data13; } set { _data13 = value; OnPropertyChanged(nameof(Data13)); } }

        private int _data14;
        public int Data14 { get { return _data14; } set { _data14 = value; OnPropertyChanged(nameof(Data14)); } }


        public List<string> ComboBoxItems1 { get; set; } = new List<string>();
        public List<string> ComboBoxItems2 { get; set; } = new List<string>();
        public List<string> ComboBoxItems3 { get; set; } = new List<string>();
        public List<string> ComboBoxItems4 { get; set; } = new List<string>();
        public List<string> ComboBoxItems5 { get; set; } = new List<string>();

        private string _selectBoxItems1, _selectBoxItems2, _selectBoxItems3, _selectBoxItems4,_selectBoxItems5;
        public string SelectBoxItems1 { get { return _selectBoxItems1; } set {  _selectBoxItems1 = value; OnPropertyChanged(nameof(SelectBoxItems1));}}
        public string SelectBoxItems2 { get { return _selectBoxItems2; } set { _selectBoxItems2 = value; OnPropertyChanged(nameof(SelectBoxItems2)); } }
        public string SelectBoxItems3 { get { return _selectBoxItems3; } set { _selectBoxItems3 = value; OnPropertyChanged(nameof(SelectBoxItems3)); } }
        public string SelectBoxItems4 { get { return _selectBoxItems4; } set { _selectBoxItems4 = value; OnPropertyChanged(nameof(SelectBoxItems4)); } }
        public string SelectBoxItems5 { get { return _selectBoxItems5; } set { _selectBoxItems5 = value; OnPropertyChanged(nameof(SelectBoxItems5)); } }

        public ICommand btnCommand1 { get; set; } //
        public ICommand btnCommand2 { get; set; } //
        public IEnumerable<object> TextBoxList { get; private set; }

        //(double) Math.Round(m_bianbishiyan_report.ReportHpressure, 3); 这样数据库就OK了吗？

        public YanPingViewModel()
        {
            ComboBoxItems1.Add("D-单相");
            ComboBoxItems1.Add("S-三相");

            ComboBoxItems2.Add("双绕组");
            ComboBoxItems2.Add("S-三绕组");

            ComboBoxItems3.Add("铜");            
            ComboBoxItems3.Add("L-铝");
            
            ComboBoxItems4.Add("无");
            ComboBoxItems4.Add("Y/D");
            ComboBoxItems4.Add("D/Y");
            ComboBoxItems4.Add("Y/Y");
            ComboBoxItems4.Add("D/D");
            ComboBoxItems4.Add("Zn/D");
            ComboBoxItems4.Add("YN/d");
            ComboBoxItems4.Add("D/yn");
            ComboBoxItems4.Add("YN/y");
            ComboBoxItems4.Add("Y/yn");
            ComboBoxItems4.Add("YN/yn");

            ComboBoxItems5.Add("00");
            ComboBoxItems5.Add("01");
            ComboBoxItems5.Add("02");
            ComboBoxItems5.Add("03");
            ComboBoxItems5.Add("04");
            ComboBoxItems5.Add("05");
            ComboBoxItems5.Add("06");
            ComboBoxItems5.Add("07");
            ComboBoxItems5.Add("08");
            ComboBoxItems5.Add("09");
            ComboBoxItems5.Add("10");
            ComboBoxItems5.Add("11");

            btnCommand1 = new RelayCommand<object>(DobtnCommand1);
            btnCommand2 = new RelayCommand<object>(DobtnCommand2);

            Items = new ObservableCollection<string>();


            //这里我已经肯定，全局变了都OK了...
            using (var context = new MyDbContext())
            {
                try
                {
                    // 获取单个实体（第一个匹配）                    
                    var listNames = context.newproinfo.ToList();
                    foreach (var proname in listNames)
                    {
                        Items.Add(proname.ProName);
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    logger.Error(message);
                }
            }

     

        }

        public void SetViewData(string pronmane)
        {
            using (var context = new MyDbContext())
            {
                var firstEntity = context.newproinfo.FirstOrDefault(e => e.ProName == pronmane);
                if (firstEntity != null)
                {
                    //注意这里不是比较，是赋值了..(m_newproinfo 设置数据给newproinfo)
                    Tools.ObjectComparer.CompareObjects(m_newproinfo, firstEntity);
                    DoSetDataToDataBase(false);
                }
            }
        }

        private void DoSetDataToDataBase(bool Isto)
        {
            if (Isto == true)
            {
                m_newproinfo.ProName = Data1; //变压器名  string
                m_newproinfo.GuigeXinhao = Data2; // 规格型号string
                m_newproinfo.Tuhao = Data3; // 图号int
                m_newproinfo.ProType = "NULL";//变压器类型string
                m_newproinfo.ProTypeItem = "NULL"; // 变压器类别string
                m_newproinfo.ProOhefanshi = "NULL";// 变压器绕组藕合方式string 
                m_newproinfo.ProXiangShu = SelectBoxItems1;//相数 string
                m_newproinfo.ProWaijyuancaizhi = "NULL"; //-- 绕组外绝缘介质string
                m_newproinfo.ProColdFunc = "NULL";//-- 冷却方式string
                m_newproinfo.ProOldloop = "NULL";//-- 油循环string
                m_newproinfo.ProLaozhu = SelectBoxItems2;//-- 绕组数string
                m_newproinfo.ProTiaoyafashi = "NULL";// -- 调压方式string
                m_newproinfo.ProXianQCaizi = SelectBoxItems3; //-- 线圈导线材质，string
                m_newproinfo.ProTianxCaizi = "NULL"; ;//-- 铁心材质string
                m_newproinfo.ProTeshuytu = "NULL"; //-- 特殊用途或特殊结构string 
                m_newproinfo.EdingRonglang = Data4; //-- 额定容量 INTint
                m_newproinfo.Taoyabili = Data5;//-- 调压比例 FLOATdouble
                m_newproinfo.GaoyaedingDianya = Data6;// -- 高压额定电压FLOATdouble
                m_newproinfo.GaoyaedingDianliu = 0; //-- 高压额定电流FLOAT
                m_newproinfo.ZhongyaedingDianya = 0; //-- 中压额定电压FLOAT
                m_newproinfo.ZhongyaedingDianliu = 0; //-- 中压额定电流FLOAT
                m_newproinfo.DiyaedingDianya = Data10;//-- 低压额定电压FLOAT
                m_newproinfo.DiyaedingDianliu = 0; //-- 低压额定电六FLOAT

                m_newproinfo.BiaoHao = SelectBoxItems4;  //--标号...N/N
                m_newproinfo.ZhuHao = SelectBoxItems5; //--组号...0 --- 11
                m_newproinfo.GaoRaozhuNumber = Data12; //--高绕组数量
                m_newproinfo.DiRaozhuNumber = Data13;//--地绕组数量        
                m_newproinfo.DanWeiNumber = Data14; //--挡位数量.

            }
            else
            {
                SelectBoxItems1 = m_newproinfo.ProXiangShu; //相数 string
                SelectBoxItems2 = m_newproinfo.ProLaozhu;//-- 绕组数string
                SelectBoxItems3 = m_newproinfo.ProXianQCaizi; //-- 线圈导线材质
                SelectBoxItems4 = m_newproinfo.BiaoHao;  //--标号...N/N
                SelectBoxItems5 = m_newproinfo.ZhuHao; //--组号...0 --- 11

                Data2 = m_newproinfo.GuigeXinhao;  // 规格型号string
                Data3 = m_newproinfo.Tuhao;// 图号int                
                Data4 = m_newproinfo.EdingRonglang; //-- 额定容量 INTint
                Data5 = m_newproinfo.Taoyabili; //-- 调压比例 FLOATdouble
                Data6 = m_newproinfo.GaoyaedingDianya; // -- 高压额定电压FLOATdouble
                Data10 = m_newproinfo.DiyaedingDianya; //-- 低压额定电压FLOAT

                Data12 = m_newproinfo.GaoRaozhuNumber; //--高绕组数量
                Data13 = m_newproinfo.DiRaozhuNumber;//--地绕组数量        
                Data14 = m_newproinfo.DanWeiNumber; //--挡位数量.
            }
        }

        private void DobtnCommand1(object button)
        {
            //发布..
            using (var context = new MyDbContext())
            {                                
                // 发送主题更改消息
                Inspection = false;
                WeakReferenceMessenger.Default.Send("Inspect", "Inspect");

                if (Inspection == false)
                {
                    MessageBox.Show("请注意输入有错!");
                    return;
                }

                //设置数据.(找不到就是添加.)                
                DoSetDataToDataBase(true);

                //设置数据.(找到了，就是修改.)
                var myitem = Tools.SafeScanPro("newproinfo", m_newproinfo);
                if (myitem.Item1 == false)
                {
                    MessageBox.Show(myitem.Item2);
                    return;
                }

                //都OK，记录产品名称.
                App_Config.currendProName = m_newproinfo.ProName;

                try
                {
                    //不能使用下代码WHERE以为不是唯一.是个表..
                    //var newproinfo = context.newproinfo.Where(p => p.ProName == m_newproinfo.ProName);
                    //下面是2个条件的查询..
                    //var firstEntity = context.newproinfo.FirstOrDefault(e => e.Guigexinhao == Tools.currentproductinformationtable.GuigeXinhao && e.Tuhao == Tools.currentproductinformationtable.Tuhao);
                    var firstEntity = context.newproinfo.FirstOrDefault(e => e.ProName == m_newproinfo.ProName);
                    if(firstEntity != null)
                    {

                        //注意这里不是比较，是赋值了..(m_newproinfo 设置数据给newproinfo)
                        Tools.ObjectComparer.CompareObjects(firstEntity, m_newproinfo);
                        int erowsAffected = context.SaveChanges();
                        if (erowsAffected > 0)
                        {
                            //数据被修改
                            MessageBox.Show("配置完成");                            
                        }
                        else
                        {
                            //数据没有1个被修改
                            MessageBox.Show("配置完成");
                        }                        
                        return;
                    }
                    m_newproinfo.Id = 0;
                    context.newproinfo.Add(m_newproinfo);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {                        
                        MessageBox.Show("配置完成");
                    }
                    else
                    {
                        MessageBox.Show("配置失败");
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    logger.Error(message);
                    MessageBox.Show("严重错误数据库报错,请检查数据库是否正常，请查看日志..");
                    //NLog.config -- 这个文件要复制到最后的文件夹，就OK了..就可以输出目录了..
                }
            }
            return;
        }

        private void DobtnCommand2(object button)
        {
            //发布..
            WeakReferenceMessenger.Default.Send<string, string>("Close", "Close");
            return;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
