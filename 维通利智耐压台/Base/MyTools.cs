using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LiveCharts.Wpf.Charts.Base;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using 维通利智耐压台.MyTable;
using CommunityToolkit.Mvvm.Messaging;
using LiveCharts.Wpf;
using Syncfusion.Windows.Shared;
using Xceed.Words.NET;

namespace 维通利智耐压台.Base
{
    public static class MyTools
    {
        public static Table_Product GetTableProcduct(string ProductNumber)
        {
            try
            {
                List<Table_Product> TableUserList = null;
                //按字段号添加数据，这个非常有用，网上的都是很一长串添加，此方法可以单个字段添加数据。            
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //@来避免转义
                string dbPath = @"Data Source=" + exeDirectory + "mySql3.db;Version=3";
                string strSql = string.Format("select * from Procduct where ProductNumber = ('{0}')", ProductNumber);

                SQLiteConnection Conn = new SQLiteConnection(dbPath);
                Conn.Open();
                SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(strSql, Conn);
                DataTable rs = new DataTable();
                mAdapter.Fill(rs);
                Conn.Close();
                TableUserList = DataTableToModelList<Table_Product>(rs);
                foreach (var item in TableUserList)
                {
                    return item;
                }                
                return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库操作异常:" + ex.Message);
                return null;
            }

            return null;
        }
        public static int AddTableProcduct(string ProductNumber, string ProductType, string ProductTuhao, string ProductCapacity,string RatedVoltage)
        {
            try
            {
                //按字段号添加数据，这个非常有用，网上的都是很一长串添加，此方法可以单个字段添加数据。            
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //@来避免转义
                string dbPath = @"Data Source=" + exeDirectory + "mySql3.db;Version=3";
                string strSql = string.Format("INSERT INTO Procduct (ProductNumber,ProductType,ProductTuhao,ProductCapacity,RatedVoltage) VALUES ('{0}','{1}','{2}','{3}','{4}')", ProductNumber, ProductType, ProductTuhao, ProductCapacity, RatedVoltage);

                SQLiteConnection Conn = new SQLiteConnection(dbPath);
                Conn.Open();
                SQLiteCommand command = new SQLiteCommand(strSql, Conn);
                int result = command.ExecuteNonQuery();                
                Conn.Close();
                return result;

            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库操作异常:" + ex.Message);
                return -1;
            }
        }

        public static List<Table_Product> GetTableProductAllNumber()
        {
            try
            {
                //D:\2022\now\维通利智耐压台\bin\Debug\net5.0-windows\
                
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //@来避免转义
                string dbPath = @"Data Source=" + exeDirectory + "mySql3.db;Version=3";
                string strSql = "select * from Procduct";
                SQLiteConnection Conn = new SQLiteConnection(dbPath);
                Conn.Open();

                SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(strSql, Conn);
                DataTable rs = new DataTable();
                mAdapter.Fill(rs);
                
                //myCollection.Clear();
                /*
                foreach (var item in TableUserList)
                {
                    myCollection.Add(item);
                }
                */

                Conn.Close();

                return DataTableToModelList<Table_Product>(rs);
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库操作异常:" + ex.Message);
                return null;
            }

        }

        public static void EditTableUser(Tabel_User tabel_User)
        {
            try
            {
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;                
                string dbPath = @"Data Source=" + exeDirectory + "mySql3.db;Version=3";
                string strSql = string.Format("UPDATE User SET Username ='{0}',Password='{1}',Power='{2}' WHERE Id ='{3}'", tabel_User.Username, tabel_User.Password, tabel_User.Power, tabel_User.Id);                
                SQLiteConnection Conn = new SQLiteConnection(dbPath);
                Conn.Open();
                SQLiteCommand command = new SQLiteCommand(strSql, Conn);                
                command.ExecuteNonQuery();
                Conn.Close();
                MessageBox.Show("数据修改完成!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库操作异常:" + ex.Message);
                return;
            }
        }

        public static void DeleteTableUser(int Id)
        {
            try
            {
                //按条件删除行
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //@来避免转义
                string dbPath = @"Data Source=" + exeDirectory + "mySql3.db;Version=3";

                string strSql = "delete from User where Id='" + Id + "'";
                SQLiteConnection Conn = new SQLiteConnection(dbPath);
                Conn.Open();

                SQLiteCommand command = new SQLiteCommand(strSql, Conn);
                if (command.ExecuteNonQuery() == 1) MessageBox.Show("删除成功!");
                else MessageBox.Show("删除失败!");

                Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库操作异常:" + ex.Message);
                return;
            }
        }

