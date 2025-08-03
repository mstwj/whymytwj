using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 自己动手1.Base;
using 自己动手1.Models;

namespace 自己动手1.ViewModels
{
    public class PBomViewModel
    {
        public ObservableCollection<PBomItemModel> PBOMList { get; set; }

        public PBomViewModel()
        {
            PBOMList = new ObservableCollection<PBomItemModel>();
            for (int i = 0;i < 5; i++)
            {
                PBOMList.Add(new PBomItemModel()
                {
                    Index = 100+i,
                    PNum = "P123kkse",
                    PName = "twj"+i.ToString()
                });
            }

            NewPBOMCommand = new Command<object>(DoNewCommand);
        }

        public Command<object> NewPBOMCommand { get; set; }

        private void DoNewCommand()
        {
            //不能 NEW PBOMEIDTWIN -- 怎么办，要解耦设计..
            //怎么办，中间容器，使用IOC...
            //比如，我现在在VM里面 我要一个VIEW ，我不能 NEW VIEW。。
            //怎么办 要一个低三方，VM->第三方->view 要这样才OK的..
            //第3方 管理是什么 -- 行为，就是说 第3方，里面可以是VEW 也可以是VIEWMODEL..
            PBomItemModel pBomItemModel = new PBomItemModel();
            pBomItemModel.PName = "Hello";
            pBomItemModel.PartList.Clear();
            //ActionManager.Execute("PBOM", pBomItemModel);

            if (ActionManager.ExecuteAndResult("PBOM-F", pBomItemModel))
                PBOMList.Add(pBomItemModel);

            //还有个问题，如果点取消呢?就不能ADD了..


            //很多，能理解就理解吧，反正跳来跳去的..
        }
    }
}
