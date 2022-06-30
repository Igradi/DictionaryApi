using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wordApiProject.Migrations
{
    public partial class banacc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BanExpires",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BanExpires",
                table: "Users");
        }
    }
}
