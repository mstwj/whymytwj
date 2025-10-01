using FreeSql;
using FreeSql.DataAnnotations;

namespace BlazorLearWebApp8.Entity
{
    public class MenuEntity:BaseEntity<MenuEntity,int>
    {
        public string? MenuName { get; set; }
        public string? Url { get; set; }

        public string? Icon { get; set; }
        public int ParentId { get; set; }

        [Navigate(nameof(ParentId))]
        public MenuEntity? Parent { get; set; }

        [Navigate(nameof(ParentId))]
        public List<MenuEntity> Children { get; set; }



        [Navigate(ManyToMany = typeof(RoleMenuEntity))]
        public List<RoleEntity>? Roles { get; set; }
    }
}