        public static void AddTableUser(string username,string password,string power)
        {
            try
            {
                //按字段号添加数据，这个非常有用，网上的都是很一长串添加，此方法可以单个字段添加数据。            
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //@来避免转义
                string dbPath = @"Data Source=" + exeDirectory + "mySql3.db;Version=3";
                string strSql = string.Format("INSERT INTO User (Username,Password,Power) VALUES ('{0}','{1}','{2}')", username, password, power);
                
                SQLiteConnection Conn = new SQLiteConnection(dbPath);
                Conn.Open();
                SQLiteCommand command = new SQLiteCommand(strSql, Conn);
                if (command.ExecuteNonQuery() == 1) MessageBox.Show("添加成功!");
                else MessageBox.Show("添加失败!");
                Conn.Close();                

            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库操作异常:" + ex.Message);
                return;
            }
        }

        public static void UpateTableUser(ObservableCollection<Tabel_User> myCollection)
        {
            try
            {
                //D:\2022\now\维通利智耐压台\bin\Debug\net5.0-windows\
                List<Tabel_User> TableUserList = null;
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
            }
            catch(Exception ex)
            {
                MessageBox.Show("数据库操作异常:" + ex.Message);
                return;
            }
        }

        public static List<T> DataTableToModelList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        if (tempName == "Id")
                        {
                            long value = (long)row[tempName];
                            pro.SetValue(t, value, null);
                        }
                        else
                        {
                            object value = row[tempName].ToString();
                            if (value.GetType() == typeof(System.DBNull))
                            {
                                value = null;
                            }
                            pro.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }

        public static string SaveDataToBmp(CartesianChart chart)
        {
            //MyMessage myMessage = new MyMessage();
            //myMessage.Message = "GetQueryChart";
            //WeakReferenceMessenger.Default.Send(myMessage);
            //CartesianChart chart = (CartesianChart)myMessage.obj1;

            // 假设你的 CartesianChart 控件名为 chart
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)chart.ActualWidth, (int)chart.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(chart);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            //如果就这样去写，就是和EXE在一个目录下面..
            //WINDOWS保持不能是: 这里格式化后HH mm ss 就是 时分秒...: 变成了 _
            //string date = DateTime.Now.ToString("current");
            //string savepng = string.Format("D://{0}.png", date);

            //这里只能定死了..
            string savepng = @"D:\NaiYa\current.png";

            using (FileStream stream = File.Create(savepng))
            {
                encoder.Save(stream);
            }

            return savepng;

        }


        //保存数据到数据库.
        public static bool SaveRecordDataToDataBase(Tabe_Record tabe_Record)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    Tabe_Record table_RocreadInfo = new Tabe_Record();
                    table_RocreadInfo.Id = 0; //(ID 产品表)
                    table_RocreadInfo.ProductName = tabe_Record.ProductName;
                    table_RocreadInfo.ProductNumber = tabe_Record.ProductNumber;
                    //产品型号
                    table_RocreadInfo.ProductType = tabe_Record.ProductType;
                    //图号
                    table_RocreadInfo.ProductTuhao = tabe_Record.ProductTuhao;
                    //标准电压
                    table_RocreadInfo.ProductStardVotil = tabe_Record.ProductStardVotil;
                    //标准局放
                    table_RocreadInfo.ProductStardPartial = tabe_Record.ProductStardPartial;
                    //开始日期..
                    table_RocreadInfo.RecordDateTimer = tabe_Record.RecordDateTimer;
                    //加压部位
                    table_RocreadInfo.ProductParts = tabe_Record.ProductParts;
                    //开始记时1
                    table_RocreadInfo.BeginTimer = tabe_Record.BeginTimer;
                    //电压1
                    table_RocreadInfo.Votil = tabe_Record.Votil;
                    //局放1
                    table_RocreadInfo.Partial = tabe_Record.Partial;
                    table_RocreadInfo.LevelCode = tabe_Record.LevelCode;
                    //开始记时2



                    context.naiya_record.Add(table_RocreadInfo);

                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }

    public class MyMessage
    {
        public string Message { get; set; }
        public object obj1 { get; set; }
        public object obj2 { get; set; }
        public float fobj1 { get; set; } = 0.0f;
        public float fobj2 { get; set; } = 0.0f;

        public int SearchIndex { get; set; }
        public string Search { get; set; }
    }


    public class MyValidateAttribute : ValidationAttribute
    {
        //属性MaxAge和MinAge，将通过特性参数赋值
        public float MaxAge { get; }
        public float MinAge { get; }
        public string MyErrorMessage { get; }
        public MyValidateAttribute(float maxAge, float minAge)
        {
            MaxAge = maxAge;
            MinAge = minAge;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            float age = (float)value;
            if (age >= MinAge && age <= MaxAge)
            {
                return ValidationResult.Success;
            }
            return new(ErrorMessage);

        }
    }



    public class MyIntValidateAttribute : ValidationAttribute
    {
        //属性MaxAge和MinAge，将通过特性参数赋值
        public int MaxAge { get; }
        public int MinAge { get; }
        public string MyErrorMessage { get; }
        public MyIntValidateAttribute(int maxAge, int minAge)
        {
            MaxAge = maxAge;
            MinAge = minAge;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int age = (int)value;
            if (age >= MinAge && age <= MaxAge)
            {
                return ValidationResult.Success;
            }
            return new(ErrorMessage);

        }
    }

}
