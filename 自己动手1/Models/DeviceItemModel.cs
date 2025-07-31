using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 自己动手1.Base;
using 自己动手1.Control;

namespace 自己动手1.Models
{
    public class DevicePropertyItemModel:NotifyBase
    {
        //如果属性值的变化需要体现在页面的话...
        //下面这样写是不对的,,,因为这个属性需要通知..
        public string PropName { get; set; }

        private string _propValue;
        public string PropValue 
        {
            get { return _propValue; }
            set { SetProperty(ref _propValue, value); }
                
        }
    }

    public class DeviceItemModel
    {
                      
        public string Image { get; set; }
        public string Title { get; set; }

        public LightState LightType { get; set; } = LightState.None;

        public List<DevicePropertyItemModel> Properties { get; set; }

        public DeviceItemModel()
        {
            Properties = new List<DevicePropertyItemModel>();
        }


    }
}
