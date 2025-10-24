using IsWorkabing.Models;
using Microsoft.EntityFrameworkCore;

namespace IsWorkabing.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // 设备报警数据表
        public DbSet<test1> test1 { get; set; }

    }
}
