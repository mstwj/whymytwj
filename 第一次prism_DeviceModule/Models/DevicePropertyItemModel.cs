using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 第一次prism_DeviceModule.Models
{
    public class DevicePropertyItemModel : BindableBase
    {
        public int PropId { get; set; }
        public string PropName { get; set; }

        // 如果属性值的变化需要体现在界面上的时候，这个属性需要通知
        private string _propValue;
        public string PropValue
        {
            get { return _propValue; }
            set { SetProperty(ref _propValue, value); }
        }
    }
}
