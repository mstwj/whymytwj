using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using 维通利智耐压台.Base;

namespace 维通利智耐压台.Model
{
    public class UserTestControlModel : ObservableValidator
    {
        //这里系统验证只能对于string来说.. 对于int 要自己去写验证..
        private float textlevel1volte = 0;
        [MyValidateAttribute(20f, 0, ErrorMessage = "阶段1最大电压20kV,最小0")]
        public float TextLevel1Volte { get => textlevel1volte; set { SetProperty(ref textlevel1volte, value,true); } }

        private int textlevel1time = 0;
        [MyIntValidateAttribute(120, 10, ErrorMessage = "阶段1最大加压时间120s,最小10s")]
        public int TextLevel1Time { get => textlevel1time; set { SetProperty(ref textlevel1time, value, true); } }


        private float textleve12volte = 0;
        [MyValidateAttribute(20f, 0, ErrorMessage = "阶段2最大电压20kV,最小0")]
        public float TextLevel2Volte { get => textleve12volte; set { SetProperty(ref textleve12volte, value, true); } }

        private int textleve12time = 0;
        [MyIntValidateAttribute(120, 0, ErrorMessage = "阶段2最大加压时间120s,最小0s")]
        public int TextLevel2Time { get => textleve12time; set { SetProperty(ref textleve12time, value, true); } }


        private float textlevel3volte = 0;
        [MyValidateAttribute(20f, 0, ErrorMessage = "阶段3最大电压20kV,最小0")]
        public float TextLevel3Volte { get => textlevel3volte; set { SetProperty(ref textlevel3volte, value, true); } }

        private int textleve13time = 0;
        [MyIntValidateAttribute(120, 0, ErrorMessage = "阶段3最大加压时间120s,最小0s")]
        public int TextLevel3Time { get => textleve13time; set { SetProperty(ref textleve13time, value, true); } }


        private float gaoyaproctectvolte = 20.0f;
        [MyValidateAttribute(20.0f, 0, ErrorMessage = "高压保护电压最大20kV,最小0")]
        public float GaoYaProctectVolte { get => gaoyaproctectvolte; set { SetProperty(ref gaoyaproctectvolte, value, true); } }


        private float diyaproctectcurrent = 150;
        [MyValidateAttribute(150, 0, ErrorMessage = "保护电流150mA,最小0")]
        public float DiYaProctectCurrent { get => diyaproctectcurrent; set { SetProperty(ref diyaproctectcurrent, value, true); } }


        public bool Check1 { get; set; } 
        public bool Check2 { get; set; }
        public bool Check3 { get; set; }

        public bool ValideAll()
        {
            ValidateAllProperties();

            if (Check1 == false)
            {
                MessageBox.Show("必须勾选阶段1");
                return false ;
            }

            if (Check2 == false && Check3 == true)
            {
                MessageBox.Show("阶段2没有勾选,就不能勾选阶段3");
                return false;
            }

            if (Check1 == true)
            {
                if (TextLevel1Volte < 1 || TextLevel1Time < 10.0)
                {
                    MessageBox.Show("阶段1设置错误,时间最小10秒,电压最小1kV");
                    return false;
                }
            }

            if (Check2 == true)
            {
                if (TextLevel2Volte < 1 || TextLevel2Time < 10.0)
                {
                    MessageBox.Show("阶段2设置错误,时间最小10秒,电压最小1kV");
                    return false;
                }
            }

            if (Check3 == true)
            {
                if (TextLevel3Volte < 1 || TextLevel3Time < 10.0)
                {
                    MessageBox.Show("阶段3设置错误,电压最小1kV,时间最小10秒");
                    return false;
                }
            }


            if (HasErrors)
            {
                string AllErrorMsg = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
                MessageBox.Show(AllErrorMsg);
                return false;
            }

            return true;

        }
    }
}
