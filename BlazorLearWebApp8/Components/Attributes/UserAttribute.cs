using System.ComponentModel.DataAnnotations;
using BlazorLearWebApp8.Entity;

namespace BlazorLearWebApp8.Components.Attributes
{
    //用户已经存在..
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class UserAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not string str)
            {
                return false;
            }

            var user = UserEntity.Where(x => x.UserName == str).First();

            return user == null;
        }
    }
}
