using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "radif",
                table: "SubForm",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "radif",
                table: "Field",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "radif",
                table: "SubForm");

            migrationBuilder.DropColumn(
                name: "radif",
                table: "Field");
        }
    }
}
