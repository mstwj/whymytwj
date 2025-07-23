
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using net5_10_14.Table;

namespace net5_10_14.Base
{
    public class RotobCallBackBase
    {
        public int currCommand { get; set; } //指令..
        public RotobErrorCode currErrorcode { get; set; } //错误吗..
        public string currMessage { get; set; } //当前消息

    }
    //回调给UI..(每个机器人返回UI处理都不一样... Page1定的是变比,所以就是Page1了..)
    public class Page1_RotobCallBackArg : RotobCallBackBase
    {
        public string datatimer { get; set; } //时间.
        public int datazbfs { get; set; }  //组别方式.
        public int datadqfj { get; set; } //当前分接
        public int datajx { get; set; }   //及性 (0,1,其他)
        public float dataelbb { get; set; } //额定变比
        public float datafjjk { get; set; } //分接间距
        public float dataKAB { get; set; }  //KAB相比
        public float dataKBC { get; set; }//KBC相比
        public float dataKCA { get; set; }//KCA相比
        public float dataEAB { get; set; }//EAB误差
        public float dataEBC { get; set; }//EBC误差
        public float dataECA { get; set; } //ECA误差
        public int dataclfs { get; set; } //测量方式
    }

    public class Page2_RotobCallBackArg : RotobCallBackBase
    {
        public float EAB { get; set; }
        public float EBC { get; set; }
        public float ECA { get; set; }
    }

    public class Page3_RotobCallBackArg : RotobCallBackBase
    {
        public float []data { get; set; }
    }




