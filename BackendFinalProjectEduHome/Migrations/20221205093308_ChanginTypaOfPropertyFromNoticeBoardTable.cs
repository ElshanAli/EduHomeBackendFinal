using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendFinalProjectEduHome.Migrations
{
    public partial class ChanginTypaOfPropertyFromNoticeBoardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoticeTitle",
                table: "NoticeBoards");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "NoticeBoards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "NoticeBoards");

            migrationBuilder.AddColumn<string>(
                name: "NoticeTitle",
                table: "NoticeBoards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
