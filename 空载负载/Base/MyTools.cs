using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.Windows.Markup;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;


using 空载负载.Model;
using 空载负载.Table;
using Application = Microsoft.Office.Interop.Excel.Application;
using Newtonsoft.Json;
using System.Text;

namespace 空载负载.Base
{
    public static class MyTools
    {
        static public Button[] UtiltyPageButton { get; set; } = new Button[10];


        static public Button[] InductionButton { get; set; } = new Button[10];

        static public object? m_MainViewModel { get; set; }

        public static ushort CRCCalc2(byte[] data)
        {
            ushort cmc = 0;
            byte temp;
            for (int i = 0; i < data.Length; i++)
            {
                temp = data[i];
                cmc = (ushort)(cmc + temp);
            }
            return cmc;
        }
        public static byte[] CRCCalc(byte[] data)
        {
            //1.预置1个16位的寄存器为十六进制FFFF(即全为1); 称此寄存器为CRC寄存器;
            //crc计算赋初始值
            var crc = 0xffff;
            for (var i = 0; i < data.Length; i++)
            {
                //2.把第一个8位二进制数据(既通讯信息帧的第一个字节)与16位的CRC寄存器的低8位相异或，把结果放于CRC寄存器;
                crc = crc ^ data[i]; //将八位数据与crc寄存器异或 ,异或的算法就是，两个二进制数的每一位进行比较，如果相同则为0，不同则为1

                //3.把CRC寄存器的内容右移一位(朝低位)用0填补最高位，并检查右移后的移出位;
                //4.如果移出位为0:重复第3步(再次右移一位); 如果移出位为1:CRC寄存器与多项式A001(1010 0000 0000 0001)进行异或;
                //5.重复步骤3和4，直到右移8次，这样整个8位数据全部进行了处理;
                for (var j = 0; j < 8; j++)
                {
                    int temp;
                    temp = crc & 1;
                    crc = crc >> 1;
                    crc = crc & 0x7fff;
                    if (temp == 1) crc = crc ^ 0xa001;
                    crc = crc & 0xffff;
                }
            }

            //CRC寄存器的高低位进行互换
            var crc16 = new byte[2];
            //CRC寄存器的高8位变成低8位，
            crc16[0] = (byte)((crc >> 8) & 0xff);
            //CRC寄存器的低8位变成高8位
            crc16[1] = (byte)(crc & 0xff);
            return crc16;
        }

        public static void  setUtiltyPageButton(bool isenable)
        {
            for(int i = 0;i < 10;i++)
            {
                UtiltyPageButton[i].IsEnabled = isenable;
            }
        }

        public static void setInductionButtonButton(bool isenable)
        {
            for (int i = 0; i < 10; i++)
            {
                InductionButton[i].IsEnabled = isenable;
            }
        }


        public static byte[] JsonSerializeToBytes(object obj)
        {
            if (obj == null) return null;
            string json = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(json);
        }

        // 反序列化
        public static T JsonDeserialize<T>(byte[] bytes)
        {
            string json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(json);
        }



        public static float[] ushortToFloat(ushort[] ushortValue)
        {
            float[] floats = new float[ushortValue.Length / 2];
            float floatValue1;
            byte[] _cbuffer = new byte[4];
            int k = 0;
            for (int i = 0; i < 9; i += 2, k++)
            {
                //Buffer.BlockCopy(ushortValue, i, _cbuffer, 0, 4);
                _cbuffer[0] = (byte)(ushortValue[i] >> 8); // 高位字节
                _cbuffer[1] = (byte)(ushortValue[i] & 0xFF); // 低位字节
                _cbuffer[2] = (byte)(ushortValue[i + 1] >> 8); // 高位字节
                _cbuffer[3] = (byte)(ushortValue[i + 1] & 0xFF); // 高位字节
                Array.Reverse(_cbuffer);
                floatValue1 = BitConverter.ToSingle(_cbuffer, 0);
                floats[k] = floatValue1;
            }
            return floats;
        }
        public static string RemoveLastPart(string input, char delimiter)
        {
            int lastIndex = input.LastIndexOf(delimiter);
            if (lastIndex == -1)
                return input; // 如果没有找到分隔符，则直接返回原字符串
            return input.Substring(0, lastIndex);
        }

        //设置当前产品信息

