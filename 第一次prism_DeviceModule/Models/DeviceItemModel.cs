using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 第一次prism_DeviceModule.Models
{
    public class DeviceItemModel 
    {
        public string Header { get; set; }

        public int DeviceId { get; set; }
        public string Image { get; set; }

        //public LightState LightType { get; set; } = LightState.None;

        public List<DevicePropertyItemModel> Properties { get; set; } =
            new List<DevicePropertyItemModel>();

        public List<EventModel> EventList { get; set; } =
            new List<EventModel>();
    }
}
