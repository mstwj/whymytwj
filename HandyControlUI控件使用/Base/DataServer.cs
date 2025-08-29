using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyControlUI控件使用.Models;

namespace HandyControlUI控件使用.Base
{
    //DataServerd -- 通过这个类，可以得到数据库里面的数据 。。
    //数据库 -- 转 -- MODEL...
    public class DataServer
    {
        SqlServerAccess sqlServerAccess = new SqlServerAccess();

        public List<StudioModel> GetStudio()
        {
            List<StudioModel> studioModels = new List<StudioModel>();
            // 获取点位信息
            // 获取设备信息
            var d_info2 = sqlServerAccess.GetDevices();
            var d_info = sqlServerAccess.GetStudio();
            foreach (var item in d_info.AsEnumerable())
            {
                StudioModel studioModel  = new StudioModel();
                studioModel.StudioId = item.Field<int>("Id");
                studioModel.StudioName = item.Field<string>("Name");
                studioModel.StudioAge = item.Field<int>("Age");
                studioModel.StudioSex = item.Field<string>("Sex");
                studioModel.StudioGrade = item.Field<string>("Grade");
                studioModel.StudioNumber = item.Field<int>("Number");
                studioModel.StudioDescription = item.Field<string>("Description");

                studioModels.Add(studioModel);



                // 获取点位信息
                /*
                var v_info = sqlServerAccess.GetMonitorValues(item.Field<string>("d_id"));
                if (v_info != null && v_info.AsEnumerable().Count() > 0)
                {
                    List<MonitorValueModel> vList = (from q in v_info.AsEnumerable()
                                                     select new MonitorValueModel
                                                     {
                                                         ValueName = q.Field<string>("tag_name"),
                                                         Address = q.Field<string>("address"),
                                                         DataType = q.Field<string>("data_type"),
                                                         Unit = q.Field<string>("unit")
                                                     }).ToList();
                    deviceModel.MonitorValueList = new System.Collections.ObjectModel.ObservableCollection<MonitorValueModel>(vList);
                }

                deviceModels.Add(deviceModel);
                */
            }

            return studioModels;
        }
    }
}
