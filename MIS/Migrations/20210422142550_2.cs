using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<bool>(
                name: "type",
                table: "UACC",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "type",
                table: "UACC",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.CreateTable(
                name: "Access",
                columns: table => new
                {
                    actiongroupid = table.Column<int>(type: "int", nullable: false),
                    actionid = table.Column<int>(type: "int", nullable: false),
                    samaneid = table.Column<int>(type: "int", nullable: false),
                    userid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }
    }
}
