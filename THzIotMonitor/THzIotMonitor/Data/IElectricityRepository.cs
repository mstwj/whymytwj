using THzIotPlatform.Models;

namespace THzIotPlatform.Data
{
    /// <summary>
    /// 电力数据访问接口
    /// </summary>
    public interface IElectricityRepository
    {
        /// <summary>
        /// 获取仪表盘汇总数据
        /// </summary>
        /// <returns>汇总数据模型</returns>
        Task<ElectricitySummary> GetDashboardSummaryAsync();

        /// <summary>
        /// 获取指定时间段的用电趋势数据
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>图表数据</returns>
        Task<ChartData> GetElectricityTrendAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 获取最新的三相电力参数
        /// </summary>
        /// <returns>三相电力参数列表</returns>
        Task<List<PhaseData>> GetLatestPhaseDataAsync();

        /// <summary>
        /// 获取指定数量的最新故障记录
        /// </summary>
        /// <param name="count">记录数量</param>
        /// <returns>故障记录列表</returns>
        Task<List<FaultDetail>> GetLatestFaultsAsync(int count);

        /// <summary>
        /// 获取区域用电统计
        /// </summary>
        /// <returns>区域用电列表</returns>
        Task<List<AreaElectricity>> GetAreaConsumptionsAsync();

        /// <summary>
        /// 获取主要用电项排名
        /// </summary>
        /// <param name="topCount">获取前N项</param>
        /// <returns>用电项排名列表</returns>
        Task<List<TopElectricityItem>> GetTopElectricityItemsAsync(int topCount);
    }
}
