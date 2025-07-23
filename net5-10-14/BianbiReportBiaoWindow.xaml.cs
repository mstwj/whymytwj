using net5_10_14.Base;
using net5_10_14.Table.bbsy;
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
    /// BianbiReportBiaoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BianbiReportBiaoWindow : Window
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public experimentstandard_bianbishiyan test { get; set; } = new experimentstandard_bianbishiyan();

        private List<TextBox> TextBoxList = new List<TextBox>();

        public BianbiReportBiaoWindow()
        {
            InitializeComponent();
            //刷新
            UpdateDataSource();
            //设置显示
            InitializeShow();
            //设置添加.
            InitializeAdd();
        }


        void UpdateDataSource()
        {
            using (var context = new MyDbContext())
            {
                try
                {
                    dataGrid.ItemsSource = null;
                    var bianbiv = context.experimentstandard_bianbishiyan.ToList();
                    // 设置DataGrid的数据源
                    dataGrid.ItemsSource = bianbiv;
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
            PropertyInfo[] properties = test.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "Id") { continue; }

                DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
                dataGridTextColumn.Header = Tools.FindDaseNameToChinese("experimentstandard_bianbishiyan", property.Name);
                dataGridTextColumn.Binding = new Binding($"{property.Name}");
                dataGridTextColumn.Width = 150;
                dataGrid.Columns.Add(dataGridTextColumn);
            }
            return;
        }

        void InitializeAdd()
        {
            PropertyInfo[] properties = test.GetType().GetProperties();
            int i = 0, k = 0;
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "Id") continue;

                TextBlock textBlock = CreateTextBlock(property.Name);
                TextBox textBox = null;
                ComboBox comboBox = null;
                switch (property.Name)
                {
                    case "Ccreateuserdate": textBox = CreateTextBox(property.Name, true); break;
                    case "Cailiao": comboBox = CreateComboBox(property.Name); break;
                    default: textBox = CreateTextBox(property.Name); break;
                }

                Grid.SetRow(textBlock, i);
                Grid.SetColumn(textBlock, k);

                if (comboBox != null)
                {
                    Grid.SetRow(comboBox, i);
                    Grid.SetColumn(comboBox, k + 1);
                    GridTJ.Children.Add(comboBox);
                }
                else
                {
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, k + 1);
                    GridTJ.Children.Add(textBox);
                    TextBoxList.Add(textBox);
                }

                GridTJ.Children.Add(textBlock);

                i++;

            }
            return;
        }

        TextBlock CreateTextBlock(string propertyname)
        {
            // 创建一个文本框并将其放在第二行第一列
            TextBlock textBlock = new TextBlock();
            textBlock.Text = Tools.FindDaseNameToChinese("experimentstandard_bianbishiyan", propertyname);
            textBlock.FontSize = 17;
            textBlock.HorizontalAlignment = HorizontalAlignment.Right;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Margin = new Thickness(0, 0, 10, 0);
            return textBlock;
        }

        TextBox CreateTextBox(string propertyname, bool IsTimer = false)
        {
            TextBox textBox = new TextBox();
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.Margin = new Thickness(10, 0, 0, 0);
            Binding binding = new Binding();
            binding.Path = new PropertyPath(propertyname);
            binding.Source = test;
            binding.ValidatesOnDataErrors = true;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            textBox.SetBinding(TextBox.TextProperty, binding);
            textBox.Width = 170;
            textBox.FontSize = 17;
            if (IsTimer)
            {
                textBox.IsReadOnly = true;
                textBox.Text = DateTime.Now.ToString();
            }
            return textBox;
        }

        ComboBox CreateComboBox(string propertyname)
        {
            // 创建一个文本框并将其放在第二行第一列
            ComboBox textBox = new ComboBox();
            textBox.Items.Add("铜");
            textBox.Items.Add("吕");
            textBox.Margin = new Thickness(10, 0, 0, 0);
            textBox.Width = 100;
            Binding binding = new Binding();
            binding.Path = new PropertyPath(propertyname);
            binding.ValidatesOnDataErrors = true;
            binding.Source = test;
            textBox.SetBinding(ComboBox.TextProperty, binding);
            textBox.FontSize = 15;
            textBox.IsEditable = true;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            return textBox;
        }

        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

            Action action = delegate
            {

                //这里是提交后的值...    C#中使用as关键字将对象转换为指定类型_c# --  成功true 失败 false..
                var editedPerson = e.Row.Item as experimentstandard_bianbishiyan;
                if (editedPerson == null) return;

                if (e.EditAction == DataGridEditAction.Commit)
                {
                    editedPerson = e.Row.Item as experimentstandard_bianbishiyan;
                }

                // 如果数据不符合要求，可以取消结束编辑
                // e.Cancel = true;

                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.experimentstandard_bianbishiyan.Find(editedPerson.Id);

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyDbContext())
            {
                test.Id = 0;//这里还必须这样搞...(iD设置为0表示，我不提供ID 数据库自己去管理ID..)
                //第1次验证...
                foreach (var item in TextBoxList)
                {
                    if (Validation.GetHasError(item))
                    {
                        MessageBox.Show("无法提交,输入有误");
                        return;
                    }
                }

                //第2次验证...
                var myitem = Tools.SafeScanPro("experimentstandard_bianbishiyan", test);
                if (myitem.Item1 == false)
                {
                    MessageBox.Show(myitem.Item2);
                    return;
                }
                try
                {
                    context.experimentstandard_bianbishiyan.Add(test);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        // 数据添加成功
                        MessageBox.Show("添加成功");
                        UpdateDataSource();
                    }
                    else
                    {
                        MessageBox.Show("添加失败");
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ex.StackTrace;
                    logger.Error(message);
                    MessageBox.Show("严重错误数据库报错,请检查数据库是否正常!");
                }
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            // 获取当前选中的项
            var selectedItem = dataGrid.SelectedItem;

            // 如果你需要操作行数据，可以将选中的项转换为适当的类型
            if (selectedItem != null && selectedItem is experimentstandard_bianbishiyan)
            {
                experimentstandard_bianbishiyan selectedData = (experimentstandard_bianbishiyan)selectedItem;
                // 现在你可以使用selectedData来访问行数据
                // 更新数据库中的数据
                using (var context = new MyDbContext())
                {
                    // 更新已有数据
                    var person = context.experimentstandard_bianbishiyan.Find(selectedData.Id);

                    if (person != null)
                    {
                        //注意这里不是比较，是赋值了..
                        context.experimentstandard_bianbishiyan.Remove(person);
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
