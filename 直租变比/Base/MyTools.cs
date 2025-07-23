using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Office.Interop.Excel;

using 直租变比.Model;
using 直租变比.Table;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace 直租变比.Base
{
    public static class MyTools
    {
        //设置当前产品信息
        public static bool GetCurrentUserInfo(UserInfoModel m_userInfoModel)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var firstEntity = context.ProductInfo.FirstOrDefault(e => e.ProductNumber == MainWindow.Current_ProductNumber);
                    if (firstEntity == null)
                    {
                        return false;
                    }
                    m_userInfoModel.ProductNumber = firstEntity.ProductNumber;
                    m_userInfoModel.ProductType = firstEntity.ProductType;
                    m_userInfoModel.ProductTuhao = firstEntity.ProductTuhao;
                    m_userInfoModel.ProductCapacity = firstEntity.ProductCapacity;
                    m_userInfoModel.RatedVoltage = firstEntity.RatedVoltage;
                    m_userInfoModel.PhaseNumber = firstEntity.PhaseNumber;

                    return true;
                }
            }
            catch (Exception ex) { return false; }
        }

        //保存数据到数据库.
        public static bool SaveDataToDataBase(UserRecordModel userRecordModel,string Info)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    Table_RocreadInfo table_RocreadInfo = new Table_RocreadInfo();

                    table_RocreadInfo.Id = 0; //(ID 产品表)
                    table_RocreadInfo.ReportcheckStartTime = DateTime.Now;

                    table_RocreadInfo.ProductNumber = Info;
                    //三相电压平均值
                    table_RocreadInfo.ATPVoltage = userRecordModel.ATPVoltage.ToString();
                    //三相电压有效值
                    table_RocreadInfo.RMSvalue= userRecordModel.RMSvalue.ToString();
                    //三相电流有效值
                    table_RocreadInfo.IRMSvalue= userRecordModel.IRMSvalue.ToString();
                    //空载电流
                    table_RocreadInfo.NoloadCurrent= userRecordModel.NoloadCurrent.ToString();
                    //空载电流%
                    table_RocreadInfo.PercentageNoloadCurrent= userRecordModel.PercentageNoloadCurrent.ToString();
                    //空载损耗
                    table_RocreadInfo.NoloadLoss= userRecordModel.NoloadLoss.ToString();
                    //合格判断
                    table_RocreadInfo.QualifiedJudgment= userRecordModel.QualifiedJudgment.ToString();

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
                float tempdata = (float)(result * 1000 / 1.732 / Edingzhashu_data);
                //必须保留3位.. 要不然就不对..
                tempdata = (float)Math.Round(tempdata, 3);

                // (临时渣叔)--每一扎电压.
                //这里就得到了 -- 施加电压.
                float Shijiazhashu_data = (float)(Tempzhashu_data * tempdata * 1.732);
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
                float Shijiazhashu_data = (float)(Tempzhashu_data * tempdata );
                Shijiazhashu_data = (float)Math.Round(Shijiazhashu_data, 3);

                return Shijiazhashu_data;
            }
            else
            {
                return -1;
            }
        }
        

        //计算额定电流(三相) 参数1 额定容量(产品)   参数2 施加电压        
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


        //计算额定电流(单相) 参数1 额定容量(产品)   参数2 施加电压        
        public static float GetCalcRatedCurrentOne(string ProductCapacity, float Shijiazhashu_data)
        {
            float result;
            bool success = float.TryParse(ProductCapacity, out result);
            //转换额定容量 * 1000
            if (success)
            {
                //计算额定电流(不显示-- )
                float NoloadCurrent = (float)(result * 1000 / Shijiazhashu_data );
                NoloadCurrent = (float)Math.Round(NoloadCurrent, 3);
                return NoloadCurrent;
            }
            else
            {
                // 处理转换失败的情况                
                return -1;
            }
        }


        //计算额定电流%(三相) 参数1 三相电流平均值 (有效值).  参数2 空载电流      
        public static float GetCalcRatedCurrentPercentageThree(float IRMSvalue, float NoloadCurrent)
        {
            //计算 电流%.(显示 电流%)
            float PercentageNoloadCurrent = IRMSvalue / NoloadCurrent * 100;
            return PercentageNoloadCurrent = (float)Math.Round(PercentageNoloadCurrent, 3);

        }

        //计算空载损耗(三相) 参数1 三相功率.  参数2 电压平均值  参数3 施加电压        
        public static float GetCalcNoloadlossThree(float Pabbcca,float ATPVoltage, float Shijiazhashu_data)
        {
            //计算 空载损耗.
            float NoloadLoss = (float)(Pabbcca * Math.Pow(ATPVoltage / Shijiazhashu_data, 2));
            return NoloadLoss = (float)Math.Round(NoloadLoss, 3);
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


}
