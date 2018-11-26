using Microsoft.EntityFrameworkCore.Migrations;

namespace Uni.DataAccess.Migrations.UniDb
{
    public partial class AudienceNumberTypeShouldBeString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AudienceNumber",
                table: "Schedule",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AudienceNumber",
                table: "Schedule",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
