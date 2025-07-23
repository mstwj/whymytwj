using Microsoft.EntityFrameworkCore.Migrations;

namespace 服务端访问数据类库.Migrations
{
    public partial class A3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "menu_icon",
                table: "menus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "menu_icon",
                table: "menus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
