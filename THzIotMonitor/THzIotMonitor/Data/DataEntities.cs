using System.ComponentModel.DataAnnotations;

namespace THzIotPlatform.Data
{
    

    /// <summary>
    /// 三相电力参数记录实体
    /// </summary>
    public class PhaseRecord
    {
        public int Id { get; set; }

        [Required]
        public string Phase { get; set; } = string.Empty; // L1, L2, L3

        [Required]
        public string ParameterName { get; set; } = string.Empty; // 电压、电流、有功功率等

        [Required]
        public string Unit { get; set; } = string.Empty; // V, A, KW等

        public decimal Value { get; set; }

        public DateTime RecordTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 故障记录实体
    /// </summary>
    public class FaultRecord
    {
        public int Id { get; set; }

        [Required]
        public string FaultType { get; set; } = string.Empty; // 设备故障、电压异常等

        public DateTime FaultTime { get; set; } = DateTime.Now;

        [Required]
        public string DeviceGroup { get; set; } = string.Empty; // 设备组名称

        public string Description { get; set; } = string.Empty; // 故障描述

        public bool IsResolved { get; set; } = false; // 是否已解决
    }

    /// <summary>
    /// 区域用电记录实体
    /// </summary>
    public class AreaConsumption
    {
        public int Id { get; set; }

        [Required]
        public string AreaName { get; set; } = string.Empty; // 大区域名称

        public string SubAreaName { get; set; } = string.Empty; // 子区域名称

        public int Electricity { get; set; } // 用电量（kWh）

        public DateTime RecordDate { get; set; } = DateTime.Now; // 记录日期
    }
}
