using Microsoft.EntityFrameworkCore.Migrations;

namespace Uni.Api.DataAccess.Migrations.UniDb
{
    public partial class AddedLessonType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LessonType",
                table: "Schedule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonType",
                table: "Schedule");
        }
    }
}
