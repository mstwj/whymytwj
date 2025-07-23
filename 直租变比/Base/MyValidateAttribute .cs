using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;


namespace 直租变比.Base
{
    
    
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
