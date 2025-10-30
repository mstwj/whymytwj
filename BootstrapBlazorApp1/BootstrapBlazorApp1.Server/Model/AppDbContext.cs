using Microsoft.EntityFrameworkCore;

namespace BootstrapBlazorApp1.Server.Model
{
    public class AppDbContext : DbContext
    {
        // 构造函数注入配置
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 定义数据库表对应的 DbSet
        public DbSet<UserTest> Users { get; set; }
    }
}
