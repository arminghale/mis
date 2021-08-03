using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SubDomainValue_DomainValue_DomainValue2id",
            //    table: "SubDomainValue");

            //migrationBuilder.DropIndex(
            //    name: "IX_SubDomainValue_DomainValue2id",
            //    table: "SubDomainValue");

            //migrationBuilder.DropColumn(
            //    name: "DomainValue2id",
            //    table: "SubDomainValue");

            migrationBuilder.AddColumn<int>(
                name: "ParentUserid",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "parentid",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserForms",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<int>(nullable: false),
                    formid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserForms", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserForms_Form_formid",
                        column: x => x.formid,
                        principalTable: "Form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserForms_User_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_ParentUserid",
                table: "User",
                column: "ParentUserid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SubDomainValue_domainvalueid",
            //    table: "SubDomainValue",
            //    column: "domainvalueid");

            migrationBuilder.CreateIndex(
                name: "IX_UserForms_formid",
                table: "UserForms",
                column: "formid");

            migrationBuilder.CreateIndex(
                name: "IX_UserForms_userid",
                table: "UserForms",
                column: "userid");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SubDomainValue_DomainValue_domainvalueid",
            //    table: "SubDomainValue",
            //    column: "domainvalueid",
            //    principalTable: "DomainValue",
            //    principalColumn: "id",
            //    onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_ParentUserid",
                table: "User",
                column: "ParentUserid",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubDomainValue_DomainValue_domainvalueid",
                table: "SubDomainValue");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_ParentUserid",
                table: "User");

            migrationBuilder.DropTable(
                name: "UserForms");

            migrationBuilder.DropIndex(
                name: "IX_User_ParentUserid",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_SubDomainValue_domainvalueid",
                table: "SubDomainValue");

            migrationBuilder.DropColumn(
                name: "ParentUserid",
                table: "User");

            migrationBuilder.DropColumn(
                name: "parentid",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "DomainValue2id",
                table: "SubDomainValue",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubDomainValue_DomainValue2id",
                table: "SubDomainValue",
                column: "DomainValue2id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubDomainValue_DomainValue_DomainValue2id",
                table: "SubDomainValue",
                column: "DomainValue2id",
                principalTable: "DomainValue",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
