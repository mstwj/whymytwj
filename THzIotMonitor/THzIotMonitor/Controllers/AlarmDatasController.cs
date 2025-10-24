using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using THzIotPlatform.Data;
using THzIotPlatform.Models;

namespace THzIotPlatform.Controllers
{
    public class AlarmDatasController : Controller
    {
        private readonly AppDbContext _context;

        public AlarmDatasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AlarmDatas
        public async Task<IActionResult> Index(DateTime? startTime, DateTime? endTime, string selectedDevices = "")
        {
            // 设置默认时间范围（最近7天）
            if (!startTime.HasValue)
                startTime = DateTime.Now.AddDays(-7);
            if (!endTime.HasValue)
                endTime = DateTime.Now;

            ViewBag.StartTime = startTime.Value.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.EndTime = endTime.Value.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.SelectedDevices = selectedDevices;
            ViewBag.HasSearched = !string.IsNullOrEmpty(selectedDevices);

            // 获取所有设备，用于筛选
            ViewBag.AllDevices = await _context.Devices
                .Include(d => d.DeviceGroup)
                .Select(d => new {
                    d.DeviceCode,
                    // 先判断 DeviceGroup 是否为 null，再拼接字符串
                    DeviceNameWithGroup = d.DeviceGroup != null
                        ? $"{d.DeviceGroup.GroupName} - {d.DeviceName}"
                        : d.DeviceName // 或使用默认值：$"未知分组 - {d.DeviceName}"
                })
                .ToListAsync();

            // 处理已选择的设备
            string selectedDevicesText = "";
            var selectedDeviceCodes = new List<string>();        
            if (!string.IsNullOrEmpty(selectedDevices))
            {
                // 过滤掉空字符串，确保集合中没有null
                selectedDeviceCodes = selectedDevices.Split(',')
                    .Where(d => !string.IsNullOrEmpty(d))
                    .ToList();

                if (selectedDeviceCodes.Any()) // 确保集合不为空
                {
                    // 构建显示文本，使用三元运算符替代空传播运算符
                    var deviceNames = await _context.Devices
                        .Where(d => selectedDeviceCodes.Contains(d.DeviceCode!)) // ! 表示确认不为null
                        .Include(d => d.DeviceGroup)
                        .Select(d => d.DeviceGroup != null
                            ? $"{d.DeviceGroup.GroupName} - {d.DeviceName}"
                            : d.DeviceName) // 或使用默认分组名：$"未知分组 - {d.DeviceName}"
                        .ToListAsync();

                    selectedDevicesText = string.Join("; ", deviceNames);
                }
            }

            ViewBag.SelectedDevicesText = selectedDevicesText;

            // 查询符合条件的报警数据
            var query = _context.AlarmDatas
                .Where(a => a.AlarmTime >= startTime.Value && a.AlarmTime <= endTime.Value);

            // 如果选择了设备，添加设备筛选条件
            if (selectedDeviceCodes.Any())
            {
                query = query.Where(a =>
                    a.DeviceCode != null &&  // 先确保DeviceCode不为null
                    selectedDeviceCodes.Contains(a.DeviceCode)
                );
            }

            // 关联设备组信息
            
            var result = await query
                .Join(_context.Devices,
                    alarm => alarm.DeviceCode,
                    device => device.DeviceCode,
                    (alarm, device) => new AlarmDataViewModel
                    {
                        Id = alarm.Id,
                        DeviceName = alarm.DeviceName,
                        DeviceCode = alarm.DeviceCode,
                        // 用三元运算符替代空传播运算符，兼容表达式树
                        GroupName = device.DeviceGroup != null ? device.DeviceGroup.GroupName : "",
                        AlarmType = alarm.AlarmType,
                        Value = alarm.Value,
                        AlarmTime = alarm.AlarmTime,
                        AlarmInfo = alarm.AlarmInfo
                    })
                .OrderByDescending(a => a.AlarmTime)
                .ToListAsync();

            return View(result);
        }
    }

    // 报警数据视图模型
    public class AlarmDataViewModel
    {
        public int Id { get; set; }
        public string?DeviceName { get; set; }
        public string?DeviceCode { get; set; }
        public string?GroupName { get; set; }
        public string?AlarmType { get; set; }
        public decimal?Value { get; set; }
        public DateTime AlarmTime { get; set; }
        public string?AlarmInfo { get; set; }
    }
}
