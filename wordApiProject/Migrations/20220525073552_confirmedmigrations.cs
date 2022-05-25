using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wordApiProject.Migrations
{
    public partial class confirmedmigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "confirmedpass",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmedpass",
                table: "Users");
        }
    }
}
