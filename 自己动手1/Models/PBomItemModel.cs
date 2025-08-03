using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 自己动手1.Base;

namespace 自己动手1.Models
{
    public class PBomItemModel:NotifyBase
    {
        //通过这个属性来控制 页面的打开和收缩..
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }
        public int Index { get; set; }

        public string PName { get; set; }
        public string PNum { get; set; }
        public string Description { get; set; }

        public ObservableCollection<string> PartList { get; set; }

        public Command<PBomItemModel> ExpandedCommand { get; set; }
        public PBomItemModel() 
        {
            //你去看看源代码 。。 BIND 什么都没有，就是绑定自己..
            //bind 什么都不写，表示什么，BIND . 就是BIND PATH=点..
            //表示什么，当前数据源对象..
            ExpandedCommand = new Command<PBomItemModel>(model =>
            {
                //如果设置为TRUE就 展开显示..
                //如果设置为FALSE就 是缩显示..
                //下面这个写发，就是点击的转方向..
                model.IsExpanded = !model.IsExpanded;
            })
            {
                DoCanExecute = new Func<object,bool>(DoCanE)
            };

            PartList = new ObservableCollection<string>();
            //监听集合的时间..
            PartList.CollectionChanged += (se, ve) =>
            {
                //去执行对应的检查..--只能这样..
                //这里可以理解为更新界面，或者界面回调刷新...(手动的..)
                ExpandedCommand.RasieCanExecuteChanged();
            };

            for (int i = 0; i < 5; i++)
            {
                PartList.Add("");
            }
        }

        private bool DoCanE(object obj)
        {
            return PartList.Count > 0;
        }

    }
}
