using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using net5_10_14.Table;
using net5_10_14.Table.bbsy;
using net5_10_14.Table.zzsy;

namespace net5_10_14.Base
{
    //独特的SQL语句....
    public static class App_Config
    {
        public static string dataBaseip { get; set; }
        public static string dataBaseport { get; set; }
        public static string dataBaseuser { get; set; }
        public static string dataBasePassword { get; set; }
        public static int currendProcId { get; set; } = 0;
        public static string currendProName { get; set; }
        public static string currendGuigeXinhao { get; set; }
        public static string currendTuhao { get; set; }

    }

    public class EFLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EFLogger(categoryName);//创建EFLogger类的实例
        }

        public void Dispose()
        {
        }
    }

    public class EFLogger : ILogger
    {
        protected string categoryName;

        public EFLogger(string categoryName)
        {
            this.categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //通过Debugger.Log方法来将EF Core生成的Log信息输出到Visual Studio的输出窗口
            Debugger.Log(0, categoryName, "=============================== EF Core log started ===============================\r\n");
            Debugger.Log(0, categoryName, formatter(state, exception) + "\r\n");
            Debugger.Log(0, categoryName, "=============================== EF Core log finished ===============================\r\n");
        }
    }

    public class MyDbContext : DbContext
    {   

        public DbSet<newproinfo> newproinfo { get; set; } //样品信息
        public DbSet<experimentstandard_bianbishiyan> experimentstandard_bianbishiyan { get; set; } //变比标准
        public DbSet<bianbishiyan_report> bianbishiyan_report { get; set; } //表笔报告        
       
        public DbSet<bb_report> bb_report { get; set; } //表报告2
        public DbSet<experimentstandard_ziliudianzushiyan> experimentstandard_ziliudianzushiyan { get; set; }//直租标准表
        
        
        public DbSet<directlease_report> directlease_report { get; set; } //直租报告.
        public DbSet<zz_report> zz_report { get; set; } //直租报告.2
        



        public MyDbContext()
        {
            this.Database.SetCommandTimeout(10);//设置SqlCommand永不超时
        }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(new EFLoggerFactory());//将EFLoggerFactory类的实例注入给EF Core，这样所有DbContext的Log信息，都会由EFLogger类输出到Visual Studio的输出窗口了            
            //optionsBuilder.UseSqlServer($"server ={sql_connect.ip},{sql_connect.port}; database = DeWei; pwd = {sql_connect.password}; uid = {sql_connect.user}; TrustServerCertificate = True;");            
            optionsBuilder.UseSqlServer(@"server = .\JSQL2008; database = Weida; pwd = 1; uid = sa; TrustServerCertificate = True;");
        }

    }

    //这是一种对象潜拷贝...
    public static class DbContextExtensions
    {

        private static void CombineParams(ref DbCommand command, params object[] parameters)
        {
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (!parameter.ParameterName.Contains("@"))
                        parameter.ParameterName = $"@{parameter.ParameterName}";
                    command.Parameters.Add(parameter);
                }
            }
        }

        private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection dbConn, params object[] parameters)
        {
            DbConnection conn = facade.GetDbConnection();
            dbConn = conn;
            conn.Open();
            DbCommand cmd = conn.CreateCommand();
            if (facade.IsSqlServer())
            {
                cmd.CommandText = sql;
                CombineParams(ref cmd, parameters);
            }
            return cmd;
        }

        public static DataTable SqlQuery(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            DbCommand cmd = CreateCommand(facade, sql, out DbConnection conn, parameters);
            DbDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }

        public static IEnumerable<T> SqlQuery<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
        {
            DataTable dt = SqlQuery(facade, sql, parameters);
            return dt.ToEnumerable<T>();
        }

        public static IEnumerable<T> ToEnumerable<T>(this DataTable dt) where T : class, new()
        {
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            T[] ts = new T[dt.Rows.Count];
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                T t = new T();
                foreach (PropertyInfo p in propertyInfos)
                {
                    if (dt.Columns.IndexOf(p.Name) != -1 && row[p.Name] != DBNull.Value)
                        p.SetValue(t, row[p.Name], null);
                }
                ts[i] = t;
                i++;
            }
            return ts;
        }


    }
}
