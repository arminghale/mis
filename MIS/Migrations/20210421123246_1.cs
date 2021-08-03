using Microsoft.EntityFrameworkCore.Migrations;

namespace MIS.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Access",
                columns: table => new
                {
                    userid = table.Column<int>(nullable: false),
                    actionid = table.Column<int>(nullable: false),
                    actiongroupid = table.Column<int>(nullable: false),
                    samaneid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Samane",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samane", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ActionGroup",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    samaneid = table.Column<int>(nullable: false),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionGroup", x => x.id);
                    table.ForeignKey(
                        name: "FK_ActionGroup_Samane_samaneid",
                        column: x => x.samaneid,
                        principalTable: "Samane",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleid = table.Column<int>(nullable: false),
                    userid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Role_roleid",
                        column: x => x.roleid,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_User_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    actiongroupid = table.Column<int>(nullable: false),
                    title = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.id);
                    table.ForeignKey(
                        name: "FK_Action_ActionGroup_actiongroupid",
                        column: x => x.actiongroupid,
                        principalTable: "ActionGroup",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RACC",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    actionid = table.Column<int>(nullable: false),
                    roleid = table.Column<int>(nullable: false),
                    type = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RACC", x => x.id);
                    table.ForeignKey(
                        name: "FK_RACC_Action_actionid",
                        column: x => x.actionid,
                        principalTable: "Action",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RACC_Role_roleid",
                        column: x => x.roleid,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UACC",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<int>(nullable: false),
                    actionid = table.Column<int>(nullable: false),
                    type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UACC", x => x.id);
                    table.ForeignKey(
                        name: "FK_UACC_Action_actionid",
                        column: x => x.actionid,
                        principalTable: "Action",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UACC_User_userid",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_actiongroupid",
                table: "Action",
                column: "actiongroupid");

            migrationBuilder.CreateIndex(
                name: "IX_ActionGroup_samaneid",
                table: "ActionGroup",
                column: "samaneid");

            migrationBuilder.CreateIndex(
                name: "IX_RACC_actionid",
                table: "RACC",
                column: "actionid");

            migrationBuilder.CreateIndex(
                name: "IX_RACC_roleid",
                table: "RACC",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_UACC_actionid",
                table: "UACC",
                column: "actionid");

            migrationBuilder.CreateIndex(
                name: "IX_UACC_userid",
                table: "UACC",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_roleid",
                table: "UserRoles",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_userid",
                table: "UserRoles",
                column: "userid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Access");

            migrationBuilder.DropTable(
                name: "RACC");

            migrationBuilder.DropTable(
                name: "UACC");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ActionGroup");

            migrationBuilder.DropTable(
                name: "Samane");
        }
    }
}
