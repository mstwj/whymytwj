using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotUserEnPhone.Models
{
    //为什么要这个类，这就类就是表示，第一行里面的4个图标啥的..

    public class FunctionItemModel
    {
        public string ButtonIcon { get; set; }
        public string Text {  get; set; }
        public string ViewRoute { get; set; }

        //打开动...
        public Command<FunctionItemModel> OpenPage { get; set; }    
    }
}
