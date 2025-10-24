using Microsoft.EntityFrameworkCore;
using THzIotPlatform.Models;

namespace THzIotPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 设备组表
        public DbSet<DeviceGroup> DeviceGroups { get; set; }

        // 设备表
        public DbSet<Device> Devices { get; set; }

        // 设备参数表
        public DbSet<DeviceParameter> DeviceParameters { get; set; }

        // 设备参数数据表
        public DbSet<ParameterData> ParameterDatas { get; set; }

        // 设备报警数据表
        public DbSet<AlarmData> AlarmDatas { get; set; }

        //public DbSet<PhaseData> PhaseDatas { get; set; }
        //public DbSet<FaultDetail> FaultDetails { get; set; }
        //public DbSet<AreaElectricity> AreaElectricities { get; set; }
        //public DbSet<TopElectricityItem> TopElectricityItems { get; set; }
        //public DbSet<ElectricityChartData> ElectricityChartDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<FaultDetail>()
               // .HasKey(f => f.Id);

            
            // 配置实体关系和约束
            base.OnModelCreating(modelBuilder);
            // 配置实体关系和约束
            base.OnModelCreating(modelBuilder);

            // 设备组与设备的关系：一对多
            modelBuilder.Entity<Device>()
                .HasOne(d => d.DeviceGroup) // 每个设备属于一个设备组
                .WithMany(g => g.Devices)   // 一个设备组包含多个设备
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Restrict);// 限制删除，避免级联删除

            // 设备与设备参数的关系：一对多
            modelBuilder.Entity<DeviceParameter>()
                .HasOne(p => p.Device) // 每个参数属于一个设备
                .WithMany(d => d.Parameters) // 一个设备可以有多个参数（双向关联）
                .HasForeignKey(p => p.DeviceCode) // 外键是DeviceParameter.DeviceCode
                .HasPrincipalKey(d => d.DeviceCode);

            // 设备与报警数据的关系：一对多
            modelBuilder.Entity<AlarmData>()
               .HasOne(alarm => alarm.Device) // 每个报警属于一个设备
               .WithMany(device => device.AlarmDatas) // 一个设备可以有多个报警
               .HasForeignKey(alarm => alarm.DeviceCode) // 外键是AlarmData.DeviceCode
               .HasPrincipalKey(device => device.DeviceCode); 

           // 配置字符串长度限制，避免数据库字段过长
           modelBuilder.Entity<DeviceGroup>()
                .Property(g => g.GroupName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<DeviceGroup>()
                .Property(g => g.CompanyName)
                .HasMaxLength(100);

            modelBuilder.Entity<Device>()
                .Property(d => d.DeviceName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Device>()
                .Property(d => d.DeviceCode)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Device>()
                .Property(d => d.DeviceType)
                .HasMaxLength(50)
                .IsRequired();

            // 添加唯一索引
            modelBuilder.Entity<Device>()
                .HasIndex(d => d.DeviceCode)
                .IsUnique();
        }
    }
}
