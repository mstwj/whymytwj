using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Office.Interop.Excel;

using 铁芯实验.Model;
using 铁芯实验.Table;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace 铁芯实验.Base
{
    public static class MyTools
    {
        static public object? m_MainViewModel { get; set; }

        static public void SetMainViewModel(object mainViewModel) => m_MainViewModel = mainViewModel;
        static public MainViewModel GetMainViewModel() => (MainViewModel)m_MainViewModel;
        public static Queue<CommandItem> GetQueue() => ((MainViewModel)m_MainViewModel).queue;
        public static UserMainModel GetUserMainModel() => ((MainViewModel)m_MainViewModel).m_userMainModel;

        public static UserRecordModel GetUserRecordModel() => ((MainViewModel)m_MainViewModel).m_userRecordModel;

        public static UserInfoModel GetUserInfoModel() => ((MainViewModel)m_MainViewModel).m_userInfoModel;

        public static UserStandardModel GetUserStandardModel() => ((MainViewModel)m_MainViewModel).m_userStandardModel;
        public static PowerStateModel GetPowerStateModel() => ((MainViewModel)m_MainViewModel).m_powerStateModel;

        public static UserPowerSetModel GetUserPowerSetModel() => ((MainViewModel)m_MainViewModel).m_userPowerSetModel;

        public static UserTestModel GetUserTestModel() => ((MainViewModel)m_MainViewModel).m_userTestModel;

        public static UserCommunicationModel GetUserCommunicationModel() => ((MainViewModel)m_MainViewModel).m_userCommunicationModel;


        public static float[] ushortToFloat(ushort[] ushortValue)
        {
            float[] floats = new float[ushortValue.Length / 2];
            float floatValue1;
            byte[] _cbuffer = new byte[4];
            int k = 0;
            for (int i = 0; i < 9; i += 2, k++)
            {
                //Buffer.BlockCopy(ushortValue, i, _cbuffer, 0, 4);
                _cbuffer[0] = (byte)(ushortValue[i] >> 8); // 高位字节
                _cbuffer[1] = (byte)(ushortValue[i] & 0xFF); // 低位字节
                _cbuffer[2] = (byte)(ushortValue[i + 1] >> 8); // 高位字节
                _cbuffer[3] = (byte)(ushortValue[i + 1] & 0xFF); // 高位字节
                Array.Reverse(_cbuffer);
                floatValue1 = BitConverter.ToSingle(_cbuffer, 0);
                floats[k] = floatValue1;
            }
            return floats;
        }
        public static string RemoveLastPart(string input, char delimiter)
        {
            int lastIndex = input.LastIndexOf(delimiter);
            if (lastIndex == -1)
                return input; // 如果没有找到分隔符，则直接返回原字符串
            return input.Substring(0, lastIndex);
        }

        //设置当前产品信息
        public static bool GetCurrentUserInfoAndStandard(UserInfoModel m_userInfoModel, UserStandardModel m_userStandardModel)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var firstEntity = context.ProductInfo.FirstOrDefault(e => e.ProductNumber == MainWindow.Current_ProductNumber);
                    if (firstEntity == null) return false;

                    m_userInfoModel.ProductNumber = firstEntity.ProductNumber;
                    m_userInfoModel.ProductType = firstEntity.ProductType;
                    m_userInfoModel.ProductTuhao = firstEntity.ProductTuhao;
                    m_userInfoModel.ProductCapacity = firstEntity.ProductCapacity;
                    m_userInfoModel.RatedVoltage = firstEntity.RatedVoltage;
                    m_userInfoModel.PhaseNumber = firstEntity.PhaseNumber;


                    var firstEntity2 = context.StandardInfo.FirstOrDefault(e => e.ProductType == MainWindow.Current_StandardType && e.ProductTuhao == MainWindow.Current_StandardTuhao);
                    if (firstEntity2 == null) return false;

                    m_userStandardModel.ProductType = firstEntity2.ProductType;
                    m_userStandardModel.ProductTuhao = firstEntity2.ProductTuhao;
                    m_userStandardModel.ProductStandard = firstEntity2.ProductStandard;
                    m_userStandardModel.ProductStandardUpperimit = firstEntity2.ProductStandardUpperimit;
                    m_userStandardModel.ProductStandardDownimit = firstEntity2.ProductStandardDownimit;
                    m_userStandardModel.ProductCurrentStandard = firstEntity2.ProductCurrentStandard;
                    m_userStandardModel.ProductCurrentStandardUpperrimit = firstEntity2.ProductCurrentStandardUpperrimit;
                    m_userStandardModel.ProductCurrentStandardDownimit = firstEntity2.ProductCurrentStandardDownimit;

                    return true;
                }
            }
            catch (Exception ex) { return false; }
        }

        //保存数据到数据库.
        public static bool SaveDataToDataBase(UserRecordModel userRecordModel, UserInfoModel userInfoModel)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    Table_RocreadInfo table_RocreadInfo = new Table_RocreadInfo();

                    table_RocreadInfo.Id = 0; //(ID 产品表)
                    table_RocreadInfo.ReportcheckStartTime = DateTime.Now;

                    table_RocreadInfo.ProductNumber = userInfoModel.ProductNumber;
                    table_RocreadInfo.ProductType = userInfoModel.ProductType;
                    table_RocreadInfo.ProductTuhao = userInfoModel.ProductTuhao;
                    table_RocreadInfo.ProductCapacity = userInfoModel.ProductCapacity;
                    table_RocreadInfo.RatedVoltage = userInfoModel.RatedVoltage;
                    table_RocreadInfo.PhaseNumber = userInfoModel.PhaseNumber;

                    //三相电压平均值 ToString("0.0");
                    table_RocreadInfo.ATPVoltage = userRecordModel.ATPVoltage.ToString("0.0"); //保留1位
                    //三相电压有效值
                    table_RocreadInfo.RMSvalue = userRecordModel.RMSvalue.ToString("0.0");//保留1位
                    //三相电流有效值
                    table_RocreadInfo.IRMSvalue = userRecordModel.IRMSvalue.ToString("0.0");//保留1位
                    //空载电流
                    table_RocreadInfo.NoloadCurrent = userRecordModel.NoloadCurrent2.ToString("0.00");//保留2位
                    //空载电流%
                    table_RocreadInfo.PercentageNoloadCurrent = userRecordModel.PercentageNoloadCurrent.ToString();
                    //空载损耗
                    table_RocreadInfo.NoloadLoss = userRecordModel.NoloadLoss.ToString("0.0");//保留1位
                    //合格判断
                    table_RocreadInfo.QualifiedJudgment = userRecordModel.QualifiedJudgment.ToString();

                    context.RocreadInfo.Add(table_RocreadInfo);

                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //计算三相施加电压 额定电压..参数2 额定扎数 参数3 临时扎数(三相)
        public static float GetCalcAppliedVoltageThree(string RatedVoltage, float Edingzhashu_data, float Tempzhashu_data)
        {
            float result;
            bool success = float.TryParse(RatedVoltage, out result);
            if (success)
            {
                // 使用转换后的result
                //额定电压(KV) -- 变V                             
                float tempdata = (float)(result * 1000 / Edingzhashu_data);
                //必须保留3位.. 要不然就不对..
                tempdata = (float)Math.Round(tempdata, 3);

                // (临时渣叔)--每一扎电压.
                //这里就得到了 -- 施加电压.
                float Shijiazhashu_data = (float)(Tempzhashu_data * tempdata);
                Shijiazhashu_data = (float)Math.Round(Shijiazhashu_data, 3);

                return Shijiazhashu_data;
            }
            else
            {
                return -1;
            }
        }

        //计算单相施加电压 额定电压..参数2 额定扎数 参数3 临时扎数
        public static float GetCalcAppliedVoltageOne(string RatedVoltage, float Edingzhashu_data, float Tempzhashu_data)
        {
            float result;
            bool success = float.TryParse(RatedVoltage, out result);
            if (success)
            {
                // 使用转换后的result
                //额定电压(KV) -- 变V                             
                float tempdata = (float)(result * 1000 / Edingzhashu_data);
                //必须保留3位.. 要不然就不对..
                tempdata = (float)Math.Round(tempdata, 3);

                // (临时渣叔)--每一扎电压.
                //这里就得到了 -- 施加电压.
                float Shijiazhashu_data = (float)(Tempzhashu_data * tempdata);
                Shijiazhashu_data = (float)Math.Round(Shijiazhashu_data, 3);

                return Shijiazhashu_data;
            }
            else
            {
                return -1;
            }
        }


        //计算 额定电流(三相) 参数1 额定容量(产品)   参数2 施加电压        
        public static float GetCalcRatedCurrentThree(string ProductCapacity, float Shijiazhashu_data)
        {
            float result;
            bool success = float.TryParse(ProductCapacity, out result);
            //转换额定容量 * 1000
            if (success)
            {
                //计算额定电流(不显示-- )
                float NoloadCurrent = (float)(result * 1000 / Shijiazhashu_data / 1.732);
                NoloadCurrent = (float)Math.Round(NoloadCurrent, 3);
                return NoloadCurrent;
            }
            else
            {
                // 处理转换失败的情况                
                return -1;
            }
        }

        //计算空载电流(三相) 参数1 三相平均电流  参数2 三相平均值电压  参数3 施加电压
        public static float GetNoloadCurrentThree(float Irms, float Atpv, float Shijiadianya_data)
        {
            float result;
            result = (float)(Irms/(Atpv/Shijiadianya_data));
            return result;
        }

        //计算额定电流(单相) 参数1 额定容量(产品)   参数2 施加电压        
        public static float GetCalcRatedCurrentOne(string ProductCapacity, float Shijiazhashu_data)
        {
            float result;
            bool success = float.TryParse(ProductCapacity, out result);
            //转换额定容量 * 1000
            if (success)
            {
                //计算额定电流(不显示-- )
                float NoloadCurrent = (float)(result * 1000 / Shijiazhashu_data);
                NoloadCurrent = (float)Math.Round(NoloadCurrent, 3);



                return NoloadCurrent;
            }
            else
            {
                // 处理转换失败的情况                
                return -1;
            }
        }


        //计算空载电流%(三相) 参数1 空载电流.  参数2 额定电流      
        public static float GetCalcRatedCurrentPercentageThree(float NoloadCurrent2, float NoloadCurrent)
        {

            //计算 电流%.(显示 电流%)
            float PercentageNoloadCurrent = NoloadCurrent2 / NoloadCurrent * 100;
            return PercentageNoloadCurrent = (float)Math.Round(PercentageNoloadCurrent, 3);

        }

        //计算空载损耗(三相) 参数1 三相功率.  参数2 电压平均值  参数3 施加电压        
        public static float GetCalcNoloadlossThree(float Pabbcca, float ATPVoltage, float Shijiazhashu_data)
        {
            //计算 空载损耗.
            //float NoloadLoss = (float)(Pabbcca * Math.Pow(ATPVoltage / Shijiazhashu_data, 2));
            float NoloadLoss = (float)(Pabbcca * Math.Pow(Shijiazhashu_data / ATPVoltage, 2));
            return NoloadLoss = (float)Math.Round(NoloadLoss, 3);
        }




        //计算不足800V的产品 -- 600V模拟数据 -- 得到合格不合格结果.
        //1..  --- 2.. -- .. --4 %   
        //(上 8 电流标准  9  电流标准（上）10 电流标准（下）


        public static float GetResultNoloadLoss(float NoloadLoss,//实际空载损耗            
            float ATPVoltage, //平均电压            
            float Shijiazhashu_data) // 施加电压
                                     //string RatedVoltage) //额定电压  
        {
            //float fRatedVoltage; if (!float.TryParse(RatedVoltage, out fRatedVoltage)) return -1;
            //float fNoloadLoss = (float)(NoloadLoss * Math.Pow(((fRatedVoltage * 1000) / Shijiazhashu_data), 2)); 

            float fNoloadLoss = (float)(NoloadLoss * Math.Pow((Shijiazhashu_data / ATPVoltage), 2));
            return fNoloadLoss;
        }



        public static int GetResultThree(float NoloadLoss, //实际空载损耗
            float fPercentageNoloadCurrent,
            string ProductStandard, //空载标准
            string ProductStandardUpperimit,//空载标准上
            string ProductStandardDownimit,//空载标准下
            string ProductCurrentStandard,//电流标准
            string ProductCurrentStandardUpperrimit,//电流标准上
            string ProductCurrentStandardDownimit) //电流标准下
        {
            float fProductStandard; if (!float.TryParse(ProductStandard, out fProductStandard)) return -1;
            float fProductStandardUpperimit; if (!float.TryParse(ProductStandardUpperimit, out fProductStandardUpperimit)) return -1;
            //float fProductStandardDownimit; if (!float.TryParse(ProductStandardDownimit, out fProductStandardDownimit)) return -1;
            float fProductCurrentStandard; if (!float.TryParse(ProductCurrentStandard, out fProductCurrentStandard)) return -1;
            float fProductCurrentStandardUpperrimit; if (!float.TryParse(ProductCurrentStandardUpperrimit, out fProductCurrentStandardUpperrimit)) return -1;
            //float fProductCurrentStandardDownimit; if (!float.TryParse(ProductCurrentStandardDownimit, out fProductCurrentStandardDownimit)) return -1;

            float fNoloadLoss = NoloadLoss;
            //这里只需要上限..
            fProductStandard = fProductStandard + (fProductStandard / fProductStandardUpperimit);
            fProductCurrentStandard = fProductCurrentStandard + (fProductCurrentStandard / fProductCurrentStandardUpperrimit);

            if (fNoloadLoss < fProductStandard && fPercentageNoloadCurrent < fProductCurrentStandard)
                return 1;
            else return 0;
        }

    }


    public static class VisualTreeHelpers
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static System.Windows.Controls.Button ScanButtonFromStackPanel(StackPanel stackPanel, string buttonName)
        {
            foreach (var child in VisualTreeHelpers.FindVisualChildren<UIElement>(stackPanel))
            {
                //得到开始测量按钮
                if (child.GetType().Name == "Button")
                {
                    System.Windows.Controls.Button button = child as System.Windows.Controls.Button;
                    if (button.Name == buttonName)
                    {
                        //child.IsEnabled = false;
                        return button;
                    }
                }
            }
            return null;

        }
    }


    public static class DataGridExtensions
    {
        public static System.Data.DataTable ToDataTable(this DataGrid dataGrid)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();

            // 添加列
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                dataTable.Columns.Add(dataGrid.Columns[i].Header.ToString(), typeof(string));
            }

            // 添加行
            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    //这里应该是内容呀..??
                    //string sstringContent = dataGrid.Items[i][j];
                    //var cellContent = dataGrid.Columns[j][i].Content;
                    var cellContent = dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]);
                    if (cellContent != null && cellContent.DataContext != null)
                    {
                        dataRow[j] = cellContent.DataContext.ToString();
                    }
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }

    public class MyMessage {
        public string Message { get; set; }
        public object obj{ get; set; }
        public string Search { get; set; }
        public int SearchIndex { get; set; }
    }
}
