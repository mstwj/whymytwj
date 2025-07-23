using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Tailuopai.Base
{    
    public class LightItemModel
    {
        public int ItemType { get; set; }
        public string Header { get; set; }
        public bool IsOpen { get; set; }
        public string Describe { get; set; }
        public string Value1 { get; set; }
        public double Value2 { get; set; }
        public ICommand EditButtonCommand { get; set; }
        public object mySelf { get; set; } = null;

        //已经被选中..
        public bool IsChecked { get; set; } = false;

        public LightItemModel Clone()
        {
            return (LightItemModel)this.MemberwiseClone();
        }
    }

    public class LightItemModelThress
    {
        // 图片地址.
        public string ImageAddress { get; set; } = string.Empty;
        public string Describe { get; set; } //说明
        public bool Straight { get; set; } //正反..        
        public string ImageBack { get; set; } //图片背面.
        public double Value2 { get; set; } //保留                
    }

    public class MyData
    {
        public bool data1 { get; set; }
        public float data2 { get; set; }
        public int data3 { get; set; }
        public string data4 { get; set; } = string.Empty;
        public string presets1 { get; set; } = string.Empty;
        public string presets2 { get; set; } = string.Empty;
        public string presets3 { get; set; } = string.Empty;
        public string presets4 { get; set; } = string.Empty;
        public string presets5 { get; set; } = string.Empty;
        public string presets6 { get; set; } = string.Empty;
        public string presets7 { get; set; } = string.Empty;
        public string presets8 { get; set; } = string.Empty;
        public string presets9 { get; set; } = string.Empty;


    }
}
