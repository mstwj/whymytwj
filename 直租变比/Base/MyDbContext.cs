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
using 直租变比.Table;

namespace 直租变比.Base
{
    //独特的SQL语句....
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
        public DbSet<Table_ProductInfo> ProductInfo { get; set; } //样品信息
        public DbSet<Table_RocreadInfo> RocreadInfo { get; set; } //记录信息
        public MyDbContext()
        {
            this.Database.SetCommandTimeout(10);//设置SqlCommand永不超时
        }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(new EFLoggerFactory());//将EFLoggerFactory类的实例注入给EF Core，这样所有DbContext的Log信息，都会由EFLogger类输出到Visual Studio的输出窗口了            
                                                                   //optionsBuilder.UseSqlServer($"server ={sql_connect.ip},{sql_connect.port}; database = DeWei; pwd = {sql_connect.password}; uid = {sql_connect.user}; TrustServerCertificate = True;");            
            optionsBuilder.UseSqlServer(@"server = .\JSQL2008; database = master; pwd = 1; uid = sa; TrustServerCertificate = True;");
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

        //以下代码已经不是比较 ，是赋值了...
        public static class ObjectComparer
        {
            public static void CompareObjects(object obj1, object obj2)
            {
                if (obj1 == null || obj2 == null)
                    throw new ArgumentException("Objects cannot be null");

                if (!obj1.GetType().Equals(obj2.GetType()))
                    throw new ArgumentException("Objects should be of the same type");


                PropertyInfo[] properties = obj1.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    var value1 = property.GetValue(obj1);
                    var value2 = property.GetValue(obj2);

                    if (!Equals(value1, value2))
                    {
                        PropertyInfo sourceProperty = obj2.GetType().GetProperty(property.Name);
                        property.SetValue(obj1, sourceProperty.GetValue(obj2));
                    }
                }
            }
        }




    }
}
