using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_platform.Migrations
{
    /// <inheritdoc />
    public partial class AddSubCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sub_course",
                columns: table => new
                {
                    sid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mid = table.Column<int>(type: "int", nullable: false),
                    sname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sstatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    samount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sub_course", x => x.sid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sub_course");
        }
    }
}
