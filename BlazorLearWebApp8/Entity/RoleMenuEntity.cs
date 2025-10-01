using System.ComponentModel.DataAnnotations.Schema;
using FreeSql.DataAnnotations;

namespace BlazorLearWebApp8.Entity
{
    public class RoleMenuEntity
    {
        [FreeSql.DataAnnotations.Column(IsPrimary = true)]
        public int RoleId { get; set; }

        [FreeSql.DataAnnotations.Column(IsPrimary = true)]
        public int MenuId { get; set; }
    }
}
