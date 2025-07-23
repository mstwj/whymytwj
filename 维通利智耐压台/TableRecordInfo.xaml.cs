using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using 维通利智耐压台.Base;
using 维通利智耐压台.MyTable;
using static 维通利智耐压台.Base.DbContextExtensions;
using System.Data;
using System.ComponentModel;
using S7.Net.Types;
using System.Collections.ObjectModel;
using Azure;
using CommunityToolkit.Mvvm.Messaging;
using LiveCharts.Defaults;
using LiveCharts;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;


namespace 维通利智耐压台
{
    /// <summary>
    /// TableRecordInfo.xaml 的交互逻辑
    /// </summary>
    public partial class TableRecordInfo : Window
    {
        public List<string> BtnComboBox { get; set; } = new List<string>();
        public string SelectSea { get; set; } = string.Empty;
        public int BtnComboBoxIndex { get; set; } = 1;

        private List<Tabe_Record> dataSource = new List<Tabe_Record>();

        private Dictionary<Tabe_Record, List<Tabe_Record>> ageDictionary = new Dictionary<Tabe_Record, List<Tabe_Record>>();

        public TableRecordInfo()
        {
            InitializeComponent();
            this.DataContext = this;
            //刷新
            UpdateDataSource(-1);

            BtnComboBox.Add("产品名称");
            BtnComboBox.Add("出厂编号");
            BtnComboBox.Add("规格型号");
            BtnComboBox.Add("图号");

            //设置显示
            //LoadPage(_currentPage);
            /*
            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, user) =>
            {
                if (user.Message == "SearchTableRecord")
                {
                    if (!string.IsNullOrEmpty(user.Search))
                    {
                        List<Tabe_Record>? queryResult = null;
                        if (user.SearchIndex == 0) queryResult = dataSource.Where(item => item.ProductName.Contains(user.Search)).ToList(); // 示例：按名字查询
                        if (user.SearchIndex == 1) queryResult = dataSource.Where(item => item.ProductNumber.Contains(user.Search)).ToList(); // 示例：按名字查询
                        if (user.SearchIndex == 2) queryResult = dataSource.Where(item => item.ProductType.Contains(user.Search)).ToList(); // 示例：按名字查询
                        if (queryResult != null)
                        {
                            dataGrid.ItemsSource = queryResult;
                            user.obj1 = dataSource;
                        }
                    }
                    else
                    {
                        //显示所有数据...
                        dataGrid.ItemsSource = dataSource;
                    }
                }

            });
            */

        }

        void UpdateDataSource(int oresult)
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    if (oresult == -1)
                    {
                        //这里不能直接给..(要分组..)
                        dataSource = context.naiya_record.ToList();
                        dataSource = dataSource.OrderBy(item => item.Id).ToList(); ;
                        //if (dataSource.Count > 0)
                        {
                            EditToImage(dataSource);
                            //dataGrid.ItemsSource = dataSource;
                            dataGrid.ItemsSource = ageDictionary.Keys.ToList();
                        }
                    }

                    if (oresult == 0)
                    {
                        var queryResult = dataSource.Where(item => item.ProductName.Contains(SelectSea)).ToList(); // 示例：按名字查询                        EditToImage(dataSource);
                        EditToImage(queryResult);
                        dataGrid.ItemsSource = ageDictionary.Keys.ToList();
                    }

                    if (oresult == 1)
                    {
                        var queryResult = dataSource.Where(item => item.ProductNumber.Contains(SelectSea)).ToList(); // 示例：按名字查询
                        EditToImage(queryResult);
                        dataGrid.ItemsSource = ageDictionary.Keys.ToList();
                    }

                    if (oresult == 2)
                    {
                        var queryResult = dataSource.Where(item => item.ProductType.Contains(SelectSea)).ToList(); // 示例：按名字查询
                        EditToImage(queryResult);
                        dataGrid.ItemsSource = ageDictionary.Keys.ToList();
                    }

