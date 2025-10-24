using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using THzIotPlatform.Data;
using THzIotPlatform.Models;

namespace THzIotPlatform.Controllers
{
    public class DeviceParametersController : Controller
    {
        // 数据库上下文（通过依赖注入获取）
        private readonly ApplicationDbContext _context;
        // 日志组件（用于记录操作日志和异常）
        private readonly ILogger<DeviceParametersController> _logger;

        public DeviceParametersController(ApplicationDbContext context, ILogger<DeviceParametersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: DeviceParameters
        public async Task<IActionResult> Index(
        string? search = null,
        int page = 1,
        int pageSize = 10)
        {
            try
            {
                // 1. 构建设备查询（查询的是 DeviceParameters 实体，而非 DeviceViewModel）
                var query = _context.DeviceParameters
                    .Include(d => d.Device)// 按需加载关联的设备
                    .AsQueryable();

                // 2. 应用搜索筛选（按设备名称/IP搜索）
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d =>
                        d.AccessAddress != null && d.AccessAddress.Contains(search) ||
                        d.ParameterName != null && d.ParameterName.Contains(search));
                    ViewData["SearchText"] = search; // 回显搜索关键词
                }

                // 3. 分页计算
                var totalCount = await query.CountAsync(); // 总数据量
                var DeviceParameter = await query
                    .OrderByDescending(d => d.CreateTime) // 按创建时间倒序
                    .Skip((page - 1) * pageSize) // 跳过前N条
                    .Take(pageSize) // 取当前页数据
                    .ToListAsync();

                // 4. 构建视图期望的 PaginationViewModel<Device> 模型
                var paginationModel = new PaginationViewModel<DeviceParameter>
                {
                    Data = DeviceParameter, // 当前页的 Device 列表
                    TotalCount = totalCount, // 总数据量
                    CurrentPage = page, // 当前页码
                    PageSize = pageSize, // 每页条数
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize) // 总页数
                };

                // 5. 传递分页模型给视图（类型匹配）
                return View(paginationModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "设备参数列表查询失败");
                TempData["ErrorMsg"] = "加载设备参数列表失败，请稍后重试";
                // 异常时返回空分页模型，避免视图报错
                return View(new PaginationViewModel<DeviceParameter> { Data = new List<DeviceParameter>() });
            }
        }

        // GET: DeviceParameters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DeviceParameters == null)
            {
                return NotFound();
            }
         
            // 先获取基础数据，不急于加载关联对象
            var deviceParameter = await _context.DeviceParameters
                .FirstOrDefaultAsync(m => m.Id == id);

            // 初始化所有可能用到的变量为默认值
            string groupName = "未知设备组";
            string deviceName = "未知设备";
            int? groupId = null;

            // 只有当主对象存在时，才尝试加载关联数据
            if (deviceParameter != null)
            {
                // 单独加载设备信息（带空检查）
                var device = await _context.Devices
                    .FirstOrDefaultAsync(d => d.DeviceCode == deviceParameter.DeviceCode);

                if (device != null)
                {
                    // 安全获取设备名称
                    deviceName = !string.IsNullOrEmpty(device.DeviceName) ? device.DeviceName : deviceName;
                    groupId = device.GroupId;

                    // 单独加载设备组信息（带空检查）
                    //if (device.GroupId.HasValue)
                    //{
                        var deviceGroup = await _context.DeviceGroups
                            .FirstOrDefaultAsync(g => g.Id == device.GroupId);

                        if (deviceGroup != null)
                        {
                            groupName = !string.IsNullOrEmpty(deviceGroup.GroupName) ? deviceGroup.GroupName : groupName;
                        }
                    //}
                }



                // 将关联数据手动附加到主对象（如果需要）
                deviceParameter.Device = device;
            }

            if (deviceParameter == null)
            {
                return NotFound();
            }

            return View(deviceParameter);
        }

        // GET: DeviceParameters/Create
        public IActionResult Create()
        {
            ViewData["Devices"] = _context.Devices.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DeviceCode,ParameterName,AccessAddress,IsGroupProperty,IsAccumulatedParameter,Unit")] DeviceParameter deviceParameter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deviceParameter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Devices"] = _context.Devices.ToList();
            return View(deviceParameter);
        }

        // GET: DeviceParameters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DeviceParameters == null)
            {
                return NotFound();
            }

            var deviceParameter = await _context.DeviceParameters.FindAsync(id);
            if (deviceParameter == null)
            {
                return NotFound();
            }
            ViewData["Devices"] = _context.Devices.ToList();
            return View(deviceParameter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DeviceCode,ParameterName,AccessAddress,IsGroupProperty,IsAccumulatedParameter,Unit")] DeviceParameter deviceParameter)
        {
            if (id != deviceParameter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deviceParameter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceParameterExists(deviceParameter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Devices"] = _context.Devices.ToList();
            return View(deviceParameter);
        }

        // GET: DeviceParameters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DeviceParameters == null)
            {
                return NotFound();
            }

            var deviceParameter = await _context.DeviceParameters
                .Include(p => p.Device)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deviceParameter == null)
            {
                return NotFound();
            }

            return View(deviceParameter);
        }

        // POST: DeviceParameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DeviceParameters == null)
            {
                return Problem("Entity set 'AppDbContext.DeviceParameters'  is null.");
            }
            var deviceParameter = await _context.DeviceParameters.FindAsync(id);
            if (deviceParameter != null)
            {
                _context.DeviceParameters.Remove(deviceParameter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 用于前端异步获取指定设备下的参数
        public async Task<IActionResult> GetParametersByDeviceCode(string deviceCode)
        {
            var parameters = await _context.DeviceParameters
                .Where(p => p.DeviceCode == deviceCode)
                .Select(p => new { id = p.Id, parameterName = p.ParameterName })
                .ToListAsync();

            return Json(parameters);
        }

        private bool DeviceParameterExists(int id)
        {
            return (_context.DeviceParameters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

    // 设备参数列表视图模型
    public class ParameterViewModel
    {
        public int Id { get; set; }
        public string? DeviceCode { get; set; }
        public string? DeviceName { get; set; }
        public string? GroupName { get; set; }
        public string? ParameterName { get; set; }
        public string? AccessAddress { get; set; }
        public string? Unit { get; set; }
    }
}
