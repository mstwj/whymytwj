using System;
using System.Collections.Generic;
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
using 空载负载.Base;
using 空载负载.Table;
using static 空载负载.Base.DbContextExtensions;

namespace 空载负载
{
    /// <summary>
    /// TableInductionRecordDialog.xaml 的交互逻辑
    /// </summary>
    public partial class TableInductionRecordDialog : Window
    {
        public string Search { get; set; }
        private List<Table_InductionRecordInfo> dataSource = new List<Table_InductionRecordInfo>();

        public TableInductionRecordDialog()
        {
            InitializeComponent();
            UpdateDataSource();
        }

        void UpdateDataSource()
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    dataGrid.ItemsSource = null;
                    dataSource = context.InductionRecordInfo.ToList();
                    dataGrid.ItemsSource = dataSource;
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    System.Windows.MessageBox.Show("更新数据源失败" + message);
                }
            }
        }


        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

            System.Action action = delegate
            {

                //这里是提交后的值...                
                var editedPerson = e.Row.Item as Table_InductionRecordInfo;
                if (editedPerson == null) return;

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    editedPerson = e.Row.Item as Table_InductionRecordInfo;
                }

                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.InductionRecordInfo.Find(editedPerson.Id);

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


        private void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Search))
            {
                var queryResult = dataSource.Where(item => item.ProductType.Contains(Search)).ToList(); // 示例：按名字查询
                dataGrid.ItemsSource = queryResult;
            }
            else
            {
                dataGrid.ItemsSource = dataSource;
            }
        }

        private void Button_Click_Save_Word(object sender, RoutedEventArgs e)
        {
            string sFileName = TB1.Text.Trim();
            if (sFileName.Length <= 0)
            {
                System.Windows.MessageBox.Show("请输入文件名!");
                return;
            }

            if (sFileName.Length > 12)
            {
                System.Windows.MessageBox.Show("文件名长度不能大于12!");
                return;
            }

            string exec = Assembly.GetExecutingAssembly().Location;

            string input = exec;
            char delimiter = '\\';
            string result = MyTools.RemoveLastPart(input, delimiter);
            result = result + '\\' + sFileName + ".xls";
            exec = result;

            DataToExeclcs dataToExeclcs = new DataToExeclcs();
            //DataTable dt = dataToExeclcs.CopyDataGridToDataTable<Table_NoloadRocreadInfo>(dataGrid);
            //dataToExeclcs.ExportDataTableToExcel(dt, exec);
        }



        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            //删除..
            // 获取当前选中的项
            var selectedItem = dataGrid.SelectedItem;

            // 如果你需要操作行数据，可以将选中的项转换为适当的类型
            if (selectedItem != null && selectedItem is Table_InductionRecordInfo)
            {
                Table_InductionRecordInfo selectedData = (Table_InductionRecordInfo)selectedItem;
                // 现在你可以使用selectedData来访问行数据
                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.InductionRecordInfo.Find(selectedData.Id);

                    if (person != null)
                    {
                        //注意这里不是比较，是赋值了..
                        context.InductionRecordInfo.Remove(person);
                        int rowsAffected = context.SaveChanges();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("删除成功！");
                            UpdateDataSource();
                        }
                        else
                        {
                            MessageBox.Show("删除失败，未找到相应实体。");
                        }
                    }
                }
            }
        }

    }
}
