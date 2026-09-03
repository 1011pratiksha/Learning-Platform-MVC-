using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_platform.Migrations
{
    
    public partial class AddMasterCourse : Migration
    {
      
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MasterCourseId",
                table: "sub_course",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Master_course",
                columns: table => new
                {
                    mid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mstatus = table.Column<bool>(type: "bit", nullable: false),
                    mthumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_course", x => x.mid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sub_course_MasterCourseId",
                table: "sub_course",
                column: "MasterCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_sub_course_Master_course_MasterCourseId",
                table: "sub_course",
                column: "MasterCourseId",
                principalTable: "Master_course",
                principalColumn: "mid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sub_course_Master_course_MasterCourseId",
                table: "sub_course");

            migrationBuilder.DropTable(
                name: "Master_course");

            migrationBuilder.DropIndex(
                name: "IX_sub_course_MasterCourseId",
                table: "sub_course");

            migrationBuilder.DropColumn(
                name: "MasterCourseId",
                table: "sub_course");
        }
    }
}
