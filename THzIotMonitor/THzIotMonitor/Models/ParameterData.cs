using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace THzIotPlatform.Models
{
    public class ParameterData
    {
        [Key]
        public int Id { get; set; }  // 编号，自增

        [Display(Name = "参数名称")]
        public string? ParameterName { get; set; }

        [Display(Name = "SN")]
        public string? SN { get; set; }  // 设备编码+“_”+访问地址

        [Display(Name = "ParameterId")]
        public int ParameterId { get; set; }  // 参数编号



        [Display(Name = "t")]
        public string? T { get; set; }

        [Display(Name = "单位")]
        public string? Unit { get; set; }

        [Display(Name = "数值")]
        public decimal Value { get; set; }

        [Display(Name = "时间")]
        public DateTime Time { get; set; }
    }
}
