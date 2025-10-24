using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using THzIotPlatform.Models;
using THzIotPlatform.Data;
using System.ComponentModel.DataAnnotations;

namespace THzIotPlatform.Controllers
{
    // 路由配置：默认路由为 /DeviceGroups/ActionName
    //[Route("[controller]/[action]")]
    public class DeviceGroupsController : Controller
    {

        // 数据库上下文（通过依赖注入获取）
        private readonly ApplicationDbContext _context;
        // 日志组件（用于记录操作日志和异常）
        private readonly ILogger<DeviceGroupsController> _logger;

        // 构造函数注入依赖
        public DeviceGroupsController(ApplicationDbContext context, ILogger<DeviceGroupsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        #region 列表页（Index）：查询所有设备组，支持搜索、分页
        /// <summary>
        /// 设备组列表页
        /// </summary>
        /// <param name="search">搜索关键词（匹配设备组名称）</param>
        /// <param name="page">当前页码（默认第1页）</param>
        /// <param name="pageSize">每页条数（默认10条）</param>
        /// <returns>列表视图</returns>
        public  async Task<IActionResult> Index(
            string? search = null,
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                // 1. 构建查询（包含关联的设备数据，用于统计设备数量）
                var query = _context.DeviceGroups
                    .Include(g => g.Devices) // 加载关联设备（按需加载，避免N+1问题）
                    .AsQueryable();

                // 2. 应用搜索筛选
                if (!string.IsNullOrEmpty(search))
                {
                    //query = query.Where(g => g.GroupName.Contains(search));
                    query = query.Where(g => g.GroupName != null && g.GroupName.Contains(search!));
                    ViewData["SearchText"] = search; // 回显搜索关键词
                }

                // 3. 分页处理
                var totalCount = await query.CountAsync(); // 总数据量
                var deviceGroups = await query
                    .OrderByDescending(g => g.CreateTime) // 按创建时间倒序
                    .Skip((page - 1) * pageSize) // 跳过前N条
                    .Take(pageSize) // 取当前页数据
                    .ToListAsync();

                // 4. 封装分页模型（传递给视图）
                var paginationModel = new PaginationViewModel<DeviceGroup>
                {
                    Data = deviceGroups,
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };

                return View(paginationModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "设备组列表查询失败，搜索关键词：{Search}", search);
                TempData["ErrorMsg"] = "加载设备组列表失败，请稍后重试";
                return View(new PaginationViewModel<DeviceGroup> { Data = new List<DeviceGroup>() });
            }
        }
        #endregion


