using Microsoft.AspNetCore.Mvc;
using THzIotPlatform.Data;
using THzIotPlatform.Models;
using System.Text.Json.Serialization;

namespace THzIotPlatform.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetElectricitySummary()
        {
            try
            {
                // 生成24小时时间标签（00:00到23:00）
                var hourlyLabels = Enumerable.Range(0, 24).Select(h => $"{h:D2}:00").ToList();

                // 生成本周日期标签（周一到周日）
                var weekdays = new List<string> { "周一", "周二", "周三", "周四", "周五", "周六", "周日" };

                var result = new
                {
                    ChartData = new
                    {
                        Dates = new List<string> { "05-01", "05-04", "05-07", "05-10", "05-13",
                          "05-16", "05-19", "05-22", "05-25", "05-28" },
                        LastMonthValues = new List<decimal> { 100, 115, 155, 165, 145, 155, 85, 60, 70, 70 },
                        CurrentMonthValues = new List<decimal> { 80, 110, 118, 110, 85, 60, 100, 84, 84, 84 }
                    },
                    deviceStatus = new
                    {
                        running = 6,   // 运行中的设备数量
                        stopped = 2,   // 停机的设备数量
                        fault = 1      // 故障的设备数量
                    },
                    powerData = new
                    {
                        capacitor = new
                        {
                            timestamps = new List<string> { "05-01", "05-04", "05-07", "05-10", "05-13", "05-16", "05-19" },
                            activePower = new List<double> { 134.8, 142.5, 150.2, 158.7, 165.3, 175.9, 186.6 },
                            reactivePower = new List<double> { 45.8, 48.2, 50.5, 52.1, 53.8, 55.2, 64 },
                            currentValue = 186.6,
                            averageValue = 53.64
                        },
                        energy = new
                        {
                            timestamps = new List<string> { "05-01", "05-04", "05-07", "05-10", "05-13", "05-16", "05-19" },
                            activePower = new List<double> { 120, 110, 123.93, 135, 142, 150, 161.43 },
                            reactivePower = new List<double> { 30, 10, -32.9, -15, 5, 15, 107.5 },
                            currentValue = 161.43,
                            averageValue = 22.73
                        }
                    },
                    // 新增：三相实时电流数据（最近24小时）
                    threePhaseCurrentData = new
                    {
                        timestamps = hourlyLabels,
                        phases = new
                        {
                            L1 = new List<double> { 112.5, 110.3, 108.7, 105.2, 103.5, 102.1, 105.8, 110.4, 118.7, 125.3, 128.6, 130.2, 132.5, 131.8, 129.6, 127.3, 125.8, 122.4, 118.5, 115.2, 113.7, 112.8, 111.5, 110.7 },
                            L2 = new List<double> { 108.2, 106.5, 105.3, 102.8, 101.2, 100.5, 103.2, 107.8, 115.3, 121.5, 124.8, 126.4, 128.7, 127.6, 125.3, 123.1, 121.5, 118.2, 114.3, 111.5, 110.2, 109.3, 107.8, 106.5 },
                            L3 = new List<double> { 115.3, 113.2, 111.5, 108.7, 106.3, 105.1, 108.5, 113.2, 120.7, 127.5, 130.8, 132.4, 135.6, 134.8, 132.5, 130.2, 128.7, 125.3, 121.4, 118.2, 116.7, 115.8, 114.5, 113.2 }
                        }
                    },
                    // 新增：本周耗电量数据
                    weeklyConsumptionData = new
                    {
                        days = weekdays,
                        values = new List<double> { 10820.5, 6245.3, 6780.2, 6520.7, 7125.8, 5980.3, 5420.6 }
                    },
                    PhaseDatas = new List<PhaseData>
            {
                new PhaseData { Name = "L1电压", Unit = "V", Value = 245.3m },
                new PhaseData { Name = "L1总柜一次电流", Unit = "A", Value = 24.2m },
                new PhaseData { Name = "L1功率因数", Unit = "", Value = 0.988m },
                new PhaseData { Name = "L2电压", Unit = "V", Value = 242.1m },
                new PhaseData { Name = "L2总柜一次电流", Unit = "A", Value = 22.8m },
                new PhaseData { Name = "L2功率因数", Unit = "", Value = 0.989m },
                new PhaseData { Name = "L3电压", Unit = "V", Value = 244.8m },
                new PhaseData { Name = "L3总柜一次电流", Unit = "A", Value = 23.5m },
                new PhaseData { Name = "L3功率因数", Unit = "", Value = 0.987m }
            },
                    FaultDetails = new List<FaultDetail>
            {
                new FaultDetail
                {
                    Id = 1,
                    Type = "设备故障",
                    Time = DateTime.ParseExact("2023-05-14T12:12:34", "yyyy-MM-ddTHH:mm:ss", null),
                    DeviceGroup = "21号空压机设备组",
                    Description = "智控10分钟之后进入关闭状态"
                },
                new FaultDetail
                {
                    Id = 2,
                    Type = "电压异常",
                    Time = DateTime.ParseExact("2023-05-15T08:45:12", "yyyy-MM-ddTHH:mm:ss", null),
                    DeviceGroup = "东区配电组",
                    Description = "电压短暂波动，已自动恢复"
                },
                new FaultDetail
                {
                    Id = 3,
                    Type = "通讯中断",
                    Time = DateTime.ParseExact("2023-05-16T15:30:22", "yyyy-MM-ddTHH:mm:ss", null),
                    DeviceGroup = "西区监控组",
                    Description = "网络故障导致通讯中断，已修复"
                }
            },
                    AreaElectricities = new List<AreaElectricity>
            {
                new AreaElectricity { AreaName = "研发车间", Electricity = 6926 },
                new AreaElectricity { AreaName = "中试车间", Electricity = 5511 },
                new AreaElectricity { AreaName = "生产一区", Electricity = 4825 },
                new AreaElectricity { AreaName = "生产二区", Electricity = 3987 },
                new AreaElectricity { AreaName = "办公区域", Electricity = 2156 }
            },
                    TopElectricityItems = new List<TopElectricityItem>
            {
                new TopElectricityItem { Name = "A馆东区", Electricity = 24528 },
                new TopElectricityItem { Name = "A馆西区", Electricity = 21876 },
                new TopElectricityItem { Name = "B馆南区", Electricity = 19654 },
                new TopElectricityItem { Name = "B馆北区", Electricity = 17845 },
                new TopElectricityItem { Name = "C馆", Electricity = 15632 }
            },
                    TodayElectricity = 6286,
                    MonthlyElectricity = 245286,
                    ActivePower = 85.33m,
                    powerFactor = 0.9859,
                    ambientTemperature = 16.8,
                    totalActivePower = 458.3,
                    Status = "正常运行",//tuane运行状态
                    tuaneCurrent = 7.6,//tuane电流
                    tuaneVoltage = 226.1,//tuane电压
                    tuaneTotalActivePower = 2.07,//tuane总有功功率
                    tuaneTotalReactivePower = 39.7,//tuane总无功功率
                    tuaneTotalActiveEnergy = 680012//tuane总有功电量
                };

                Response.ContentType = "application/json; charset=utf-8";
                return Json(result);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    ChartData = new
                    {
                        Dates = new List<string>(),
                        LastMonthValues = new List<decimal>(),
                        CurrentMonthValues = new List<decimal>()
                    },
                    Error = ex.Message
                });
            }
        }
    }
 }

