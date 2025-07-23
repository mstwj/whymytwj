using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;


namespace 铁芯实验.Base
{

    

    public class MyValidateAttributeString : ValidationAttribute
    {
        //属性MaxAge和MinAge，将通过特性参数赋值
        public string MaxAge { get; }
        public string MinAge { get; }
        public string MyErrorMessage { get; }
        public MyValidateAttributeString(string maxAge, string minAge)
        {
            MaxAge = maxAge;
            MinAge = minAge;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string age = (string)value;
            float result1,result2,age3;
            if (!float.TryParse(MaxAge, out result1)) return new(ErrorMessage);            
            if (!float.TryParse(MinAge, out result2)) return new(ErrorMessage);            
            if (!float.TryParse(age, out age3)) return new(ErrorMessage);

            if (age3 >= result2 && age3 <= result1)
            {
                return ValidationResult.Success;
            }
            return new(ErrorMessage);

        }
    }


    public class MyValidateAttribute : ValidationAttribute
    {
        //属性MaxAge和MinAge，将通过特性参数赋值
        public float MaxAge { get; }
        public float MinAge { get; }
        public string MyErrorMessage { get; }
        public MyValidateAttribute(int maxAge, int minAge)
        {
            MaxAge = maxAge;
            MinAge = minAge;
            //MyErrorMessage = Message;
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

}
