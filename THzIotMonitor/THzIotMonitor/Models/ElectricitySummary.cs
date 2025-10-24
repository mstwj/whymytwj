using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace THzIotPlatform.Models
{
    /// <summary>
    /// 电力监控仪表盘汇总数据模型
    /// </summary>
    public class ElectricitySummary
    {
        [JsonProperty("chartData")]
        public ChartData ChartData { get; set; } = new ChartData();

        [JsonProperty("phaseDatas")]
        public List<PhaseData> PhaseDatas { get; set; } = new List<PhaseData>();

        [JsonProperty("faultDetails")]
        public List<FaultDetail> FaultDetails { get; set; } = new List<FaultDetail>();

        [JsonProperty("areaElectricities")]
        public List<AreaElectricity> AreaElectricities { get; set; } = new List<AreaElectricity>();

        [JsonProperty("topElectricityItems")]
        public List<TopElectricityItem> TopElectricityItems { get; set; } = new List<TopElectricityItem>();

        [JsonProperty("todayElectricity")]
        public int TodayElectricity { get; set; }

        [JsonProperty("monthlyElectricity")]
        public int MonthlyElectricity { get; set; }

        [JsonProperty("activePower")]
        public decimal ActivePower { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("error")]
        public string? Error { get; set; } // 用于传递错误信息
    }

    /// <summary>
    /// 图表数据模型
    /// </summary>
    public class ChartData
    {
        [JsonProperty("dates")]
        public List<string> Dates { get; set; } = new List<string>();

        [JsonProperty("lastMonthValues")]
        public List<decimal> LastMonthValues { get; set; } = new List<decimal>();

        [JsonProperty("currentMonthValues")]
        public List<decimal> CurrentMonthValues { get; set; } = new List<decimal>();
    }

    /// <summary>
    /// 三相电力参数模型
    /// </summary>
    public class PhaseData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("unit")]
        public string Unit { get; set; } = string.Empty;

        [JsonProperty("value")]
        public decimal Value { get; set; }
    }

    /// <summary>
    /// 故障详情模型
    /// </summary>
    public class FaultDetail
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("deviceGroup")]
        public string DeviceGroup { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// 区域用电量模型
    /// </summary>
    public class AreaElectricity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("areaName")]
        public string AreaName { get; set; } = string.Empty;

        [JsonProperty("electricity")]
        public int Electricity { get; set; }
    }

    /// <summary>
    /// 主要用电项模型
    /// </summary>
    public class TopElectricityItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("electricity")]
        public int Electricity { get; set; }
    }
}
