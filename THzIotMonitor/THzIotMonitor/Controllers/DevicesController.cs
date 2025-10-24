using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using THzIotPlatform.Data;
using THzIotPlatform.Models;
using Microsoft.Extensions.Logging;

namespace THzIotPlatform.Controllers
{
    public class DevicesController : Controller
    {
        // 数据库上下文（通过依赖注入获取）
        private readonly ApplicationDbContext _context;
        // 日志组件（用于记录操作日志和异常）
        private readonly ILogger<DevicesController> _logger;

        // 构造函数注入依赖
        public DevicesController(ApplicationDbContext context, ILogger<DevicesController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // GET: Devices
        
        public async Task<IActionResult> Index(
        string? search = null,
        int page = 1,
        int pageSize = 10)
        {
            try
            {
                // 1. 构建设备查询（查询的是 Device 实体，而非 DeviceViewModel）
                var query = _context.Devices
                    .Include(d => d.DeviceGroup) // 按需加载关联的设备组
                    .Include(d => d.Parameters)
                    .AsQueryable();

                // 2. 应用搜索筛选（按设备名称/IP搜索）
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(d =>
                        d.DeviceName != null && d.DeviceName.Contains(search) ||
                        d.DeviceCode != null && d.DeviceCode.Contains(search));
                    ViewData["SearchText"] = search; // 回显搜索关键词
                }

                // 3. 分页计算
                var totalCount = await query.CountAsync(); // 总数据量
                var devices = await query
                    .OrderByDescending(d => d.CreateTime) // 按创建时间倒序
                    .Skip((page - 1) * pageSize) // 跳过前N条
                    .Take(pageSize) // 取当前页数据
                    .ToListAsync();

                // 4. 构建视图期望的 PaginationViewModel<Device> 模型
                var paginationModel = new PaginationViewModel<Device>
                {
                    Data = devices, // 当前页的 Device 列表
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
                _logger.LogError(ex, "设备列表查询失败");
                TempData["ErrorMsg"] = "加载设备列表失败，请稍后重试";
                // 异常时返回空分页模型，避免视图报错
                return View(new PaginationViewModel<Device> { Data = new List<Device>() });
            }
        }

        // GET: Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Devices == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.DeviceGroup)
                .Include(d => d.Parameters)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Devices/Create
        public async Task<IActionResult> Create()
        {
            var deviceGroups = await _context.DeviceGroups
                    .OrderBy(g => g.GroupName) // 按设备组名称升序排列
                    .ToListAsync();

            // 2. 将设备组数据存入 ViewData，供视图读取
            ViewData["DeviceGroups"] = deviceGroups;

            ViewBag.DeviceGroups = _context.DeviceGroups.ToList(); // 赋值到 ViewBag
                                                                     // 或 ViewData["DeviceGroups"] = _dbContext.DeviceGroups.ToList();
           
            ViewData["DeviceTypes"] = new List<string> { "电表", "自动补偿控制器" };
            //return View();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GroupId,DeviceName,DeviceCode,DeviceType,Brand,Model,ManufactureDate,PurchaseDate,FactoryNumber,InstallationLocation,Remarks")] Device device)
        {
            if (ModelState.IsValid)
            {
                // 3. 补充默认字段（前端无需传入）
                device.CreateTime = DateTime.Now;
                device.UpdateTime = DateTime.Now;
                _context.Add(device);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Groups"] = _context.DeviceGroups.ToList();
            ViewData["DeviceTypes"] = new List<string> { "电表", "自动补偿控制器" };
            return View(device);
        }

        // GET: Devices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Devices == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            var deviceGroups = await _context.DeviceGroups
                    .OrderBy(g => g.GroupName) // 按设备组名称升序排列
                    .ToListAsync();

            // 2. 将设备组数据存入 ViewData，供视图读取
            ViewData["DeviceGroups"] = deviceGroups;

            ViewBag.DeviceGroups = _context.DeviceGroups.ToList(); // 赋值到 ViewBag
                                                                   // 或 ViewData["DeviceGroups"] = _dbContext.DeviceGroups.ToList();
            ViewData["DeviceTypes"] = new List<string> { "电表", "自动补偿控制器" };
            return View(device);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GroupId,DeviceName,DeviceCode,DeviceType,Brand,Model,ManufactureDate,PurchaseDate,FactoryNumber,InstallationLocation,Remarks")] Device device)
        {
            if (id != device.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.Id))
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
            ViewData["Groups"] = _context.DeviceGroups.ToList();
            ViewData["DeviceTypes"] = new List<string> { "电表", "自动补偿控制器" };
            return View(device);
        }

        // GET: Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Devices == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(d => d.DeviceGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Devices == null)
            {
                return Problem("Entity set 'AppDbContext.Devices'  is null.");
            }
            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 用于前端异步获取指定设备组下的设备
        public async Task<IActionResult> GetDevicesByGroupId(int groupId)
        {
            var devices = await _context.Devices
                .Where(d => d.GroupId == groupId)
                .Select(d => new { deviceCode = d.DeviceCode, deviceName = d.DeviceName })
                .ToListAsync();

            return Json(devices);
        }

        private bool DeviceExists(int id)
        {
            return (_context.Devices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

    // 设备列表视图模型
    public class DeviceViewModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceCode { get; set; }
        public string? DeviceType { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
    }
}