                    if (oresult == 3)
                    {
                        var queryResult = dataSource.Where(item => item.ProductTuhao.Contains(SelectSea)).ToList(); // 示例：按名字查询
                        EditToImage(queryResult);
                        dataGrid.ItemsSource = ageDictionary.Keys.ToList();
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    System.Windows.MessageBox.Show("更新数据源失败" + message);
                }
            }
        }

        /*
         * 
         * RowEditEnding="dataGrid_RowEditEnding"                                              HTML
        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            
            System.Action action = delegate
            {

                //这里是提交后的值...                
                var editedPerson = e.Row.Item as Tabe_Record;
                if (editedPerson == null) return;

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    editedPerson = e.Row.Item as Tabe_Record;
                }

                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.naiya_record.Find(editedPerson.Id);

                    if (person != null)
                    {
                        //注意这里不是比较，是赋值了..
                        ObjectComparer.CompareObjects(person, editedPerson);
                    }
                    context.SaveChanges();
                }


            };
            Dispatcher.BeginInvoke(action, System.Windows.Threading.DispatcherPriority.Background);

            //前面这里可以去验证一下...(不是如果是红框就不来了..)
            
        }
        */

        private void  TestButton_Click(object sender, RoutedEventArgs e)
        {
            //MyConvert.MyTest();
        }

        private void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (!string.IsNullOrEmpty(SelectSea))
            {
                UpdateDataSource(BtnComboBoxIndex);
            }
            else
            {
                UpdateDataSource(-1);
            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            //删除..
            // 获取当前选中的项
            var selectedItem = dataGrid.SelectedItem;

            // 如果你需要操作行数据，可以将选中的项转换为适当的类型
            if (selectedItem != null && selectedItem is Tabe_Record)
            {
                //这里比较复炸..
                List < Tabe_Record > deletedata = ageDictionary[(Tabe_Record)selectedItem];



                Tabe_Record selectedData = (Tabe_Record)selectedItem;
                // 现在你可以使用selectedData来访问行数据
                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    //var person = context.naiya_record.Find(selectedData.Id);

                    //if (person != null)
                    {
                        //注意这里不是比较，是赋值了..
                        context.naiya_record.RemoveRange(deletedata);
                        int rowsAffected = context.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("删除成功！");
                            UpdateDataSource(-1);
                        }
                        else
                        {
                            MessageBox.Show("删除失败，未找到相应实体。");
                        }
                    }
                }
            }
        }

        private void Button_Click_Save_Word(object sender, RoutedEventArgs e)
        {
            /*
            string sFileName = TB1.Text.Trim(); 
            if (sFileName.Length <= 0)
            {
                System.Windows.MessageBox.Show("请输入文件名!");
                return;
            }

            string exec = Assembly.GetExecutingAssembly().Location;

            string input = exec;
            char delimiter = '\\';
            string result = MyTools.RemoveLastPart(input, delimiter);
            result = result + '\\' + sFileName + ".xls";
            exec = result;

            DataToExeclcs dataToExeclcs = new DataToExeclcs();
            DataTable dt = dataToExeclcs.CopyDataGridToDataTable<Table_RocreadInfo>(dataGrid);
            dataToExeclcs.ExportDataTableToExcel(dt, exec);
            */
        }


        private void Button_Click_Save_Text(object sender, RoutedEventArgs e)
        {
            /*
            directlease_report test = (directlease_report)dataGrid.SelectedItem;
            int i = 0;
            if (test != null)
            {
                PropertyInfo[] properties = test.GetType().GetProperties();
                string filePath = @"C:\export.txt";
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.Name == "Id") { continue; }
                        var value = property.GetValue(test);

                        {
                            object column1 = checkboxlist[i].Content;
                            object column2 = value;
                            //builder.Writeln($"{column1.ToString()}\t{column2.ToString()}");
                            writer.Write($"{column1.ToString()}\t{column2.ToString()}");
                            writer.WriteLine();
                        }
                        i++;
                    }
                }
                MessageBox.Show("导出TXT完成-文档位置:" + filePath);
            }
            else
            {
                MessageBox.Show("请先选择一行记录!");
            }
            */
        }


        private void EditToImage(List<Tabe_Record> dataSource)
        {
            ageDictionary.Clear();

            List<Tabe_Record> currentList = new List<Tabe_Record>();

            bool add = false;
            //这里使用了很傻的一种方式..
            foreach (var item in dataSource)
            {
                if (item.BeginTimer == "Begin") // 开始新的段
                {
                    currentList.Add(item);
                    add = true;
                    continue;
                }
                if (add == true)
                {
                    if (item.BeginTimer == "End")
                    {
                        currentList.Add(item);
                        add = false;
                        Tabe_Record p = new Tabe_Record();
                        //这里需要相同的类型..
                        ObjectComparer.CompareObjects(p, item);
                        ageDictionary.Add(p, currentList);
                        currentList = new List<Tabe_Record>();
                        continue;
                    }
                    currentList.Add(item); // 添加开始标记到当前段
                }
            }

            return;

        }


        private void Button_Click_Export(object sender, RoutedEventArgs e)
        {
            // 获取当前选中的项
            Tabe_Record selectedItem = (Tabe_Record)dataGrid.SelectedItem;

            // 如果你需要操作行数据，可以将选中的项转换为适当的类型
            if (selectedItem != null && selectedItem is Tabe_Record)
            {
                List<Tabe_Record> data = ageDictionary[(Tabe_Record)selectedItem];
                ExportWindow exportWindow = new ExportWindow();
                exportWindow.mode.Operate = string.Empty;
                exportWindow.mode.Airpressure = string.Empty;
                exportWindow.mode.Temperature = string.Empty;
                exportWindow.mode.ProductName = selectedItem.ProductName;
                //产品编号
                exportWindow.mode.ProductNumber = selectedItem.ProductNumber;
                //产品型号
                exportWindow.mode.ProductType = selectedItem.ProductType;
                //图号
                exportWindow.mode.ProductTuhao = selectedItem.ProductTuhao;
                //标准电压
                exportWindow.mode.ProductStardVotil = selectedItem.ProductStardVotil;
                //标准局放
                exportWindow.mode.ProductStardPartial = selectedItem.ProductStardPartial;
                //开始时间
                exportWindow.mode.RecordDateTimer = selectedItem.RecordDateTimer.ToString();                
                //施加部位
                exportWindow.mode.ProductParts = selectedItem.ProductParts;
                //合格判断..
                exportWindow.mode.ProductQualified = string.Empty;

                //3个阶段的平均值..
                List<float> flevelV1 = new List<float>();
                List<float> flevelV2 = new List<float>();
                List<float> flevelV3 = new List<float>();
                List<float> flevelPc1 = new List<float>();
                List<float> flevelPc2 = new List<float>();
                List<float> flevelPc3 = new List<float>();

                float fpartial, fvotil;

                //这里不需要
                //data.RemoveRange(0, 1); // 移除前1个元素
                //data.RemoveRange(data.Count - 1, 1); // 移除后1个元素

                //exportWindow.mode.QueryServerLineData.Add(new ObservableValue(0));
                //exportWindow.mode.QueryServerLineData2.Add(new ObservableValue(0));

                
                List<Tabe_Record>? level1data = data.Where(item => item.LevelCode.Contains("Level1")).ToList();
                List<Tabe_Record>? level2data = data.Where(item => item.LevelCode.Contains("Level2")).ToList();
                List<Tabe_Record>? level3data = data.Where(item => item.LevelCode.Contains("Level3")).ToList();

                //先画表格..
                foreach (var item in data)
                {
                    if (!float.TryParse(item.Votil, out fvotil))
                    {
                        MessageBox.Show("严重错误，转换失败");
                        return;
                    }

                    if (!float.TryParse(item.Partial, out fpartial))
                    {
                        MessageBox.Show("严重错误，转换失败");
                        return;
                    }
                    exportWindow.mode.QueryServerLineData.Add(new ObservableValue(fpartial));
                    exportWindow.mode.QueryServerLineData2.Add(new ObservableValue(fvotil));
                }


                foreach(var item in level1data)
                {
                    if (!float.TryParse(item.Votil,out  fvotil))
                    {
                        MessageBox.Show("第一阶段数据无法转换,严重错误，转换失败");
                        return;
                    }

                    if (!float.TryParse(item.Partial, out fpartial))
                    {
                        MessageBox.Show("第一阶段数据无法转换,严重错误，转换失败");
                        return;
                    }
                    flevelV1.Add(fvotil);
                    flevelPc1.Add(fpartial);
                }

                foreach (var item in level2data)
                {
                    if (!float.TryParse(item.Votil, out fvotil))
                    {
                        MessageBox.Show("第2阶段数据无法转换,严重错误，转换失败");
                        return;
                    }

                    if (!float.TryParse(item.Partial, out fpartial))
                    {
                        MessageBox.Show("第2阶段数据无法转换,严重错误，转换失败");
                        return;
                    }
                    flevelV2.Add(fvotil);
                    flevelPc2.Add(fpartial);
                }


                foreach (var item in level3data)
                {
                    if (!float.TryParse(item.Votil, out fvotil))
                    {
                        MessageBox.Show("第3阶段数据无法转换,严重错误，转换失败");
                        return;
                    }

                    if (!float.TryParse(item.Partial, out fpartial))
                    {
                        MessageBox.Show("第3阶段数据无法转换,严重错误，转换失败");
                        return;
                    }
                    flevelV3.Add(fvotil);
                    flevelPc3.Add(fpartial);
                }

                if (flevelV1.Count == 0)
                {
                    MessageBox.Show("没找到任何有效数据,试验期间可能被强制中断");
                    return;
                }

                if (flevelV1.Count > 0 && 
                flevelV2.Count > 0 &&
                flevelV3.Count > 0)
                {

                    if (flevelV1.Count < 5 || flevelV2.Count < 5 || flevelV3.Count < 5)
                    {
                        MessageBox.Show("数据异常-中途可能被强制中断，有效数据个数小于5个.");
                        return;
                    }

                    exportWindow.mode.LevelTime1 = flevelV1.Count().ToString();
                    exportWindow.mode.LevelTime2 = flevelV2.Count().ToString();
                    exportWindow.mode.LevelTime3 = flevelV3.Count().ToString();

                    exportWindow.mode.LevelV1 = GetMyAverage(flevelV1).ToString();
                    //string 保留2位，使用正则...
                    exportWindow.mode.LevelV1 = Regex.Replace(exportWindow.mode.LevelV1, @"(\.\d{2})\d*$", "$1");

                    exportWindow.mode.LevelV2 = GetMyAverage(flevelV2).ToString();
                    exportWindow.mode.LevelV2 = Regex.Replace(exportWindow.mode.LevelV2, @"(\.\d{2})\d*$", "$1");

                    exportWindow.mode.LevelV3 = GetMyAverage(flevelV3).ToString();
                    exportWindow.mode.LevelV3 = Regex.Replace(exportWindow.mode.LevelV3, @"(\.\d{2})\d*$", "$1");

                    exportWindow.mode.LevelPc1 = GetMyAverage(flevelPc1).ToString();
                    exportWindow.mode.LevelPc1 = Regex.Replace(exportWindow.mode.LevelPc1, @"(\.\d{2})\d*$", "$1");

                    exportWindow.mode.LevelPc2 = GetMyAverage(flevelPc2).ToString();
                    exportWindow.mode.LevelPc2 = Regex.Replace(exportWindow.mode.LevelPc2, @"(\.\d{2})\d*$", "$1");

                    exportWindow.mode.LevelPc3 = GetMyAverage(flevelPc3).ToString();
                    exportWindow.mode.LevelPc3 = Regex.Replace(exportWindow.mode.LevelPc3, @"(\.\d{2})\d*$", "$1");


                    float pc1, startpc, pc2, pc3;
                    if (!float.TryParse(selectedItem.ProductStardPartial, out startpc))
                    {
                        MessageBox.Show("转换失败StartPc,严重错误..");
                        return;
                    }

                    if (!float.TryParse(exportWindow.mode.LevelPc1, out pc1))
                    {
                        MessageBox.Show("转换失败Pc1,严重错误..");
                        return;
                    }

                    if (!float.TryParse(exportWindow.mode.LevelPc2, out pc2))
                    {
                        MessageBox.Show("转换失败Pc2,严重错误..");
                        return;
                    }

                    if (!float.TryParse(exportWindow.mode.LevelPc3, out pc3))
                    {
                        MessageBox.Show("转换失败Pc3,严重错误..");
                        return;
                    }

                    if (pc1 > startpc)
                        exportWindow.mode.LevelIsGood1 = "不合格";
                    else
                        exportWindow.mode.LevelIsGood1 = "合格";

                    if (pc2 > startpc)
                        exportWindow.mode.Leve2IsGood2 = "不合格";
                    else
                        exportWindow.mode.Leve2IsGood2 = "合格";

                    if (pc3 > startpc)
                        exportWindow.mode.Leve3IsGood3 = "不合格";
                    else
                        exportWindow.mode.Leve3IsGood3 = "合格";
                }
                else
                {
                    if (flevelV1.Count > 0 && flevelV2.Count > 0)
                    {

                        if (flevelV1.Count < 5 || flevelV2.Count < 5 )
                        {
                            MessageBox.Show("L2数据异常-中途可能被强制中断，有效数据个数小于5个.");
                            return;
                        }

                        exportWindow.mode.LevelTime1 = flevelV1.Count().ToString();
                        exportWindow.mode.LevelTime2 = flevelV2.Count().ToString();
                 
                        exportWindow.mode.LevelV1 = GetMyAverage(flevelV1).ToString();
                        //string 保留2位，使用正则...
                        exportWindow.mode.LevelV1 = Regex.Replace(exportWindow.mode.LevelV1, @"(\.\d{2})\d*$", "$1");

                        exportWindow.mode.LevelV2 = GetMyAverage(flevelV2).ToString();
                        exportWindow.mode.LevelV2 = Regex.Replace(exportWindow.mode.LevelV2, @"(\.\d{2})\d*$", "$1");

                  
                        exportWindow.mode.LevelPc1 = GetMyAverage(flevelPc1).ToString();
                        exportWindow.mode.LevelPc1 = Regex.Replace(exportWindow.mode.LevelPc1, @"(\.\d{2})\d*$", "$1");

                        exportWindow.mode.LevelPc2 = GetMyAverage(flevelPc2).ToString();
                        exportWindow.mode.LevelPc2 = Regex.Replace(exportWindow.mode.LevelPc2, @"(\.\d{2})\d*$", "$1");

                   

                        float pc1, startpc, pc2, pc3;
                        if (!float.TryParse(selectedItem.ProductStardPartial, out startpc))
                        {
                            MessageBox.Show("转换失败StartPc,严重错误..");
                            return;
                        }

                        if (!float.TryParse(exportWindow.mode.LevelPc1, out pc1))
                        {
                            MessageBox.Show("转换失败Pc1,严重错误..");
                            return;
                        }

                        if (!float.TryParse(exportWindow.mode.LevelPc2, out pc2))
                        {
                            MessageBox.Show("转换失败Pc2,严重错误..");
                            return;
                        }

                  
                        if (pc1 > startpc)
                            exportWindow.mode.LevelIsGood1 = "不合格";
                        else
                            exportWindow.mode.LevelIsGood1 = "合格";

                        if (pc2 > startpc)
                            exportWindow.mode.Leve2IsGood2 = "不合格";
                        else
                            exportWindow.mode.Leve2IsGood2 = "合格";
                    }
                    else
                    {
                        if (flevelV1.Count > 0)
                        {
                            if (flevelV1.Count < 5 )
                            {
                                MessageBox.Show("L1数据异常-中途可能被强制中断，有效数据个数小于5个.");
                                return;
                            }

                            exportWindow.mode.LevelTime1 = flevelV1.Count().ToString();

                            exportWindow.mode.LevelV1 = GetMyAverage(flevelV1).ToString();
                            //string 保留2位，使用正则...
                            exportWindow.mode.LevelV1 = Regex.Replace(exportWindow.mode.LevelV1, @"(\.\d{2})\d*$", "$1");

                            exportWindow.mode.LevelPc1 = GetMyAverage(flevelPc1).ToString();
                            exportWindow.mode.LevelPc1 = Regex.Replace(exportWindow.mode.LevelPc1, @"(\.\d{2})\d*$", "$1");

                            float pc1, startpc;
                            if (!float.TryParse(selectedItem.ProductStardPartial, out startpc))
                            {
                                MessageBox.Show("转换失败StartPc,严重错误..");
                                return;
                            }

                            if (!float.TryParse(exportWindow.mode.LevelPc1, out pc1))
                            {
                                MessageBox.Show("转换失败Pc1,严重错误..");
                                return;
                            }

                            if (pc1 > startpc)
                                exportWindow.mode.LevelIsGood1 = "不合格";
                            else
                                exportWindow.mode.LevelIsGood1 = "合格";

                        }

                    }
                }
                exportWindow.ShowDialog();
            }
        }

        private float GetMyAverage(List<float> data)
        {
            data.RemoveRange(0, 2); // 移除前两个元素
            data.RemoveRange(data.Count - 2, 2); // 移除后两个元素
            float VAll = data.Sum();
            VAll = VAll / data.Count();                         
            return VAll;
        }
    }
}

