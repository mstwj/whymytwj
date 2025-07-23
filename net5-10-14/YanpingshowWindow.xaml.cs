using net5_10_14.Base;
using net5_10_14.Table;
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

namespace net5_10_14
{
    /// <summary>
    /// YanpingshowWindow.xaml 的交互逻辑
    /// </summary>
    public partial class YanpingshowWindow : Window
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public YanpingshowWindow()
        {
            InitializeComponent();
            //刷新
            UpdateDataSource();

            InitializeShow();


        }

        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

            Action action = delegate
            {

                //这里是提交后的值...                
                var editedPerson = e.Row.Item as newproinfo;
                if (editedPerson == null) return;

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    editedPerson = e.Row.Item as newproinfo;
                }

                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.newproinfo.Find(editedPerson.Id);

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


        void UpdateDataSource()
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    dataGrid.ItemsSource = null;
                    var bianbi = context.newproinfo.ToList();
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
            newproinfo test = new newproinfo();
            PropertyInfo[] properties = test.GetType().GetProperties();
            int k = 0;
            int i = 0;

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "Id") { continue; }

                DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
                dataGridTextColumn.Header = Tools.FindDaseNameToChinese("newproinfo", property.Name);
                dataGridTextColumn.Binding = new Binding($"{property.Name}");
                dataGridTextColumn.Width = 150;
                dataGrid.Columns.Add(dataGridTextColumn);               
            }
            return;
        }
    }
}
