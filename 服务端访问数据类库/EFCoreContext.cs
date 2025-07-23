using System;
using Microsoft.EntityFrameworkCore;
using 服务端访问数据类库.表;

namespace 服务端访问数据类库
{
    public class EFCoreContext : DbContext
    {
        //add-migration A4
        //update-database
        //有时候出现一些 稀奇古怪的问题，退出2022 在启动工程，就OK了..
        //只要设置我就OK了，和其他工程没有任何关系.(开始就设置我为启动项目..)
        //注意这里出现了，BUILD错误，很有可能是其他的程序集 影响，不知道为什么，反正 看 错误列表，如果有1个，都会
        //莫名其妙的BUILD FAIL，记住了..
        //SysUserInfo 就是表名..
        public DbSet<SystemUserInfo> SysUserInfo { get; set; }


        //MenuInfo 
        public DbSet<MenuInfo> MenuInfo { get; set; }

        public DbSet<UserDescInformat> UserDescInformat { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        public EFCoreContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                                                                   //optionsBuilder.UseSqlServer($"server ={sql_connect.ip},{sql_connect.port}; database = DeWei; pwd = {sql_connect.password}; uid = {sql_connect.user}; TrustServerCertificate = True;");            
            optionsBuilder.UseSqlServer(@"server = .\JSQL2008; database = zx_sp_record; pwd = sjk123; uid = sa; TrustServerCertificate = True;");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
