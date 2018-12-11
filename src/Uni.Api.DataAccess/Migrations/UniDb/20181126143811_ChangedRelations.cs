using Microsoft.EntityFrameworkCore.Migrations;

namespace Uni.Api.DataAccess.Migrations.UniDb
{
    public partial class ChangedRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Teacher",
                table: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Subject_TeacherId",
                table: "Subject");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Subject",
                newName: "ScheduleId");

            migrationBuilder.AddColumn<int>(
                name: "AudienceNumber",
                table: "Schedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Schedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_TeacherId",
                table: "Schedule",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Teacher",
                table: "Schedule",
                column: "TeacherId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Teacher",
                table: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_TeacherId",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "AudienceNumber",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Schedule");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "Subject",
                newName: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_TeacherId",
                table: "Subject",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Teacher",
                table: "Subject",
                column: "TeacherId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