        #region 详情页（Details）：查看单个设备组的完整信息
        /// <summary>
        /// 设备组详情页
        /// </summary>
        /// <param name="id">设备组ID</param>
        /// <returns>详情视图或404</returns>
        public async Task<IActionResult> Details(int? id)
        {
            // 1. 校验ID是否为空
            if (id == null)
            {
                return NotFound("设备组ID不能为空"); // 返回404
            }

            try
            {
                // 2. 查询设备组（包含完整关联设备信息）
                var deviceGroup = await _context.DeviceGroups
                    .Include(g => g.Devices) // 加载关联设备
                        .ThenInclude(d => d.Parameters) // 按需加载设备的参数（可选）
                    .FirstOrDefaultAsync(g => g.Id == id);

                // 3. 校验设备组是否存在
                if (deviceGroup == null)
                {
                    _logger.LogWarning("设备组详情查询失败，ID：{Id}（不存在）", id);
                    return NotFound($"未找到ID为 {id} 的设备组");
                }

                return View(deviceGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "设备组详情查询失败，ID：{Id}", id);
                TempData["ErrorMsg"] = "加载设备组详情失败，请稍后重试";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion


        #region 创建页（Create）：新增设备组
        /// <summary>
        /// 新建设备组（GET：显示表单）
        /// </summary>
        /// <returns>创建视图</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 提交新建设备组（POST：处理表单数据）
        /// </summary>
        /// <param name="deviceGroup">设备组实体（表单绑定）</param>
        /// <returns>重定向到列表页或返回表单（带错误）</returns>
        [HttpPost]
        [ValidateAntiForgeryToken] // 防跨站请求伪造（必须加）
        public async Task<IActionResult> Create(DeviceGroup deviceGroup)
        {
            // 1. 自定义数据验证：设备组名称不能重复
            var nameExists = await _context.DeviceGroups
                .AnyAsync(g => g.GroupName == deviceGroup.GroupName);
            if (nameExists)
            {
                ModelState.AddModelError("GroupName", "该设备组名称已存在，请更换名称");
            }

            // 2. 校验模型是否有效（包含数据注解和自定义验证）
            if (!ModelState.IsValid)
            {
                return View(deviceGroup); // 验证失败，返回表单并显示错误
            }

            try
            {
                // 3. 补充默认字段（前端无需传入）
                deviceGroup.CreateTime = DateTime.Now;
                deviceGroup.UpdateTime = DateTime.Now;

                // 4. 保存到数据库
                _context.Add(deviceGroup);
                await _context.SaveChangesAsync();

                _logger.LogInformation("新建设备组成功，ID：{Id}，名称：{Name}", deviceGroup.Id, deviceGroup.GroupName);
                TempData["SuccessMsg"] = "设备组创建成功";
                return RedirectToAction(nameof(Index)); // 成功后重定向到列表页
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "新建设备组数据库操作失败，名称：{Name}", deviceGroup.GroupName);
                ModelState.AddModelError(string.Empty, "数据库操作失败，请检查数据后重试");
                return View(deviceGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新建设备组异常，名称：{Name}", deviceGroup.GroupName);
                TempData["ErrorMsg"] = "设备组创建失败，请稍后重试";
                return View(deviceGroup);
            }
        }
        #endregion


        #region 编辑页（Edit）：修改设备组信息
        /// <summary>
        /// 编辑设备组（GET：显示表单，加载现有数据）
        /// </summary>
        /// <param name="id">设备组ID</param>
        /// <returns>编辑视图或404</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("设备组ID不能为空");
            }

            try
            {
                var deviceGroup = await _context.DeviceGroups.FindAsync(id);
                if (deviceGroup == null)
                {
                    _logger.LogWarning("编辑设备组查询失败，ID：{Id}（不存在）", id);
                    return NotFound($"未找到ID为 {id} 的设备组");
                }

                return View(deviceGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "编辑设备组查询失败，ID：{Id}", id);
                TempData["ErrorMsg"] = "加载编辑表单失败，请稍后重试";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// 提交编辑设备组（POST：处理更新）
        /// </summary>
        /// <param name="id">设备组ID（路由参数，用于校验）</param>
        /// <param name="deviceGroup">设备组实体（表单绑定）</param>
        /// <returns>重定向到列表页或返回表单</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DeviceGroup deviceGroup)
        {
            // 1. 校验路由ID与表单ID是否一致
            if (id != deviceGroup.Id)
            {
                _logger.LogWarning("编辑设备组ID不匹配，路由ID：{RouteId}，表单ID：{FormId}", id, deviceGroup.Id);
                return BadRequest("参数不匹配");
            }

            // 2. 校验设备组是否存在
            var existingGroup = await _context.DeviceGroups.FindAsync(id);
            if (existingGroup == null)
            {
                return NotFound($"未找到ID为 {id} 的设备组");
            }

            // 3. 自定义验证：名称修改时不能与其他设备组重复
            if (existingGroup.GroupName != deviceGroup.GroupName)
            {
                var nameExists = await _context.DeviceGroups
                    .AnyAsync(g => g.GroupName == deviceGroup.GroupName && g.Id != id);
                if (nameExists)
                {
                    ModelState.AddModelError("GroupName", "该设备组名称已存在，请更换名称");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(deviceGroup);
            }

            try
            {
                // 4. 更新字段（仅更新允许修改的字段，避免覆盖未提交的属性）
                existingGroup.GroupName = deviceGroup.GroupName;
                existingGroup.Description = deviceGroup.Description;
                existingGroup.UpdateTime = DateTime.Now; // 更新时间戳

                // 5. 保存更新
                _context.Update(existingGroup);
                await _context.SaveChangesAsync();

                _logger.LogInformation("编辑设备组成功，ID：{Id}，名称：{Name}", id, deviceGroup.GroupName);
                TempData["SuccessMsg"] = "设备组更新成功";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // 并发冲突处理：其他用户已修改该数据
                _logger.LogError(ex, "编辑设备组并发冲突，ID：{Id}", id);
                ModelState.AddModelError(string.Empty, "该数据已被其他用户修改，请刷新后重试");
                return View(deviceGroup);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "编辑设备组数据库操作失败，ID：{Id}", id);
                ModelState.AddModelError(string.Empty, "数据库操作失败，请检查数据后重试");
                return View(deviceGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "编辑设备组异常，ID：{Id}", id);
                TempData["ErrorMsg"] = "设备组更新失败，请稍后重试";
                return View(deviceGroup);
            }
        }
        #endregion


        #region 删除页（Delete）：删除设备组
        /// <summary>
        /// 删除设备组（GET：确认页面）
        /// </summary>
        /// <param name="id">设备组ID</param>
        /// <returns>确认视图或404</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("设备组ID不能为空");
            }

            try
            {
                // 查询设备组（包含关联设备，用于校验是否有依赖）
                var deviceGroup = await _context.DeviceGroups
                    .Include(g => g.Devices)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (deviceGroup == null)
                {
                    _logger.LogWarning("删除设备组查询失败，ID：{Id}（不存在）", id);
                    return NotFound($"未找到ID为 {id} 的设备组");
                }

                // 校验是否有关联设备（有则禁止删除）
                if (deviceGroup.Devices != null && deviceGroup.Devices.Any())
                {
                    TempData["ErrorMsg"] = $"该设备组关联了 {deviceGroup.Devices.Count} 台设备，无法删除（请先移除关联设备）";
                    return RedirectToAction(nameof(Details), new { id });
                }

                return View(deviceGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除设备组查询失败，ID：{Id}", id);
                TempData["ErrorMsg"] = "加载删除确认页失败，请稍后重试";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// 确认删除设备组（POST：执行删除）
        /// </summary>
        /// <param name="id">设备组ID</param>
        /// <returns>重定向到列表页</returns>
        [HttpPost, ActionName("Delete")] //  ActionName：与GET方法共用"Delete"名称
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var deviceGroup = await _context.DeviceGroups.FindAsync(id);
                if (deviceGroup == null)
                {
                    _logger.LogWarning("执行删除时设备组不存在，ID：{Id}", id);
                    TempData["ErrorMsg"] = "该设备组已不存在";
                    return RedirectToAction(nameof(Index));
                }

                // 再次校验关联设备（避免GET到POST之间数据变化）
                var hasDevices = await _context.Devices.AnyAsync(d => d.GroupId == id);
                if (hasDevices)
                {
                    TempData["ErrorMsg"] = "该设备组已关联设备，无法删除";
                    return RedirectToAction(nameof(Details), new { id });
                }

                // 执行删除
                _context.DeviceGroups.Remove(deviceGroup);
                await _context.SaveChangesAsync();

                _logger.LogInformation("删除设备组成功，ID：{Id}，名称：{Name}", id, deviceGroup.GroupName);
                TempData["SuccessMsg"] = "设备组删除成功";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "删除设备组数据库操作失败，ID：{Id}", id);
                TempData["ErrorMsg"] = "数据库操作失败，请稍后重试";
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除设备组异常，ID：{Id}", id);
                TempData["ErrorMsg"] = "设备组删除失败，请稍后重试";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
        #endregion


        #region 辅助方法：校验设备组是否存在（供内部使用）
        /// <summary>
        /// 校验设备组是否存在
        /// </summary>
        /// <param name="id">设备组ID</param>
        /// <returns>存在返回true，否则false</returns>
        private async Task<bool> DeviceGroupExists(int id)
        {
            return await _context.DeviceGroups.AnyAsync(g => g.Id == id);
        }
        #endregion
    }


    
}