    public class BaseModel : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }


    public class tempData
    {
        public string types;
        public string tuhao;
    }

    public static class Tools
    {

        public static string[] zz_report_MapArrayDataBaseName =
        {
            "Id",
            "ProName",
            "Guigexinhao",
            "Tuhao",
            "Datatimer",
            "DataAB",
            "DataBC",
            "DataCA",
            "DataABBCCAXJC",
            "DataHGPD",
            "Dataabx",
            "Databcx",
            "Datacax",
            "Dabbccaxjcx",
            "Dataanx",
            "Databnx",
            "Datacnx",
            "Dnbncnxjcx",
            "Datahgqdx"
        };

        public static string[] zz_report_MapArrayDataBaseNameChinese =
{
            "Id",
            "样品编号",
            "规格型号",
            "图号",
            "时间",            
            "高压AB",
            "高压BC",
            "高压CA",
            "高压相间差",
            "高压合格判断",
            "低压ab",
            "低压bc",
            "低压ca",
            "低压相间差",
            "低压an",
            "低压bn",
            "低压cn",
            "低压线间差",
            "低压合格判断"
        };


        public static string[] bb_report_MapArrayDataBaseName = 
        {
            "Id",
            "ProName",
            "Guigexinhao",
            "Tuhao",
            "Datatimer",
            "Datazbfs",
            "Datadqfj",
            "Datajx",
            "Dataelbb",
            "Datafjjk",
            "DataKAB",
            "DataKBC",
            "DataKCA",
            "DataEAB",
            "DataEBC",
            "DataECA",
            "Dataclfs",
            "DataHGPD"
        };

        public static string[]  bb_report_MapArrayDataBaseNameChinese = 
        {
            "Id",
            "样品编号",
            "规格型号",
            "图号",
            "时间",
            "接法",
            "组号",
            "及性",
            "额定变比",
            "分接间距",
            "KAB相比",
            "KBC相比",
            "KCA相比",
            "EAB误差",
            "EBC误差",
            "ECA误差",
            "测量方式",
            "合格"
        };


        //experimentstandard_bianbishiyan(变比试验试验标准库)
        public static string[] experimentstandard_bianbishiyan_MapArrayDataBaseName =
        {
            "Id",       
            "Guigexinhao",
            "Tuhao",
            "Bianbiwucha",
            "Createuser",
            "Ccreateuserdate",
        };
        public static string[] experimentstandard_bianbishiyan_MapArrayDataBaseNameChinese =
{
            "Id",       
            "规格型号",
            "图号",
            "变比误差",
            "标准表创建者",
            "标准表创建时间",
        };


        //experimentstandard_ziliudianzushiyan 直流电阻试验标准库
        public static string[] experimentstandard_ziliudianzushiyan_MapArrayDataBaseName =
        {
            "Id",
            "Guigexinhao",
            "Tuhao",
            "Xiandianzhupinghen",
            "Xdianzhupinghen",
            "Createuser",
            "Ccreateuserdate",
        };
        
        public static string[] experimentstandard_ziliudianzushiyan_MapArrayDataBaseNameChinese =
{
            "Id",
            "规格型号",
            "图号",
            "线电阻平衡率",
            "相电阻平衡率",
            "标准表创建者",
            "标准表创建时间",
        };

        //变比实验报告
        public static string[] bianbishiyan_report_MapArrayDataBaseName =
        {
            "Id",
            "Reportnumber",
            "Rreportdate",
            "Reportuser",
            "Reportcheckuser",
            "ReportcheckStartTime",
            "ReportcheckEndTime",
            "ReportcheckWendu",
            "ReportcheckShidu",
            "ReportcheckDaqiya",
            "ReportcheckProNumber",
            "Reportlocate", //变压器变比测试报表(bianbishiyan_report)
            "ReportHpressure",
            "ReportLpressure",
            "ReportEdingbianbi",
            "ReportABwucha",
            "ReportBCwucha",
            "ReportCAwucha",
            "RecportconnectNumber",
            "RecportTestName",
            "RecportTestSecnumber",
            "Recportzonghoujielun",
        };

        public static string[] bianbishiyan_report_MapArrayDataBaseNameChinese =
        {
            "Id",
            "报告编号",
            "报告日期",
            "实验人员",
            "报告审核人员",
            "试验开始时间",
            "试验结束时间",
            "试验环境温度",
            "试验环境湿度",
            "试验环境大气压",
            "产品序号",
            "分接位置", //变压器变比测试报表(bianbishiyan_report)
            "高压电压",
            "低压电压",
            "额定变比",
            "变比误差AB%",
            "变比误差BC%",
            "变比误差CA%",
            "联结组标号",
            "试验设备名称",
            "试验设备型号",
            "结论",
        };

        public static string[] directlease_report_MapArrayDataBaseName =
        {
            "Id",
            "Reportnumber",
            "Rreportdate",
            "Reportuser",
            "Reportcheckuser",
            "ReportcheckStartTime",
            "ReportcheckEndTime",
            "ReportcheckWendu",
            "ReportcheckShidu",
            "ReportcheckDaqiya",
            "ReportcheckProNumber",
            "Reportlocate",             
            "Reporthab",
            "Reporthbc",
            "Reporthca",
            "ReportGaoyaceXiancha",
            "ReportGaoyaceAN",
            "ReportGaoyaceBN",
            "ReportGaoyaceCN",
            "ReportGaoyaceXCha",
            "Reporthab1",
            "Reporthab2",
            "Reporthab3",
            "Reporthab4",
            "Reporthab5",
            "Reporthab6",
            "Reporthbc1",
            "Reporthbc2",
            "Reporthbc3",
            "Reporthbc4",
            "Reporthbc5",
            "Reporthbc6",
            "Reporthca1",
            "Reporthca2",
            "Reporthca3",
            "Reporthca4",
            "Reporthca5",
            "Reporthca6",
            "Reporth1DiyaXiancha",
            "Reporth2DiyaXiancha",
            "ReporthMabXiancha1",
            "ReporthMabXiancha2",
            "Reportzonghoujielun"
        };

        public static string[] directlease_report_MapArrayDataBaseNameChinese =
        {
            "Id",
            "报告编号",
            "报告日期",
            "报告审核人员",
            "报告审核人员",
            "试验开始时间",
            "试验结束时间",
            "试验环境温度",
            "试验环境湿度",
            "试验环境大气压",
            "产品序号",
            "分接位置 ",
            "AB高压测线电阻",
            "BC高压测线电阻",
            "CA高压测线电阻",
            "高压侧线间差",
            "AN高压测相电阻",
            "BN高压测相电阻",
            "CN高压测相电阻",
            "高压侧相间差",
            "ab低压测1线电阻",
            "ab低压测2线电阻",
            "ab低压测3线电阻",
            "ab低压测4线电阻",
            "ab低压测5线电阻",
            "ab低压测6线电阻",
            "bc低压测1线电阻",
            "bc低压测2线电阻",
            "bc低压测3线电阻",
            "bc低压测4线电阻",
            "bc低压测5线电阻",
            "bc低压测6线电阻",
            "ca低压测1线电阻",
            "ca低压测2线电阻",
            "ca低压测3线电阻",
            "ca低压测4线电阻",
            "ca低压测5线电阻",
            "ca低压测6线电阻",
            "低压侧1线间差",
            "低压侧2相间差",
            "ab中压测1线电阻",
            "ab中压测2线电阻",
            "结论"
        };


        //样品
        public static string[] newproinfo_MapArrayDataBaseName =
{
            "Id",
            "ProName",
            "GuigeXinhao",
            "Tuhao",
            "ProType",
            "ProTypeItem",
            "ProOhefanshi",
            "ProXiangShu",
            "ProWaijyuancaizhi",
            "ProColdFunc",
            "ProOldloop",
            "ProLaozhu", 
            "ProTiaoyafashi",
            "ProXianQCaizi",
            "ProTianxCaizi",
            "ProTeshuytu",
            "EdingRonglang",
            "Taoyabili",
            "GaoyaedingDianya",
            "GaoyaedingDianliu",
            "ZhongyaedingDianya",
            "ZhongyaedingDianliu",
            "DiyaedingDianya",
            "DiyaedingDianliu",
            "BiaoHao", 
            "ZhuHao", 
            "GaoRaozhuNumber", 
            "DiRaozhuNumber",
            "DanWeiNumber"

        };

        public static string[] newproinfo_MapArrayDataBaseNameChinese =
{
            "Id",
            "样品名称",
            "规格型号",
            "图号",
            "类型",
            "变压器类别  ",
            "绕组藕合方式",
            "相数",
            " 绕组外绝缘介质",
            "冷却方式",
            "油循环",
            "绕组数",
            "调压方式",
            "线圈导线材质",
            "铁心材质",
            "特殊用途或特殊结构",
            "额定容量",
            "调压比例",
            "高压额定电压",
            "高压额定电流",
            "中压额定电压",
            "中压额定电流",
            "低压额定电压",
            "低压额定电压",
            "标号",
            "组号",
            "高压侧绕组数",
            "底压侧绕组数",
            "档位数量"

        };


        //以下代码已经不是比较 ，是赋值了...
        public static class ObjectComparer
        {
            public static void CompareObjects(object obj1, object obj2)
            {
                if (obj1 == null || obj2 == null)
                    throw new ArgumentException("Objects cannot be null");

                if (!obj1.GetType().Equals(obj2.GetType()))
                    throw new ArgumentException("Objects should be of the same type");


                PropertyInfo[] properties = obj1.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    var value1 = property.GetValue(obj1);
                    var value2 = property.GetValue(obj2);

                    if (!Equals(value1, value2))
                    {
                        PropertyInfo sourceProperty = obj2.GetType().GetProperty(property.Name);
                        property.SetValue(obj1, sourceProperty.GetValue(obj2));
                    }
                }
            }
        }

        //得到类的字段类型，通过类型来验证..
        public static (bool, string) SafeScanPro(string tablename,object Example)
        {
            PropertyInfo[] properties = Example.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string fildname = Tools.FindDaseNameToChinese(tablename,property.Name);
                var value = property.GetValue(Example);
                Type propertyType = property.PropertyType;

                if (value == null) return (false, $"{fildname}错误,不能为空值");
                switch (propertyType.Name)
                {
                    case "Int32":
                        {
                            int result;
                            if (!int.TryParse(value.ToString(), out result)) return (false, $"{fildname}:输入错误,int转换失败!");
                        }
                        break;
                    case "String":
                        {
                            bool isValid = Regex.IsMatch(value.ToString(), @"^.{1,30}$");
                            if (!isValid) return (false, $"{fildname}:输入错误,不符合标准字符最小1位最多30位");
                        }
                        break;
                    case "Float":
                        {
                            float result;
                            if (!float.TryParse(value.ToString(), out result)) return (false, $"{fildname}:输入错误,浮点float转换失败!");
                            //下面代码保留3位..
                            value = (float)Math.Round((float)value, 3); 
                        }
                        break;
                    case "Double":
                        {
                            double result;
                            if (!double.TryParse(value.ToString(), out result)) return (false, $"{fildname}:输入错误,浮点double转换失败!");
                            //下面代码保留3位..
                            value = (double)Math.Round((double)value, 3);
                        }
                        break;                
                    case "DateTime":
                        {
                            bool isValid = Regex.IsMatch(value.ToString(), @"^.{10,30}$");
                            if (!isValid) return (false, $"{fildname}:输入错误,不符合标准");
                        }
                        break;
                }
            }
            return (true, "所有验证成功");
        }

        public static string FindDaseNameToChinese(string tablename,  string dbfieldname)
        {

            int index = 0;

            
            if (tablename == "zz_report")
            {
                index = Array.FindIndex(zz_report_MapArrayDataBaseName, item => item == dbfieldname);
                if (index != -1)
                    return zz_report_MapArrayDataBaseNameChinese[index];
                else
                    throw new Exception("严重错误,字段没有找到");
            }


            if (tablename == "bb_report")
            {
                index = Array.FindIndex(bb_report_MapArrayDataBaseName, item => item == dbfieldname);
                if (index != -1)
                    return bb_report_MapArrayDataBaseNameChinese[index];
                else
                    throw new Exception("严重错误,字段没有找到");
            }

            

            if (tablename == "newproinfo")
            {
                index = Array.FindIndex(newproinfo_MapArrayDataBaseName, item => item == dbfieldname);
                if (index != -1)
                    return newproinfo_MapArrayDataBaseNameChinese[index];
                else
                    throw new Exception("严重错误,字段没有找到");
            }


            if (tablename == "experimentstandard_ziliudianzushiyan")
            {
                index = Array.FindIndex(experimentstandard_ziliudianzushiyan_MapArrayDataBaseName, item => item == dbfieldname);
                if (index != -1)
                    return experimentstandard_ziliudianzushiyan_MapArrayDataBaseNameChinese[index];
                else
                    throw new Exception("严重错误,字段没有找到");

            }            

            if (tablename == "directlease_report")
            {
                index = Array.FindIndex(directlease_report_MapArrayDataBaseName, item => item == dbfieldname);
                if (index != -1)
                    return directlease_report_MapArrayDataBaseNameChinese[index];
                else
                    throw new Exception("严重错误,字段没有找到");

            }

            if (tablename == "bianbishiyan_report")
            {
                index = Array.FindIndex(bianbishiyan_report_MapArrayDataBaseName, item => item == dbfieldname);
                if (index != -1)
                    return bianbishiyan_report_MapArrayDataBaseNameChinese[index];
                else
                    throw new Exception("严重错误,字段没有找到");
            }

            if (tablename == "experimentstandard_bianbishiyan")
            {
                index = Array.FindIndex(experimentstandard_bianbishiyan_MapArrayDataBaseName, item => item == dbfieldname);
                if (index != -1) return experimentstandard_bianbishiyan_MapArrayDataBaseNameChinese[index];
                else throw new Exception("严重错误,字段没有找到");

            }
            return "null";
        }

        public static string FindDaseNameToEnglish(string tablename, string dbfieldname)
        {
            int index = 0;

            return "null";
        }



    }
}
