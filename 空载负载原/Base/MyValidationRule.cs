using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace 空载_负载.Base
{
    public class MyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;
            if (string.IsNullOrEmpty(input) || input.Length < 1)
            {
                return new ValidationResult(false, "输入必须至少1个字符");
            }
            return new ValidationResult(true, null);
        }
    }
}
