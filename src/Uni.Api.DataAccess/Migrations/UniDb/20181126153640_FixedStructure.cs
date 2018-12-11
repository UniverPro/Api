using Microsoft.EntityFrameworkCore.Migrations;

namespace Uni.Api.DataAccess.Migrations.UniDb
{
    public partial class FixedStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Subject",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Subject");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_SubjectId",
                table: "Schedule",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Subject",
                table: "Schedule",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Subject",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_SubjectId",
                table: "Schedule");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Subject",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Subject",
                table: "Schedule",
                column: "Id",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
