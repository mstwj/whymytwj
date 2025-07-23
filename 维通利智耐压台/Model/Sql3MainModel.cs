using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using 维通利智耐压台.MyTable;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml.Linq;
using 维通利智耐压台.Base;

namespace 维通利智耐压台.Model
{
    public class Sql3MainModel : ObservableValidator
    {
        public ObservableCollection<Tabel_User> myCollection { get; set; } = new ObservableCollection<Tabel_User>();
                
        //private List<Tabel_User> TableUserList = null;
        public ICommand BtnAddCommand { get; set; }
        public ICommand BtnEditCommand { get; set; }
        public ICommand BtnDeleteCommand { get; set; }

        private string textBox1 = string.Empty;
        private string textBox2 = string.Empty;
        private string textBox3 = string.Empty;

        [Required(ErrorMessage = "必须输入用户名")]
        [MinLength(2, ErrorMessage = "用户名最小不能小于2个字符")]
        [MaxLength(10, ErrorMessage = "用户名最大不能超过10个字符")]
        public string TextBox1 { get => textBox1; set { SetProperty(ref textBox1, value, true); } }

        [Required(ErrorMessage = "必须输入密码")]
        [MinLength(2, ErrorMessage = "密码最小不能小于2个字符")]
        [MaxLength(10, ErrorMessage = "密码最大不能超过10个字符")]
        public string TextBox2 { get => textBox2; set { SetProperty(ref textBox2, value, true); } }

        [Required(ErrorMessage = "必须输入权限")]
        [MinLength(1, ErrorMessage = "权限最小不能小于1个字符")]
        [MaxLength(5, ErrorMessage = "权限最大不能超过5个字符")]
        public string TextBox3 { get => textBox3; set { SetProperty(ref textBox3, value, true); } }

        public Tabel_User mySelectedItem { get; set; } = null;

        public Sql3MainModel() 
        {
            BtnAddCommand = new RelayCommand<object>(DoBtnAddCommand);
            BtnEditCommand = new RelayCommand<object>(DoBtnEditCommand);
            BtnDeleteCommand = new RelayCommand<object>(DoBtnDeleteCommand);
            Connection();
        }

        private void DoBtnDeleteCommand(object param)
        {
            if (mySelectedItem == null)
            {
                MessageBox.Show("无法删除用户,请先选择一个用户");
                return;
            }

            MyTools.DeleteTableUser((int)mySelectedItem.Id);
            MyTools.UpateTableUser(myCollection);
        }

        private void DoBtnAddCommand(object param)
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return;
            }
            else
            {
                MyTools.AddTableUser(TextBox1,TextBox2,TextBox3);
                MyTools.UpateTableUser(myCollection);
            }            
        }

        private void DoBtnEditCommand(object param)
        {
            //MyEdit();
            //Connection();
            if (mySelectedItem == null)
            {
                MessageBox.Show("无法修改用户表,请先选择一个用户");
                return;
            }
            MyTools.EditTableUser(mySelectedItem);
            MyTools.UpateTableUser(myCollection);
        }

        void MyEdit()
        {
            /*
            try
            {
                
                if (mySelectedItem == null)
                {
                    MessageBox.Show("无法修改用户表,请先选择一个用户");
                    return;
                }

                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //@来避免转义
                string dbPath = @"Data Source=" + exeDirectory + "mySql3.db;Version=3";

                //string strSql = "UPDATE User SET Username = @value1,Password = @value2,Power = @value3 WHERE Id = @mySelectedItem.Id";


                string strSql = string.Format("UPDATE User SET Username ='{0}',Password='{1}',Power='{2}' WHERE Id ='{3}'", mySelectedItem.Username, mySelectedItem.Password, mySelectedItem.Power, mySelectedItem.Id);

                //string strSql = "update User set Username = " + mySelectedItem.Username + " where Id='" + mySelectedItem.Id + "'";
                SQLiteConnection Conn = new SQLiteConnection(dbPath);
                Conn.Open();

                SQLiteCommand command = new SQLiteCommand(strSql, Conn);
                //command.Parameters.AddRange(para);
                command.ExecuteNonQuery();

                Conn.Close();

                MessageBox.Show("数据库修改完成!");

            }
            catch(Exception ex)
            {
                MessageBox.Show("数据库操作异常:" + ex.Message);
                return;
            }
            */
        }

        void Delete()
        {
            /*
            try
            {
                if (mySelectedItem == null)
                {
                    MessageBox.Show("必须选择一个用户,才能删除!");
                    return;
                }
                //按条件删除行
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //@来避免转义
                string dbPath = @"Data Source=" + exeDirectory + "mySql3.db;Version=3";

                string strSql = "delete from User where Username='" + mySelectedItem.Username + "'";
                SQLiteConnection Conn = new SQLiteConnection(dbPath);
                Conn.Open();

                SQLiteCommand command = new SQLiteCommand(strSql, Conn);
                if (command.ExecuteNonQuery() == 1) MessageBox.Show("删除成功!");
                else MessageBox.Show("删除失败!");

                Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库操作异常:"+ex.Message);
                return;
            }
            */
        }

        void Add()
        {
            /*
            try
            {
                //按字段号添加数据，这个非常有用，网上的都是很一长串添加，此方法可以单个字段添加数据。            
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //@来避免转义
                string dbPath = @"Data Source=" + exeDirectory + "mySql3.db;Version=3";
                string strSql = "select * from User";
                SQLiteConnection Conn = new SQLiteConnection(dbPath);
                Conn.Open();

                SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(strSql, Conn);
                SQLiteCommandBuilder builder = new SQLiteCommandBuilder(mAdapter);
                DataTable rs = new DataTable();
                mAdapter.Fill(rs);

                DataRow dr = rs.NewRow();
                rs.Rows.Add(dr);

                dr["Username"] = TextBox1;
                dr["Password"] = TextBox2;
                dr["Power"] = TextBox3;
                mAdapter.Update(rs);


                Conn.Close();
                MessageBox.Show("添加数据成功!");

            }
            catch(Exception ex)
            {
                MessageBox.Show("数据库操作异常:"+ex.Message);
                return;
            }
            */
        }


        //连接数据库
        void Connection()
        {
            /*
            //D:\2022\now\维通利智耐压台\bin\Debug\net5.0-windows\
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //@来避免转义
            string dbPath = @"Data Source=" + exeDirectory + "mySql3.db;Version=3";
            string strSql = "select * from User";
            SQLiteConnection Conn = new SQLiteConnection(dbPath);
            Conn.Open();

            SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(strSql, Conn);
            DataTable rs = new DataTable();
            mAdapter.Fill(rs);

            TableUserList = DataTableToModelList<Tabel_User>(rs);
            myCollection.Clear();

            foreach (var item in TableUserList)
            {
                myCollection.Add(item);
            }
            Conn.Close();
            */
        }


        /// <summary>
        /// DataTale转为实体列表
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List<T></returns>
        


    }
}
