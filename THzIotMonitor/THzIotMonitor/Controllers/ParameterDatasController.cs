using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using THzIotPlatform.Models;
using THzIotPlatform.Data;

namespace THzIotPlatform.Controllers
{
    public class ParameterDatasController : Controller
    {
        // 数据库上下文（通过依赖注入获取）
        private readonly ApplicationDbContext _context;
        // 日志组件（用于记录操作日志和异常）
        private readonly ILogger<ParameterDatasController> _logger;

        public ParameterDatasController(ApplicationDbContext context, ILogger<ParameterDatasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ParameterDatas
        // GET: ParameterDatas
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string[] selectedParameters)
        {

            string[] targetIds;
            if (selectedParameters == null || !selectedParameters.Any())
            {
                selectedParameters = new string[] { "106|澳門俾利喇購物廣場-智能电容-L2電壓|qmgL6v7CusB,107|澳門俾利喇購物廣場-智能电容-L2電壓|qmgL6v7CusB,108|澳門俾利喇購物廣場-智能电容-L2電壓|qmgL6v7CusB" };

                endDate = DateTime.Now;
                startDate = DateTime.Now.AddHours(-1);
            }
            

            int[] targetIntIds = Array.Empty<int>();
            ViewBag.DeviceGroupMap = _context.DeviceGroups.ToDictionary(g => g.Id, g => g.GroupName);
            ViewBag.DeviceMap = _context.Devices.ToDictionary(g => g.Id, g => g.DeviceName);

            //ViewBag.AllDeviceGroups= _context.DeviceGroups.ToDictionary(g => g.Id, g => g.GroupName);
            ViewBag.AllDeviceGroups = await _context.DeviceGroups
                                        .OrderBy(g => g.GroupName)
                                        .ToListAsync();

            // 1. 原有设备组加载逻辑保留
            ViewData["DeviceGroups"] = await _context.DeviceGroups
                .Include(g => g.Devices)
                .ThenInclude(d => d.Parameters)
                .ToListAsync();

            // 新增：首次加载且无筛选参数时，默认加载前10个参数的SN
            if (selectedParameters == null || !selectedParameters.Any())
            {
                // 使用ToArrayAsync()异步获取，并添加必要的命名空间
                selectedParameters = await _context.DeviceParameters
                    .Take(100) // 取前10个参数
                    .Select(p => $"{p.DeviceCode}_{p.AccessAddress}") // 按规则生成SN
                    .ToArrayAsync(); // 异步版本的ToArray()

                ViewData["SelectedParameters"] = selectedParameters; // 传递到视图回显
            }
            else
            {
                // 1. 获取原始复合字符串
                string original = selectedParameters[0]+ ",99999999|澳門俾利喇購物廣場-智能电容-L2電壓|qmgL6v7CusB";

                // 2. 按逗号分割为单个参数项（得到 ["D1|...", "D2|...", "D3|..."]）
                string[] paramItems = original.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // 3. 提取每个参数项的第一个标识（D1、D2、D3）
                string[] ids = paramItems
                    .Select(item =>
                    {
                        var parts = item.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        return parts.Length > 0 ? parts[0] : string.Empty;
                    })
                    .Where(id => !string.IsNullOrEmpty(id))
                    .ToArray();

                               

                List<int> intIdsList = new List<int>();
                foreach (string idStr in ids)
                {
                    // 尝试将字符串转换为 int
                    if (int.TryParse(idStr, out int id))
                    {
                        intIdsList.Add(id); // 转换成功则添加到列表
                    }
                    else
                    {
                        // 处理转换失败的情况（如日志记录）
                        Console.WriteLine($"无效的数字格式：{idStr}");
                    }
                }

                // 转换为 int 数组（最终结果）
                targetIntIds = intIdsList.ToArray();
            }




            var defaultEndDate = DateTime.Now;
            var defaultStartDate = defaultEndDate.AddDays(-1);
            ViewBag.StartTime = defaultStartDate.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.EndTime = defaultEndDate.ToString("yyyy-MM-ddTHH:mm");
            ViewData["startDate"] = startDate ?? defaultStartDate;
            ViewData["endDate"] = endDate ?? defaultEndDate;

            // 3. 数据查询（保留原有筛选）
            var query = _context.ParameterDatas.AsQueryable();
            if (startDate.HasValue) query = query.Where(d => d.Time >= startDate.Value);
            if (endDate.HasValue) query = query.Where(d => d.Time <= endDate.Value);
            if (selectedParameters != null && selectedParameters.Any())
                query = query.Where(d => targetIntIds.Contains(d.ParameterId));

            if (selectedParameters == null || !selectedParameters.Any())
            {
                // 使用ToArrayAsync()异步获取，并添加必要的命名空间
                selectedParameters = await _context.DeviceParameters
                    .Take(0) // 取前10个参数
                    .Select(p => $"{p.DeviceCode}_{p.AccessAddress}") // 按规则生成SN
                    .ToArrayAsync(); // 异步版本的ToArray()

                ViewData["SelectedParameters"] = selectedParameters; // 传递到视图回显
            }

            var parameterDatas = await query.ToListAsync();

            // 4. 按 SN 分组（保留原有逻辑）
            var groupedData = parameterDatas
                .GroupBy(d => d.SN ?? "UNKNOWN_SN")
                .ToDictionary(g => g.Key, g => g.ToList());

            // 5. 关键修复：获取分组后所有实际存在的 SN（而非仅筛选参数）
            var actualSns = groupedData.Keys.Where(sn => sn != "UNKNOWN_SN").ToArray();

            // 6. 准备视图模型（使用实际 SN 解析参数信息）
            var viewModel = new ParameterDataViewModel
            {
                GroupedDatas = groupedData,
                SelectedParameters = selectedParameters ?? Array.Empty<string>(),
                // 传入实际存在的 SN 解析参数信息，确保覆盖所有分组
                ParameterInfo = await GetParameterInfo(actualSns),

                DeviceMap = await _context.Devices
            .ToDictionaryAsync(d => d.DeviceCode, d => new DeviceInfo
            {
                DeviceName = d.DeviceName,
                GroupId = d.GroupId
            }),
                DeviceGroupMap = await _context.DeviceGroups
            .ToDictionaryAsync(g => g.Id.ToString(), g => g.GroupName)
            };

            // 7. 计算统计数据（保留原有逻辑）
            viewModel.Statistics = CalculateStatistics(parameterDatas, viewModel.ParameterInfo);

            return View(viewModel);
        }

