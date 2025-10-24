using Microsoft.EntityFrameworkCore;
using THzIotPlatform.Models;

namespace THzIotPlatform.Data
{
    /// <summary>
    /// 电力监控系统数据库上下文
    /// </summary>
    public class ElectricityDbContext : DbContext
    {
        public ElectricityDbContext(DbContextOptions<ElectricityDbContext> options) : base(options)
        {
        }

        // 数据库表映射
        public DbSet<PhaseRecord> PhaseRecords { get; set; }
        public DbSet<FaultRecord> FaultRecords { get; set; }
        public DbSet<AreaConsumption> AreaConsumptions { get; set; }
        public DbSet<ElectricitySummary> ElectricitySummaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 配置实体关系和约束
            modelBuilder.Entity<PhaseRecord>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<FaultRecord>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<AreaConsumption>()
                .HasKey(a => a.Id);

            // 设置索引以提高查询性能
            modelBuilder.Entity<PhaseRecord>()
                .HasIndex(p => p.RecordTime);

            modelBuilder.Entity<FaultRecord>()
                .HasIndex(f => f.FaultTime);

            modelBuilder.Entity<AreaConsumption>()
                .HasIndex(a => new { a.AreaName, a.RecordDate });
        }
    }
}
