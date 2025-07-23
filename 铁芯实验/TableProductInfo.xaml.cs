using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CommunityToolkit.Mvvm.Messaging;
using 铁芯实验.Base;
using 铁芯实验.Table;
using static 铁芯实验.Base.DbContextExtensions;

namespace 铁芯实验
{
    /// <summary>
    /// TableProductInfo.xaml 的交互逻辑
    /// </summary>
    public partial class TableProductInfo : Window
    {
        private List<Table_ProductInfo> dataSource = new List<Table_ProductInfo>();


        //查询--
        public string SelectXinhao { get; set; } = string.Empty;
        public TableProductInfo()
        {
            InitializeComponent();
            this.DataContext = this;
            //刷新
            UpdateDataSource();

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, user) =>
            {
                if (user.Message == "SearchTableProduction")
                {
                    if (!string.IsNullOrEmpty(user.Search))
                    {
                        List<Table_ProductInfo>? queryResult = null;
                        if (user.SearchIndex == 0) queryResult = dataSource.Where(item => item.ProductNumber.Contains(user.Search)).ToList(); // 示例：按名字查询
                        if (user.SearchIndex == 1) queryResult = dataSource.Where(item => item.ProductType.Contains(user.Search)).ToList(); // 示例：按名字查询
                        if (user.SearchIndex == 2) queryResult = dataSource.Where(item => item.ProductTuhao.Contains(user.Search)).ToList(); // 示例：按名字查询
                        if (user.SearchIndex == 3) queryResult = dataSource.Where(item => item.ProductCapacity.Contains(user.Search)).ToList(); // 示例：按名字查询
                        if (user.SearchIndex == 4) queryResult = dataSource.Where(item => item.RatedVoltage.Contains(user.Search)).ToList(); // 示例：按名字查询
                        if (queryResult != null) dataGrid.ItemsSource = queryResult;
                    }
                    else
                    {
                         dataGrid.ItemsSource = dataSource;
                    }
                }

            });
        }

        void UpdateDataSource()
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    dataGrid.ItemsSource = null;
                    dataSource = context.ProductInfo.ToList();
                    dataGrid.ItemsSource = dataSource;
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    MessageBox.Show("更新数据源失败" + message);
                }
            }
        }

        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            
            Action action = delegate
            {

                //这里是提交后的值...                
                var editedPerson = e.Row.Item as Table_ProductInfo;
                if (editedPerson == null) return;

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    editedPerson = e.Row.Item as Table_ProductInfo;
                }

                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.ProductInfo.Find(editedPerson.Id);

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
            /*
            if (!string.IsNullOrEmpty(SelectXinhao))
            {
                var queryResult = dataSource.Where(item => item.ProductType.Contains(SelectXinhao)).ToList(); // 示例：按名字查询
                dataGrid.ItemsSource = queryResult;
            }
            else
            {
                dataGrid.ItemsSource = dataSource;
            }
            */
            QueryDialog queryDialog = new QueryDialog();
            queryDialog.QueryCBoxData.Add("出厂编号");
            queryDialog.QueryCBoxData.Add("规格型号");
            queryDialog.QueryCBoxData.Add("图号");
            queryDialog.QueryCBoxData.Add("额定容量");
            queryDialog.QueryCBoxData.Add("额定电压");
            queryDialog.MainDialog = "TableProduction";
            queryDialog.ShowDialog();
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            //删除..
            // 获取当前选中的项
            var selectedItem = dataGrid.SelectedItem;

            // 如果你需要操作行数据，可以将选中的项转换为适当的类型
            if (selectedItem != null && selectedItem is Table_ProductInfo)
            {
                Table_ProductInfo selectedData = (Table_ProductInfo)selectedItem;
                // 现在你可以使用selectedData来访问行数据
                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.ProductInfo.Find(selectedData.Id);

                    if (person != null)
                    {
                        //注意这里不是比较，是赋值了..
                        context.ProductInfo.Remove(person);
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
