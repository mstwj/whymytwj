using Microsoft.EntityFrameworkCore.Migrations;

namespace 服务端访问数据类库.Migrations
{
    public partial class A4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_desc_info",
                columns: table => new
                {
                    _id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_age = table.Column<int>(type: "int", nullable: false),
                    user_real_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_desc_info", x => x._id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_desc_info");
        }
    }
}
