using System.ComponentModel.DataAnnotations;

namespace IsWorkabing.Models
{
    public class test1
    {
        [Key]
        public int Id { get; set; }  // 编号，自增


        [Display(Name = "A电压")]
        public double Avoltage { get; set; }

        [Display(Name = "B电压")]
        public double Bvoltage { get; set; }

        [Display(Name = "C电压")]
        public double Cvoltage { get; set; }

        [Display(Name = "A电流")]
        public double Aelectric { get; set; }

        [Display(Name = "B电流")]
        public double Belectric { get; set; }

        [Display(Name = "C电流")]
        public double Celectric { get; set; }

        [Display(Name = "A功率")]
        public double Aactivepower { get; set; }

        [Display(Name = "B功率")]
        public double Bactivepower { get; set; }

        [Display(Name = "C功率")]
        public double Cactivepower { get; set; }
        

    }
}
