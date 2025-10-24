using Microsoft.EntityFrameworkCore;
using THzIotPlatform.Models;

namespace THzIotPlatform.Data
{
    
    /// <summary>
    /// 电力数据访问实现
    /// </summary>
    public class ElectricityRepository : IElectricityRepository
    {
        private readonly ElectricityDbContext _context;

        public ElectricityRepository(ElectricityDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取仪表盘汇总数据
        /// </summary>
        public async Task<ElectricitySummary> GetDashboardSummaryAsync()
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            // 获取今日用电量
            var todayElectricity = await CalculateTodayElectricityAsync();

            // 获取本月用电量
            var monthlyElectricity = await CalculateMonthlyElectricityAsync();

            // 获取当前有功功率
            var activePower = await GetCurrentActivePowerAsync();

            // 构建汇总数据
            return new ElectricitySummary
            {
                TodayElectricity = todayElectricity,
                MonthlyElectricity = monthlyElectricity,
                ActivePower = activePower,
                Status = "正常运行",
                ChartData = await GetElectricityTrendAsync(firstDayOfMonth.AddMonths(-1), today),
                PhaseDatas = await GetLatestPhaseDataAsync(),
                FaultDetails = await GetLatestFaultsAsync(10),
                AreaElectricities = await GetAreaConsumptionsAsync(),
                TopElectricityItems = await GetTopElectricityItemsAsync(5)
            };
        }

        /// <summary>
        /// 获取指定时间段的用电趋势数据
        /// </summary>
        public async Task<ChartData> GetElectricityTrendAsync(DateTime startDate, DateTime endDate)
        {
            // 从数据库查询历史数据
            var records = await _context.AreaConsumptions
                .Where(a => a.RecordDate >= startDate && a.RecordDate <= endDate)
                .GroupBy(a => a.RecordDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Total = g.Sum(a => a.Electricity)
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            // 构建上月和本月数据
            var lastMonthStart = startDate;
            var lastMonthEnd = startDate.AddMonths(1).AddDays(-1);
            var currentMonthStart = lastMonthEnd.AddDays(1);

            var chartData = new ChartData
            {
                Dates = records.Select(r => r.Date.ToString("MM-dd")).ToList(),
                LastMonthValues = records
                    .Where(r => r.Date >= lastMonthStart && r.Date <= lastMonthEnd)
                    .Select(r => (decimal)r.Total)
                    .ToList(),
                CurrentMonthValues = records
                    .Where(r => r.Date >= currentMonthStart)
                    .Select(r => (decimal)r.Total)
                    .ToList()
            };

            return chartData;
        }

        /// <summary>
        /// 获取最新的三相电力参数
        /// </summary>
        public async Task<List<PhaseData>> GetLatestPhaseDataAsync()
        {
            // 获取最近的三相数据记录时间
            var latestTime = await _context.PhaseRecords
                .MaxAsync(p => p.RecordTime);

            // 获取该时间点的所有三相数据
            var phaseRecords = await _context.PhaseRecords
                .Where(p => p.RecordTime == latestTime)
                .ToListAsync();

            // 转换为前端需要的模型
            return phaseRecords.Select(p => new PhaseData
            {
                Id = p.Id,
                Name = $"{p.Phase} {p.ParameterName}",
                Unit = p.Unit,
                Value = p.Value
            }).ToList();
        }

        /// <summary>
        /// 获取指定数量的最新故障记录
        /// </summary>
        public async Task<List<FaultDetail>> GetLatestFaultsAsync(int count)
        {
            var faults = await _context.FaultRecords
                .OrderByDescending(f => f.FaultTime)
                .Take(count)
                .ToListAsync();

            return faults.Select(f => new FaultDetail
            {
                Id = f.Id,
                Type = f.FaultType,
                Time = f.FaultTime,
                DeviceGroup = f.DeviceGroup,
                Description = f.Description
            }).ToList();
        }

        /// <summary>
        /// 获取区域用电统计
        /// </summary>
        public async Task<List<AreaElectricity>> GetAreaConsumptionsAsync()
        {
            var today = DateTime.Today;

            var areaData = await _context.AreaConsumptions
                .Where(a => a.RecordDate >= today.AddDays(-30)) // 近30天
                .GroupBy(a => a.AreaName)
                .Select(g => new AreaElectricity
                {
                    AreaName = g.Key,
                    Electricity = g.Sum(a => a.Electricity)
                })
                .OrderByDescending(a => a.Electricity)
                .ToListAsync();

            return areaData;
        }

        /// <summary>
        /// 获取主要用电项排名
        /// </summary>
        public async Task<List<TopElectricityItem>> GetTopElectricityItemsAsync(int topCount)
        {
            var today = DateTime.Today;

            var topItems = await _context.AreaConsumptions
                .Where(a => a.RecordDate >= today.AddMonths(-1)) // 近一个月
                .GroupBy(a => a.SubAreaName)
                .Select(g => new TopElectricityItem
                {
                    Name = g.Key,
                    Electricity = g.Sum(a => a.Electricity)
                })
                .OrderByDescending(t => t.Electricity)
                .Take(topCount)
                .ToListAsync();

            return topItems;
        }

        #region 私有辅助方法
        /// <summary>
        /// 计算今日用电量
        /// </summary>
        private async Task<int> CalculateTodayElectricityAsync()
        {
            var today = DateTime.Today;
            return await _context.AreaConsumptions
                .Where(a => a.RecordDate >= today)
                .SumAsync(a => a.Electricity);
        }

        /// <summary>
        /// 计算本月用电量
        /// </summary>
        private async Task<int> CalculateMonthlyElectricityAsync()
        {
            var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            return await _context.AreaConsumptions
                .Where(a => a.RecordDate >= firstDayOfMonth)
                .SumAsync(a => a.Electricity);
        }

        /// <summary>
        /// 获取当前有功功率
        /// </summary>
        private async Task<decimal> GetCurrentActivePowerAsync()
        {
            var latestTime = await _context.PhaseRecords
                .Where(p => p.ParameterName == "有功功率")
                .MaxAsync(p => p.RecordTime);

            return await _context.PhaseRecords
                .Where(p => p.RecordTime == latestTime && p.ParameterName == "有功功率")
                .SumAsync(p => p.Value);
        }
        #endregion
    }
}