        //计算三相施加电压 额定电压..参数2 额定扎数 参数3 临时扎数(三相)
        public static float GetCalcAppliedVoltageThree(string RatedVoltage, float Edingzhashu_data, float Tempzhashu_data)
        {
            float result;
            bool success = float.TryParse(RatedVoltage, out result);
            if (success)
            {
                // 使用转换后的result
                //额定电压(KV) -- 变V                             
                float tempdata = (float)(result * 1000 / Edingzhashu_data);
                //必须保留3位.. 要不然就不对..
                tempdata = (float)Math.Round(tempdata, 3);

                // (临时渣叔)--每一扎电压.
                //这里就得到了 -- 施加电压.
                float Shijiazhashu_data = (float)(Tempzhashu_data * tempdata);
                Shijiazhashu_data = (float)Math.Round(Shijiazhashu_data, 3);

                return Shijiazhashu_data;
            }
            else
            {
                return -1;
            }
        }

        //计算单相施加电压 额定电压..参数2 额定扎数 参数3 临时扎数
        public static float GetCalcAppliedVoltageOne(string RatedVoltage, float Edingzhashu_data, float Tempzhashu_data)
        {
            float result;
            bool success = float.TryParse(RatedVoltage, out result);
            if (success)
            {
                // 使用转换后的result
                //额定电压(KV) -- 变V                             
                float tempdata = (float)(result * 1000 / Edingzhashu_data);
                //必须保留3位.. 要不然就不对..
                tempdata = (float)Math.Round(tempdata, 3);

                // (临时渣叔)--每一扎电压.
                //这里就得到了 -- 施加电压.
                float Shijiazhashu_data = (float)(Tempzhashu_data * tempdata);
                Shijiazhashu_data = (float)Math.Round(Shijiazhashu_data, 3);

                return Shijiazhashu_data;
            }
            else
            {
                return -1;
            }
        }


        //计算 额定电流(三相) 参数1 额定容量(产品)   参数2 施加电压        
        public static float GetCalcRatedCurrentThree(string ProductCapacity, float Shijiazhashu_data)
        {
            float result;
            bool success = float.TryParse(ProductCapacity, out result);
            //转换额定容量 * 1000
            if (success)
            {
                //计算额定电流(不显示-- )
                float NoloadCurrent = (float)(result * 1000 / Shijiazhashu_data / 1.732);
                NoloadCurrent = (float)Math.Round(NoloadCurrent, 3);
                return NoloadCurrent;
            }
            else
            {
                // 处理转换失败的情况                
                return -1;
            }
        }

        //计算空载电流(三相) 参数1 三相平均电流  参数2 三相平均值电压  参数3 施加电压
        public static float GetNoloadCurrentThree(float Irms, float Atpv, float Shijiadianya_data)
        {
            float result;
            result = (float)(Irms/(Atpv/Shijiadianya_data));
            return result;
        }

        //计算额定电流(单相) 参数1 额定容量(产品)   参数2 施加电压        
        public static float GetCalcRatedCurrentOne(string ProductCapacity, float Shijiazhashu_data)
        {
            float result;
            bool success = float.TryParse(ProductCapacity, out result);
            //转换额定容量 * 1000
            if (success)
            {
                //计算额定电流(不显示-- )
                float NoloadCurrent = (float)(result * 1000 / Shijiazhashu_data);
                NoloadCurrent = (float)Math.Round(NoloadCurrent, 3);



                return NoloadCurrent;
            }
            else
            {
                // 处理转换失败的情况                
                return -1;
            }
        }


        //计算空载电流%(三相) 参数1 空载电流.  参数2 额定电流      
        public static float GetCalcRatedCurrentPercentageThree(float NoloadCurrent2, float NoloadCurrent)
        {

            //计算 电流%.(显示 电流%)
            float PercentageNoloadCurrent = NoloadCurrent2 / NoloadCurrent * 100;
            return PercentageNoloadCurrent = (float)Math.Round(PercentageNoloadCurrent, 3);

        }

        //计算空载损耗(三相) 参数1 三相功率.  参数2 电压平均值  参数3 施加电压        
        public static float GetCalcNoloadlossThree(float Pabbcca, float ATPVoltage, float Shijiazhashu_data)
        {
            //计算 空载损耗.
            //float NoloadLoss = (float)(Pabbcca * Math.Pow(ATPVoltage / Shijiazhashu_data, 2));
            float NoloadLoss = (float)(Pabbcca * Math.Pow(Shijiazhashu_data / ATPVoltage, 2));
            return NoloadLoss = (float)Math.Round(NoloadLoss, 3);
        }




