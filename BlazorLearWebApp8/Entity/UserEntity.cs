using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BlazorLearWebApp8.Components.Attributes;
using FreeSql;
using FreeSql.DataAnnotations;
using MiniExcelLibs.Attributes;

namespace BlazorLearWebApp8.Entity
{
    [Description("用户信息表")]
    public class UserEntity : BaseEntity<UserEntity,int>
    {
        [ExcelColumn(Name = "用户名")]
        [Description("用户名")]
        [Required(ErrorMessage ="用户不能为空")]
        [User(ErrorMessage= "用户名不能重复")]
        //现在已经有了4个字段了，ID -- UPDATE -- timer -- 软删除..
        public string? UserName { get; set; }

        [DisplayName("密码")]
        [Description("密码")]
        public string? Password { get; set; }


        [DisplayName("显示")]
        [Description("显示名称")]
        public string? NickName { get; set; }

        [Description("角色ID")]
        public int RoleId { get; set; }

        //什么意思？一个用户表，一个用户，只能设置一个角色..
        //Role这个，是通过RoleId来得到的..
        //一对一..(一个RoleId 对应一个 Role角色..)
        //现在如果要添加一个用户，那么，会自动去查 ROLEID的...
        [Navigate(nameof(RoleId))]
        public RoleEntity? Role { get; set; }
    }

}
