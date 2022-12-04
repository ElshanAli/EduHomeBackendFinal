using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendFinalProjectEduHome.Migrations
{
    public partial class AddedSomeColumnToSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdressImage",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneImage",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WebsiteImage",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdressImage",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PhoneImage",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "WebsiteImage",
                table: "Settings");
        }
    }
}
