using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clockify.Tracking.Data.Migrations
{
    public partial class Tracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    LunchTime = table.Column<TimeSpan>(nullable: false),
                    ToleranceTime = table.Column<TimeSpan>(nullable: false),
                    WorkingTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ExtraTime = table.Column<TimeSpan>(nullable: false),
                    MissingTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DayId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeEntries_DayEntries_DayId",
                        column: x => x.DayId,
                        principalTable: "DayEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_DayId",
                table: "TimeEntries",
                column: "DayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropTable(
                name: "TimeEntries");

            migrationBuilder.DropTable(
                name: "DayEntries");
        }
    }
}
