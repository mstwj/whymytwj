using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLC自己动手2.Models;

namespace PLC自己动手2.ViewModels
{
    public class MonitorViewModel
    {
        public List<LabelModel> RunLabels { get; set; } = new List<LabelModel>();
        public List<LabelModel> BaseLabels { get; set; } = new List<LabelModel>();


        public ObservableCollection<DataGridItemModel> DataList { get; set; } = new ObservableCollection<DataGridItemModel>();
        public MonitorViewModel()
        {
            //这里的意思是，对象的值变化，不会涉及到对象LIST 多少的变化..
            //这里的值是可以随便变化的，可是 LIST一开始是多少是顶死的，所以可以使用LIST,不使用OBSERVER也行.
            //所以上面这里显示对应的就是 ItemsControl -- ItemsControl...
            //DataList 就不一样了，DataList 里面的个数是变化的，所以这里必须是 ObservableCollection
            //DATALIST显示模板也不一样，使用的是 DataGrid
            RunLabels.Add(new LabelModel { Text = "当前状态", Value = "运行" });
            RunLabels.Add(new LabelModel { Text = "周运行时长", Value = "80h" });
            RunLabels.Add(new LabelModel { Text = "周关机时长", Value = "50h" });
            RunLabels.Add(new LabelModel { Text = "周故障时长", Value = "50h" });
            RunLabels.Add(new LabelModel { Text = "监控状态", Value = "良好" });

            BaseLabels.Add(new LabelModel { Text = "最大工作范围", Value = "1.44m" });
            BaseLabels.Add(new LabelModel { Text = "有效符合", Value = "运行" });
            BaseLabels.Add(new LabelModel { Text = "有效咒术", Value = "6J" });
            BaseLabels.Add(new LabelModel { Text = "春风得意这种毒", Value = "0.001cm" });
            BaseLabels.Add(new LabelModel { Text = "额定功率", Value = "2500w" });
            BaseLabels.Add(new LabelModel { Text = "偿债能力", Value = "5kg" });
            BaseLabels.Add(new LabelModel { Text = "J6轴最大速度", Value = "2.1M/S" });
            BaseLabels.Add(new LabelModel { Text = "电源打压", Value = "299-500v" });
            BaseLabels.Add(new LabelModel { Text = "经总", Value = "224kg" });

            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                DataList.Add(new DataGridItemModel 
                {
                    Name = "测试-" + i.ToString("00"),
                    Age = random.Next(18, 90),
                    Value = random.Next(30, 120).ToString()
                });
            }
        }
    }
}
