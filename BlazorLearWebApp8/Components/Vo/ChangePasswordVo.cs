using System.ComponentModel.DataAnnotations;

namespace BlazorLearWebApp8.Components.Vo
{
    public class ChangePasswordVo
    {
        [Required(ErrorMessage ="旧密码不能空")]
        public string? OldPassword { get; set; }


        [Required(ErrorMessage = "密码不能空")]
        public string? NewPassword { get; set; }


    }
}
