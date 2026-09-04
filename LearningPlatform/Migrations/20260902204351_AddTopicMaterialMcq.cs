using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddTopicMaterialMcq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    tid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mid = table.Column<int>(type: "int", nullable: false),
                    sid = table.Column<int>(type: "int", nullable: false),
                    tname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    videoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tstatus = table.Column<bool>(type: "bit", nullable: false),
                    tthumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.tid);
                    table.ForeignKey(
                        name: "FK_Topic_Master_course_mid",
                        column: x => x.mid,
                        principalTable: "Master_course",
                        principalColumn: "mid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Topic_sub_course_sid",
                        column: x => x.sid,
                        principalTable: "sub_course",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    material_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mid = table.Column<int>(type: "int", nullable: false),
                    sid = table.Column<int>(type: "int", nullable: false),
                    tid = table.Column<int>(type: "int", nullable: false),
                    assignment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.material_id);
                    table.ForeignKey(
                        name: "FK_Material_Master_course_mid",
                        column: x => x.mid,
                        principalTable: "Master_course",
                        principalColumn: "mid",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Material_Topic_tid",
                        column: x => x.tid,
                        principalTable: "Topic",
                        principalColumn: "tid",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Material_sub_course_sid",
                        column: x => x.sid,
                        principalTable: "sub_course",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Mcq",
                columns: table => new
                {
                    mcqid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tid = table.Column<int>(type: "int", nullable: false),
                    question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mcq", x => x.mcqid);
                    table.ForeignKey(
                        name: "FK_Mcq_Topic_tid",
                        column: x => x.tid,
                        principalTable: "Topic",
                        principalColumn: "tid",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Material_mid",
                table: "Material",
                column: "mid");

            migrationBuilder.CreateIndex(
                name: "IX_Material_sid",
                table: "Material",
                column: "sid");

            migrationBuilder.CreateIndex(
                name: "IX_Material_tid",
                table: "Material",
                column: "tid");

            migrationBuilder.CreateIndex(
                name: "IX_Mcq_tid",
                table: "Mcq",
                column: "tid");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_mid",
                table: "Topic",
                column: "mid");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_sid",
                table: "Topic",
                column: "sid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Mcq");

            migrationBuilder.DropTable(
                name: "Topic");
        }
    }
}
