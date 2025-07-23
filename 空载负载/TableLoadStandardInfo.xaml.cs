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
using CommunityToolkit.Mvvm.Messaging;
using 空载负载.Base;
using 空载负载.Model;
using 空载负载.Table;
using static 空载负载.Base.DbContextExtensions;

namespace 空载负载
{
    /// <summary>
    /// TableStandardInfo.xaml 的交互逻辑
    /// </summary>
    public partial class TableLoadStandardInfo : Window
    {
        TableLoadStandardModel model = new TableLoadStandardModel();
        private List<Table_LoadStandardInfo> dataSource = new List<Table_LoadStandardInfo>();
        //public string SelectXinhao { get; set; } = string.Empty;

        public TableLoadStandardInfo()
        {
            InitializeComponent();
            // 注册消息..(必须在前面..)
            WeakReferenceMessenger.Default.Register<string>(this, (r, user) => 
            {
                if (user == "添加成功")
                {
                    UpdateDataSource();
                }
            });
            //刷新
            UpdateDataSource();
            this.DataContext = model;

        }



        void UpdateDataSource()
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    dataGrid.ItemsSource = null;
                    dataSource = context.LoadStandardInfo.ToList();                    
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
                var editedPerson = e.Row.Item as Table_LoadStandardInfo;
                if (editedPerson == null) return;

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    editedPerson = e.Row.Item as Table_LoadStandardInfo;
                }

                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.LoadStandardInfo.Find(editedPerson.Id);

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

        private void InitShowProc(object recipient, string message)
        {
            if (message == "添加成功")
            {                
                UpdateDataSource();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //删除..
            // 获取当前选中的项
            var selectedItem = dataGrid.SelectedItem;

            // 如果你需要操作行数据，可以将选中的项转换为适当的类型
            if (selectedItem != null && selectedItem is Table_LoadStandardInfo)
            {
                Table_LoadStandardInfo selectedData = (Table_LoadStandardInfo)selectedItem;
                // 现在你可以使用selectedData来访问行数据
                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.LoadStandardInfo.Find(selectedData.Id);

                    if (person != null)
                    {
                        //注意这里不是比较，是赋值了..
                        context.LoadStandardInfo.Remove(person);
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

        private void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(model.SelectXinhao))
            {
                var queryResult = dataSource.Where(item => item.ProductType.Contains(model.SelectXinhao)).ToList(); // 示例：按名字查询
                dataGrid.ItemsSource = queryResult;
            }
            else
            {
                dataGrid.ItemsSource = dataSource;
            }
        }
    }
}
