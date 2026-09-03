using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Migrations
{
    /// <inheritdoc />
    public partial class AddMcq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mcq_Topic_tid",
                table: "Mcq");

            migrationBuilder.RenameColumn(
                name: "tid",
                table: "Mcq",
                newName: "material_id");

            migrationBuilder.RenameColumn(
                name: "msqid",
                table: "Mcq",
                newName: "mcqid");

            migrationBuilder.RenameIndex(
                name: "IX_Mcq_tid",
                table: "Mcq",
                newName: "IX_Mcq_material_id");

            migrationBuilder.AddColumn<string>(
                name: "option1",
                table: "Mcq",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "option2",
                table: "Mcq",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "option3",
                table: "Mcq",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "option4",
                table: "Mcq",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Mcq_Material_material_id",
                table: "Mcq",
                column: "material_id",
                principalTable: "Material",
                principalColumn: "material_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mcq_Material_material_id",
                table: "Mcq");

            migrationBuilder.DropColumn(
                name: "option1",
                table: "Mcq");

            migrationBuilder.DropColumn(
                name: "option2",
                table: "Mcq");

            migrationBuilder.DropColumn(
                name: "option3",
                table: "Mcq");

            migrationBuilder.DropColumn(
                name: "option4",
                table: "Mcq");

            migrationBuilder.RenameColumn(
                name: "material_id",
                table: "Mcq",
                newName: "tid");

            migrationBuilder.RenameColumn(
                name: "mcqid",
                table: "Mcq",
                newName: "msqid");

            migrationBuilder.RenameIndex(
                name: "IX_Mcq_material_id",
                table: "Mcq",
                newName: "IX_Mcq_tid");

            migrationBuilder.AddForeignKey(
                name: "FK_Mcq_Topic_tid",
                table: "Mcq",
                column: "tid",
                principalTable: "Topic",
                principalColumn: "tid",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
