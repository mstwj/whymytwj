using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;

namespace 铁芯实验.Base
{
    public class DataToExeclcs
    {
        public System.Data.DataTable CopyDataGridToDataTable<T>(DataGrid dataGrid) where T : class, new()
        {
            // 获取DataGrid的数据源
            IEnumerable<T> dataSource = dataGrid.ItemsSource as IEnumerable<T>;
            if (dataSource == null)
                throw new ArgumentException("DataGrid的ItemsSource不是IEnumerable<T>类型");

            // 创建DataTable并定义列
            System.Data.DataTable dataTable = new System.Data.DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                
                dataTable.Columns.Add(property.Name, property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                                        ? Nullable.GetUnderlyingType(property.PropertyType) ?? typeof(object)
                                        : property.PropertyType);
            }

            // 遍历数据源并填充DataTable
            foreach (var item in dataSource)
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (var property in properties)
                {
                    object value = property.GetValue(item, null);
                    // 处理可能的空值和数据类型转换
                    if (value == null)
                    {
                        dataRow[property.Name] = DBNull.Value;
                    }
                    else if (dataTable.Columns[property.Name].DataType.IsAssignableFrom(value.GetType()))
                    {
                        dataRow[property.Name] = value;
                    }
                    else
                    {
                        // 这里可以添加更复杂的类型转换逻辑，或者抛出异常
                        throw new InvalidCastException($"无法将类型 {value.GetType().Name} 转换为 {dataTable.Columns[property.Name].DataType.Name}");
                    }
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }



        public void ExportDataTableToExcel(System.Data.DataTable dataTable, string excelFilePath)
        {

            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            Worksheet worksheet = null;

            if (workbook != null && dataTable != null)
            {
                worksheet = (Worksheet?)workbook.Sheets["Sheet1"];
                if (worksheet == null)
                {
                    worksheet = (Worksheet?)workbook.Sheets.Add();
                    worksheet.Name = "Sheet1";
                }

                for (int i = 1; i <= dataTable.Columns.Count; i++)
                {

                    
                string Header = string.Empty;
                if (dataTable.Columns[i - 1].ColumnName == "Id") Header = "ID";
                if (dataTable.Columns[i - 1].ColumnName == "ReportcheckStartTime")  Header = "记录日期";
                if (dataTable.Columns[i - 1].ColumnName == "ProductNumber") Header = "出厂编号"; 
                if (dataTable.Columns[i - 1].ColumnName == "ProductType") Header = "规格型号"; 
                if (dataTable.Columns[i - 1].ColumnName == "ProductTuhao") Header = "图号"; 
                if (dataTable.Columns[i - 1].ColumnName == "ProductCapacity") Header = "额定容量(kVA)"; 
                if (dataTable.Columns[i - 1].ColumnName == "RatedVoltage") Header = "额定电压(kV)"; 
                if (dataTable.Columns[i - 1].ColumnName == "PhaseNumber") Header = "相数"; 
                if (dataTable.Columns[i - 1].ColumnName == "ATPVoltage") Header = "三相电压平均值(V)"; 
                if (dataTable.Columns[i - 1].ColumnName == "RMSvalue") Header = "三相电压有效值(V)"; 
                if (dataTable.Columns[i - 1].ColumnName == "IRMSvalue") Header = "三相电流有效值(A)"; 
                if (dataTable.Columns[i - 1].ColumnName == "NoloadCurrent") Header = "空载电流(A)"; 
                if (dataTable.Columns[i - 1].ColumnName == "PercentageNoloadCurrent") Header = "空载电流百分比(%)"; 
                if (dataTable.Columns[i - 1].ColumnName == "NoloadLoss") Header = "空载损耗(W)"; 
                if (dataTable.Columns[i - 1].ColumnName == "QualifiedJudgment") Header = "合格判断"; 

                 

                    //worksheet.Cells[1, i] = dataTable.Columns[i - 1].ColumnName;
                    worksheet.Cells[1, i] = Header;
                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataTable.Rows[i][j].ToString();
                    }
                }
            }

            if (excelFilePath != null && excelFilePath != string.Empty)
            {
                try
                {
                    //worksheet.SaveAs(excelFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    worksheet.SaveAs(excelFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing,
                        Type.Missing, Type.Missing);

                }
                catch
                {
                    // Handle exception
                }
            }

            workbook.Close(false, Type.Missing, Type.Missing);
            excelApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            worksheet = null;
            workbook = null;
            excelApp = null;

            GC.Collect();

            System.Windows.MessageBox.Show("保存成功!");
        }

        
        void ExportToWord(DataGrid dataGridView1)
        {
            /*
            Microsoft.Office.Interop.Word.Document mydoc = new Microsoft.Office.Interop.Word.Document();
            Microsoft.Office.Interop.Word.Table mytable;
            Microsoft.Office.Interop.Word.Selection mysel;
            Object myobj;

            //建立Word对象
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            myobj = System.Reflection.Missing.Value;
            mydoc = word.Documents.Add(ref myobj, ref myobj, ref myobj, ref myobj);
            word.Visible = true;
            mydoc.Select();
            mysel = word.Selection;


            //将数据生成Word表格文件
             mytable = mydoc.Tables.Add(mysel.Range, dataGridView1.RowCount, dataGridView1.ColumnCount, ref myobj, ref myobj);

             //设置列宽
             mytable.Columns.SetWidth(30, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustNone);
             //输出列标题数据
             for (int i = 0; i < dataGridView1.ColumnCount; i++)
             {
                 mytable.Cell(1, i + 1).Range.InsertAfter(dataGridView1.Columns[i].Header.ToString());
             }
             
             //输出控件中的记录
             for (int i = 0; i < dataGridView1.RowCount - 1; i++)
             {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        mytable.Cell(i + 2, j + 1).Range.InsertAfter(dataGridView1[j, i].Value.ToString());
                    }
             }
            */
        }
    }
}
