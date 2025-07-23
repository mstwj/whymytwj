using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 童文君第一个Prism项目.Interface;

namespace 童文君第一个Prism项目.InterfaceDal
{
    public class DataAccess : IDataAccess
    {
        SQLiteConnection conn;
        SQLiteCommand cmd;
        SQLiteDataAdapter adapter;

               
        public DataTable GetMenuAll(string index)
        {
            try
            {
                conn = new SQLiteConnection("Data Source=data.j.db;");
                conn.Open();

                string sql = $"select menu_id,menu_header,target_view,pid from menus where cid='{index}'";
                adapter = new SQLiteDataAdapter(sql, conn);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn = null;
            }
        }

        public bool Login(string username, string password)
        {
            try
            {
                // 数据库的访问 -- 这里会按照你EXE的路线来收...注意：这里必须是data.j.db -- 一定要在EXE下面..
                conn = new SQLiteConnection("Data Source=data.j.db;");
                conn.Open();

                // Sql注入攻击
                string sql = $"select count(1) from user where user_name='{username}' and pwd = '{password}'";
                cmd = new SQLiteCommand(sql, conn);

                var result = cmd.ExecuteScalar();// 返回数据集合的第一行第一列数据

                return result.ToString() == "1";
            }
            catch (Exception)
            {
                //这里错误了，上抛？？
                throw;
            }
            finally
            {
                conn.Close();
                conn = null;
            }
        }
    }
    
}
