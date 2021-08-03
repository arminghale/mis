using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Samane",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "ActionGroup",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "url",
                table: "Action",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Action",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Domain",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(nullable: false),
                    IsAccess = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domain", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DomainValue",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    domainid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainValue", x => x.id);
                    table.ForeignKey(
                        name: "FK_DomainValue_Domain_domainid",
                        column: x => x.domainid,
                        principalTable: "Domain",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubDomainValue",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    domainvalueid = table.Column<int>(nullable: false),
                    domainvalue2id = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDomainValue", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubDomainValue_DomainValue_domainvalueid",
                        column: x => x.domainvalueid,
                        principalTable: "DomainValue",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubDomainValue_DomainValue_domainvalue2id",
                        column: x => x.domainvalue2id,
                        principalTable: "DomainValue",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UACCDomain",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<int>(nullable: false),
                    domainvalueid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UACCDomain", x => x.id);
                    table.ForeignKey(
                        name: "FK_UACCDomain_DomainValue_domainvalueid",
                        column: x => x.domainvalueid,
                        principalTable: "DomainValue",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UACCDomain_User_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DomainValue_domainid",
                table: "DomainValue",
                column: "domainid");

            migrationBuilder.CreateIndex(
                name: "IX_SubDomainValue_DomainValue2id",
                table: "SubDomainValue",
                column: "DomainValue2id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SubDomainValue_domainvalue2id",
            //    table: "SubDomainValue",
            //    column: "domainvalue2id");

            migrationBuilder.CreateIndex(
                name: "IX_UACCDomain_domainvalueid",
                table: "UACCDomain",
                column: "domainvalueid");

            migrationBuilder.CreateIndex(
                name: "IX_UACCDomain_userid",
                table: "UACCDomain",
                column: "userid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubDomainValue");

            migrationBuilder.DropTable(
                name: "UACCDomain");

            migrationBuilder.DropTable(
                name: "DomainValue");

            migrationBuilder.DropTable(
                name: "Domain");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Samane",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "ActionGroup",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "url",
                table: "Action",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Action",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
