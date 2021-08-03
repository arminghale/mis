using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Form",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    formid = table.Column<int>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    isnull = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.id);
                    table.ForeignKey(
                        name: "FK_Field_Form_formid",
                        column: x => x.formid,
                        principalTable: "Form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubForm",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    form1id = table.Column<int>(nullable: false),
                    form2id = table.Column<int>(nullable: false),
                    type = table.Column<int>(nullable: false),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubForm", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubForm_Form_form1id",
                        column: x => x.form1id,
                        principalTable: "Form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubForm_Form_form2id",
                        column: x => x.form2id,
                        principalTable: "Form",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "SubField",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fieldid = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubField", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubField_Field_fieldid",
                        column: x => x.fieldid,
                        principalTable: "Field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Field_formid",
                table: "Field",
                column: "formid");

            migrationBuilder.CreateIndex(
                name: "IX_SubField_fieldid",
                table: "SubField",
                column: "fieldid");

            migrationBuilder.CreateIndex(
                name: "IX_SubForm_form1id",
                table: "SubForm",
                column: "form1id");

            migrationBuilder.CreateIndex(
                name: "IX_SubForm_form2id",
                table: "SubForm",
                column: "form2id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubField");

            migrationBuilder.DropTable(
                name: "SubForm");

            migrationBuilder.DropTable(
                name: "Field");

            migrationBuilder.DropTable(
                name: "Form");
        }
    }
}
