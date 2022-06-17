using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wordApiProject.Migrations
{
    public partial class updateUSer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasID",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HasID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
