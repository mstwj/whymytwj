using Aspose.Words;
using net5_10_14.Base;
using net5_10_14.Table.bbsy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace net5_10_14
{
    /// <summary>
    /// BianbiReportWindow2.xaml 的交互逻辑
    /// </summary>
    public partial class BianbiReportWindow2 : Window
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public List<CheckBox> checkboxlist { get; set; } = new List<CheckBox>();


        public BianbiReportWindow2()
        {
            InitializeComponent();
            //刷新
            UpdateDataSource();
            //设置显示
            InitializeShow();

        }

        void UpdateDataSource()
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    dataGrid.ItemsSource = null;
                    var bianbi = context.bb_report.ToList();
                    // 设置DataGrid的数据源
                    dataGrid.ItemsSource = bianbi;
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    logger.Error(message);
                }
            }
        }

        void InitializeShow()
        {
            //以下代码制动画...
            bb_report test = new bb_report();
            PropertyInfo[] properties = test.GetType().GetProperties();
            int k = 0;
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "Id") { continue; }

                DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
                dataGridTextColumn.Header = Tools.FindDaseNameToChinese("bb_report", property.Name);
                dataGridTextColumn.Binding = new Binding($"{property.Name}");
                dataGridTextColumn.Width = 150;
                dataGrid.Columns.Add(dataGridTextColumn);

                CheckBox checkbox = new CheckBox();
                checkbox.Content = Tools.FindDaseNameToChinese("bb_report", property.Name);

                Grid.SetRow(checkbox, i);
                Grid.SetColumn(checkbox, k);
                if (k < 2) k++;
                else { k = 0; i++; }

                GridTJ.Children.Add(checkbox);
                checkboxlist.Add(checkbox);

            }
            return;
        }

        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

            Action action = delegate
            {

                //这里是提交后的值...                
                var editedPerson = e.Row.Item as bb_report;
                if (editedPerson == null) return;

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    editedPerson = e.Row.Item as bb_report;
                }

                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.bb_report.Find(editedPerson.Id);

                    if (person != null)
                    {
                        //注意这里不是比较，是赋值了..
                        Tools.ObjectComparer.CompareObjects(person, editedPerson);
                    }
                    context.SaveChanges();
                }


            };
            Dispatcher.BeginInvoke(action, System.Windows.Threading.DispatcherPriority.Background);

            //前面这里可以去验证一下...(不是如果是红框就不来了..)

        }



        private void Button_Click_Save_Word(object sender, RoutedEventArgs e)
        {
            bb_report test = (bb_report)dataGrid.SelectedItem;
            if (test != null)
            {
                PropertyInfo[] properties = test.GetType().GetProperties();

                int i = 0;
                Document doc = new Document();
                DocumentBuilder builder = new DocumentBuilder(doc);

                foreach (PropertyInfo property in properties)
                {
                    if (property.Name == "Id") { continue; }
                    var value = property.GetValue(test);

                    if (checkboxlist[i].IsChecked == true)
                    {
                        object column1 = checkboxlist[i].Content;
                        object column2 = value;
                        builder.Writeln($"{column1.ToString()}\t{column2.ToString()}");
                    }
                    i++;
                }

                string docPath = @"C:\变比document.docx";
                doc.Save(docPath);

                MessageBox.Show("导出WORD完成-文档位置:" + docPath);

            }
            else
            {
                MessageBox.Show("请先选择一行记录!");
            }

        }

        private void Button_Click_Save_Text(object sender, RoutedEventArgs e)
        {
            bb_report test = (bb_report)dataGrid.SelectedItem;
            int i = 0;
            if (test != null)
            {
                PropertyInfo[] properties = test.GetType().GetProperties();
                string filePath = @"C:\变比export.txt";
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.Name == "Id") { continue; }
                        var value = property.GetValue(test);

                        if (checkboxlist[i].IsChecked == true)
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
        }


    }
}
