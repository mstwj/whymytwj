using Microsoft.EntityFrameworkCore.Migrations;

namespace 服务端访问数据类库.Migrations
{
    public partial class A2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "menus",
                columns: table => new
                {
                    menu_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    menu_header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_view = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parent_id = table.Column<int>(type: "int", nullable: false),
                    menu_icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    index = table.Column<int>(type: "int", nullable: false),
                    menu_type = table.Column<int>(type: "int", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menus", x => x.menu_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "menus");
        }
    }
}
