using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendFinalProjectEduHome.Migrations
{
    public partial class AddedSecondAddressAndSecondWebsiteColumnToSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondAddress",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondWebsite",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondAddress",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SecondWebsite",
                table: "Settings");
        }
    }
}
