using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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


namespace 维通利智耐压台
{
    /// <summary>
    /// TableProductInfo.xaml 的交互逻辑
    /// </summary>
    public partial class TableProductInfo : Window
    {
        private List<Table_Product> dataSource = new List<Table_Product>();

        public List<string> BtnComboBox { get; set; } = new List<string>();
        
        public string SelectSea { get; set; } = string.Empty;

        public int BtnComboBoxIndex { get; set; } = 1;

        public TableProductInfo()
        {
            InitializeComponent();
            this.DataContext = this;

            BtnComboBox.Add("产品名称");
            BtnComboBox.Add("出厂编号");
            BtnComboBox.Add("规格型号");
            BtnComboBox.Add("图号");
 
            //刷新
            UpdateDataSource();
        }

        void UpdateDataSource()
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    dataGrid.ItemsSource = null;
                    dataSource = context.naiya_product.ToList();
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
                var editedPerson = e.Row.Item as Table_Product;
                if (editedPerson == null) return;

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    editedPerson = e.Row.Item as Table_Product;
                }

                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.naiya_product.Find(editedPerson.Id);

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
            
            if (!string.IsNullOrEmpty(SelectSea))
            {
                if (BtnComboBoxIndex == 0)
                {
                    var queryResult = dataSource.Where(item => item.ProductName.Contains(SelectSea)).ToList(); // 示例：按名字查询
                    dataGrid.ItemsSource = queryResult;
                }

                if (BtnComboBoxIndex == 1)
                {
                    var queryResult = dataSource.Where(item => item.ProductNumber.Contains(SelectSea)).ToList(); // 示例：按名字查询
                    dataGrid.ItemsSource = queryResult;
                }

                if (BtnComboBoxIndex == 2)
                {
                    var queryResult = dataSource.Where(item => item.ProductType.Contains(SelectSea)).ToList(); // 示例：按名字查询
                    dataGrid.ItemsSource = queryResult;
                }

                if (BtnComboBoxIndex == 3)
                {
                    var queryResult = dataSource.Where(item => item.ProductTuhao.Contains(SelectSea)).ToList(); // 示例：按名字查询
                    dataGrid.ItemsSource = queryResult;
                }

            }
            else
            {
                dataGrid.ItemsSource = dataSource;
            }
            
            
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            //删除..
            // 获取当前选中的项
            var selectedItem = dataGrid.SelectedItem;

            // 如果你需要操作行数据，可以将选中的项转换为适当的类型
            if (selectedItem != null && selectedItem is Table_Product)
            {
                Table_Product selectedData = (Table_Product)selectedItem;
                // 现在你可以使用selectedData来访问行数据
                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.naiya_product.Find(selectedData.Id);

                    if (person != null)
                    {
                        //注意这里不是比较，是赋值了..
                        context.naiya_product.Remove(person);
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