        //计算不足800V的产品 -- 600V模拟数据 -- 得到合格不合格结果.
        //1..  --- 2.. -- .. --4 %   
        //(上 8 电流标准  9  电流标准（上）10 电流标准（下）


        public static float GetResultNoloadLoss(float NoloadLoss,//实际空载损耗            
            float ATPVoltage, //平均电压            
            float Shijiazhashu_data) // 施加电压
                                     //string RatedVoltage) //额定电压  
        {
            //float fRatedVoltage; if (!float.TryParse(RatedVoltage, out fRatedVoltage)) return -1;
            //float fNoloadLoss = (float)(NoloadLoss * Math.Pow(((fRatedVoltage * 1000) / Shijiazhashu_data), 2)); 

            float fNoloadLoss = (float)(NoloadLoss * Math.Pow((Shijiazhashu_data / ATPVoltage), 2));
            return fNoloadLoss;
        }



        public static int GetResultThree(float NoloadLoss, //实际空载损耗
            float fPercentageNoloadCurrent,
            string ProductStandard, //空载标准
            string ProductStandardUpperimit,//空载标准上
            string ProductStandardDownimit,//空载标准下
            string ProductCurrentStandard,//电流标准
            string ProductCurrentStandardUpperrimit,//电流标准上
            string ProductCurrentStandardDownimit) //电流标准下
        {
            float fProductStandard; if (!float.TryParse(ProductStandard, out fProductStandard)) return -1;
            float fProductStandardUpperimit; if (!float.TryParse(ProductStandardUpperimit, out fProductStandardUpperimit)) return -1;
            //float fProductStandardDownimit; if (!float.TryParse(ProductStandardDownimit, out fProductStandardDownimit)) return -1;
            float fProductCurrentStandard; if (!float.TryParse(ProductCurrentStandard, out fProductCurrentStandard)) return -1;
            float fProductCurrentStandardUpperrimit; if (!float.TryParse(ProductCurrentStandardUpperrimit, out fProductCurrentStandardUpperrimit)) return -1;
            //float fProductCurrentStandardDownimit; if (!float.TryParse(ProductCurrentStandardDownimit, out fProductCurrentStandardDownimit)) return -1;

            float fNoloadLoss = NoloadLoss;
            //这里只需要上限..
            fProductStandard = fProductStandard + (fProductStandard / fProductStandardUpperimit);
            fProductCurrentStandard = fProductCurrentStandard + (fProductCurrentStandard / fProductCurrentStandardUpperrimit);

            if (fNoloadLoss < fProductStandard && fPercentageNoloadCurrent < fProductCurrentStandard)
                return 1;
            else return 0;
        }

    }


    public static class VisualTreeHelpers
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static System.Windows.Controls.Button ScanButtonFromStackPanel(StackPanel stackPanel, string buttonName)
        {
            foreach (var child in VisualTreeHelpers.FindVisualChildren<UIElement>(stackPanel))
            {
                //得到开始测量按钮
                if (child.GetType().Name == "Button")
                {
                    System.Windows.Controls.Button button = child as System.Windows.Controls.Button;
                    if (button.Name == buttonName)
                    {
                        //child.IsEnabled = false;
                        return button;
                    }
                }
            }
            return null;

        }
    }


    public static class DataGridExtensions
    {
        public static System.Data.DataTable ToDataTable(this DataGrid dataGrid)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();

            // 添加列
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                dataTable.Columns.Add(dataGrid.Columns[i].Header.ToString(), typeof(string));
            }

            // 添加行
            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    //这里应该是内容呀..??
                    //string sstringContent = dataGrid.Items[i][j];
                    //var cellContent = dataGrid.Columns[j][i].Content;
                    var cellContent = dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]);
                    if (cellContent != null && cellContent.DataContext != null)
                    {
                        dataRow[j] = cellContent.DataContext.ToString();
                    }
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }

    public class MyMessage {
        public string Message { get; set; }
        public object obj{ get; set; }
        public string Search { get; set; }
        public int SearchIndex { get; set; }
    }
}
