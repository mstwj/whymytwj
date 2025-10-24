// Models/PaginationViewModel.cs
namespace THzIotPlatform.Models // 必须与视图中引用的命名空间一致
{
    /// <summary>
    /// 通用分页视图模型（支持任意数据类型的分页）
    /// </summary>
    /// <typeparam name="T">分页数据的类型（如 DeviceGroup）</typeparam>
    public class PaginationViewModel<T>
    {
        /// <summary>
        /// 当前页的数据列表
        /// </summary>
        public List<T> Data { get; set; } = new List<T>(); // 初始化空列表，避免null

        /// <summary>
        /// 总数据量（所有页的合计）
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 每页显示的条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总页数（由 TotalCount / PageSize 计算得出）
        /// </summary>
        public int TotalPages { get; set; }
    }
}