        // 获取参数详细信息

        // 获取参数详细信息（批量查询优化版）
        // 修改GetParameterInfo方法中的参数查询部分
        private async Task<Dictionary<string, ParameterInfo>> GetParameterInfo(string[] sns)
        {
            if (sns == null || !sns.Any())
                return new Dictionary<string, ParameterInfo>();

            // 1. 解析所有SN为(devicecode, accessaddress)键值对
            var snPartsList = sns
                .Select(sn =>
                {
                    var parts = sn.Split('_');
                    return parts.Length == 2
                        ? new { DeviceCode = parts[0], AccessAddress = parts[1], SN = sn }
                        : null;
                })
                .Where(parts => parts != null)
                .ToList();

            if (!snPartsList.Any())
                return new Dictionary<string, ParameterInfo>();

            // 2. 批量查询设备（保留原有逻辑）
            var deviceCodes = snPartsList.Select(p => p.DeviceCode).Distinct().ToList();
            var devices = await _context.Devices
                .Include(d => d.DeviceGroup)
                .Where(d => deviceCodes.Contains(d.DeviceCode))
                .ToDictionaryAsync(d => d.DeviceCode);

            // 3. 修复查询：使用值元组和Contains替代Join
            // 创建值元组列表（EF Core可识别）
            var paramTuples = snPartsList
                .Select(p => (p.DeviceCode, p.AccessAddress)) // 使用值元组
                .ToList();

            // 构建可翻译的查询
            var parameters = new List<DeviceParameter>();
            foreach (var (deviceCode, accessAddress) in paramTuples)
            {
                // 每次查询单个组合（适合参数数量不多的情况）
                var param = await _context.DeviceParameters
                    .FirstOrDefaultAsync(p =>
                        p.DeviceCode == deviceCode &&
                        p.AccessAddress == accessAddress);

                if (param != null)
                    parameters.Add(param);
            }

            // 转换为按SN索引的字典
            var paramDict = parameters
                .ToDictionary(p => $"{p.DeviceCode}_{p.AccessAddress}");

            // 4. 组装结果（保留原有逻辑）
            var result = new Dictionary<string, ParameterInfo>();
            foreach (var snParts in snPartsList)
            {
                devices.TryGetValue(snParts.DeviceCode, out var device);
                paramDict.TryGetValue(snParts.SN, out var parameter);

                if (parameter != null)
                {
                    result[snParts.SN] = new ParameterInfo
                    {
                        DeviceGroupName = device?.DeviceGroup?.GroupName ?? "未知设备组",
                        DeviceName = device?.DeviceName ?? "未知设备",
                        ParameterName = parameter.ParameterName,
                        FullName = $"{device?.DeviceGroup?.GroupName ?? "未知设备组"}-{device?.DeviceName ?? "未知设备"}-{parameter.ParameterName}"
                    };
                }
            }

            return result;
        }



