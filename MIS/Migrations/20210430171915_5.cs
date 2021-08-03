using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RACCDomain",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleid = table.Column<int>(nullable: false),
                    domainvalueid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RACCDomain", x => x.id);
                    table.ForeignKey(
                        name: "FK_RACCDomain_DomainValue_domainvalueid",
                        column: x => x.domainvalueid,
                        principalTable: "DomainValue",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RACCDomain_Role_roleid",
                        column: x => x.roleid,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RACCDomain_domainvalueid",
                table: "RACCDomain",
                column: "domainvalueid");

            migrationBuilder.CreateIndex(
                name: "IX_RACCDomain_roleid",
                table: "RACCDomain",
                column: "roleid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RACCDomain");
        }
    }
}
