using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clockify.Tracking.Data.Migrations
{
    public partial class WorkedTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkedTime",
                table: "DayEntries",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkedTime",
                table: "DayEntries");
        }
    }
}
