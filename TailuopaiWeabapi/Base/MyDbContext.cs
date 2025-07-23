using Microsoft.EntityFrameworkCore;
using TailuopaiWeabapi.Table;

namespace TailuopaiWeabapi.Base
{
    public class MyDbContext : DbContext
    {
        public DbSet<Table_StandardInfo> Myopenaiinfo { get; set; } //样品信息
        public MyDbContext()
        {
            this.Database.SetCommandTimeout(10);//设置SqlCommand永不超时
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLoggerFactory(new EFLoggerFactory());//将EFLoggerFactory类的实例注入给EF Core，这样所有DbContext的Log信息，都会由EFLogger类输出到Visual Studio的输出窗口了            
                                                                   //optionsBuilder.UseSqlServer($"server ={sql_connect.ip},{sql_connect.port}; database = DeWei; pwd = {sql_connect.password}; uid = {sql_connect.user}; TrustServerCertificate = True;");            
            optionsBuilder.UseSqlServer(@"server = .\JSQL2008; database = mytest; pwd = 1; uid = sa; TrustServerCertificate = True;");
        }
    }
}