        // 计算统计数据
        private List<StatisticViewModel> CalculateStatistics(List<ParameterData> datas, Dictionary<string, ParameterInfo> parameterInfo)
        {
            return datas
                .Where(d => !string.IsNullOrEmpty(d.SN)) // 过滤掉SN为null或空的记录
                .GroupBy(d => d.SN!) // 使用!运算符告知编译器SN不为null
                .Select(g => new StatisticViewModel
                {
                    ParameterFullName = parameterInfo.ContainsKey(g.Key) ? parameterInfo[g.Key].FullName : g.Key,
                    MaxValue = g.Max(d => d.Value),
                    MinValue = g.Min(d => d.Value),
                    AverageValue = g.Average(d => d.Value)
                })
                .ToList();
        }

        // 获取用于图表的数据
        public async Task<IActionResult> GetChartData(string sn, DateTime startDate, DateTime endDate, string granularity)
        {
            var query = _context.ParameterDatas
                .Where(d => d.SN== sn && d.Time >= startDate && d.Time <= endDate)
                .OrderBy(d => d.Time);

            var data = await query.ToListAsync();

            // 根据时间颗粒度聚合数据
            var aggregatedData = AggregateDataByGranularity(data, granularity);

            return Json(new
            {
                Labels = aggregatedData.Select(d => d.Key).ToList(),
                Values = aggregatedData.Select(d => d.Value).ToList()
            });
        }

        // 按时间颗粒度聚合数据
        private Dictionary<string, double> AggregateDataByGranularity(List<ParameterData> data, string granularity)
        {
            var result = new Dictionary<string, double>();

            if (!data.Any())
                return result;

            switch (granularity)
            {
                case "minute":
                    // 按分钟聚合
                    var minuteGroups = data.GroupBy(d => new { d.Time.Date, d.Time.Hour, d.Time.Minute });
                    foreach (var group in minuteGroups)
                    {
                        var key = $"{group.Key.Date:yyyy-MM-dd} {group.Key.Hour:D2}:{group.Key.Minute:D2}";
                        // 显式转换为double（如果result的值类型是double）
                        result[key] = (double)group.Average(d => (double)d.Value);
                    }
                    break;

                case "hour":
                    // 按小时聚合
                    var hourGroups = data.GroupBy(d => new { d.Time.Date, d.Time.Hour });
                    foreach (var group in hourGroups)
                    {
                        var key = $"{group.Key.Date:yyyy-MM-dd} {group.Key.Hour:D2}:00";
                        // 显式转换为double
                        result[key] = (double)group.Average(d => (double)d.Value);
                    }
                    break;

                case "day":
                    // 按天聚合
                    var dayGroups = data.GroupBy(d => d.Time.Date);
                    foreach (var group in dayGroups)
                    {
                        var key = group.Key.ToString("yyyy-MM-dd");
                        // 显式转换为double
                        result[key] = (double)group.Average(d => (double)d.Value);
                    }
                    break;
            }

            return result;
        }
    }

    // 视图模型
    public class ParameterDataViewModel
    {
        public Dictionary<string, List<ParameterData>>?GroupedDatas { get; set; }
        public string[]?SelectedParameters { get; set; }
        public Dictionary<string, ParameterInfo>?ParameterInfo { get; set; }
        public List<StatisticViewModel>?Statistics { get; set; }

        public Dictionary<string, DeviceInfo> DeviceMap { get; set; } = new();
        public Dictionary<string, string> DeviceGroupMap { get; set; } = new();
    }

    public class ParameterInfo
    {
        public string?DeviceGroupName { get; set; }
        public string?DeviceName { get; set; }
        public string?ParameterName { get; set; }
        public string?FullName { get; set; }
    }

    public class StatisticViewModel
    {
        public string?ParameterFullName { get; set; }
        public decimal MaxValue { get; set; }  // 改为decimal
        public decimal MinValue { get; set; }  // 改为decimal
        public decimal AverageValue { get; set; }  // 改为decimal
    }
}
