using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ActivityTrackerAPI.Migrations
{
    public partial class InitialBuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityType",
                columns: table => new
                {
                    ActivityTypeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityType", x => x.ActivityTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivityTypeId = table.Column<int>(nullable: false),
                    Distance = table.Column<int>(nullable: false),
                    FirebaseLocation = table.Column<string>(nullable: false),
                    Pace = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_ActivityType_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityType",
                        principalColumn: "ActivityTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    NoteId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActivityId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Note_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ActivityTypeId",
                table: "Activity",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_ActivityId",
                table: "Note",
                column: "ActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "ActivityType");
        }
    }
}
