using THzIotPlatform.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; 
//using THzIotPlatform.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据服务注入扩展
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// 注册数据服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            // 注册数据库上下文
            services.AddDbContext<ElectricityDbContext>(options =>
                options.UseSqlServer("name=ConnectionStrings:ElectricityDb"));

            // 注册仓储服务
            services.AddScoped<IElectricityRepository, ElectricityRepository>();

            return services;
        }
    }
}