using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Topic_mid",
                table: "Topic",
                column: "mid");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_sid",
                table: "Topic",
                column: "sid");

            migrationBuilder.CreateIndex(
                name: "IX_sub_course_mid",
                table: "sub_course",
                column: "mid");

            migrationBuilder.CreateIndex(
                name: "IX_my_courses_user_id",
                table: "my_courses",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Mcq_tid",
                table: "Mcq",
                column: "tid");

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
                name: "IX_AdminSubscription_mid",
                table: "AdminSubscription",
                column: "mid");

            migrationBuilder.CreateIndex(
                name: "IX_AdminSubscription_sid",
                table: "AdminSubscription",
                column: "sid");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminSubscription_Master_course_mid",
                table: "AdminSubscription",
                column: "mid",
                principalTable: "Master_course",
                principalColumn: "mid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminSubscription_sub_course_sid",
                table: "AdminSubscription",
                column: "sid",
                principalTable: "sub_course",
                principalColumn: "sid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Master_course_mid",
                table: "Material",
                column: "mid",
                principalTable: "Master_course",
                principalColumn: "mid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Topic_tid",
                table: "Material",
                column: "tid",
                principalTable: "Topic",
                principalColumn: "tid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_sub_course_sid",
                table: "Material",
                column: "sid",
                principalTable: "sub_course",
                principalColumn: "sid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mcq_Topic_tid",
                table: "Mcq",
                column: "tid",
                principalTable: "Topic",
                principalColumn: "tid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_my_courses_sub_course_sid",
                table: "my_courses",
                column: "sid",
                principalTable: "sub_course",
                principalColumn: "sid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_my_courses_user_user_id",
                table: "my_courses",
                column: "user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sub_course_Master_course_mid",
                table: "sub_course",
                column: "mid",
                principalTable: "Master_course",
                principalColumn: "mid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Master_course_mid",
                table: "Topic",
                column: "mid",
                principalTable: "Master_course",
                principalColumn: "mid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_sub_course_sid",
                table: "Topic",
                column: "sid",
                principalTable: "sub_course",
                principalColumn: "sid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminSubscription_Master_course_mid",
                table: "AdminSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminSubscription_sub_course_sid",
                table: "AdminSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Master_course_mid",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Topic_tid",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_sub_course_sid",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Mcq_Topic_tid",
                table: "Mcq");

            migrationBuilder.DropForeignKey(
                name: "FK_my_courses_sub_course_sid",
                table: "my_courses");

            migrationBuilder.DropForeignKey(
                name: "FK_my_courses_user_user_id",
                table: "my_courses");

            migrationBuilder.DropForeignKey(
                name: "FK_sub_course_Master_course_mid",
                table: "sub_course");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Master_course_mid",
                table: "Topic");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_sub_course_sid",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_mid",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_sid",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_sub_course_mid",
                table: "sub_course");

            migrationBuilder.DropIndex(
                name: "IX_my_courses_user_id",
                table: "my_courses");

            migrationBuilder.DropIndex(
                name: "IX_Mcq_tid",
                table: "Mcq");

            migrationBuilder.DropIndex(
                name: "IX_Material_mid",
                table: "Material");

            migrationBuilder.DropIndex(
                name: "IX_Material_sid",
                table: "Material");

            migrationBuilder.DropIndex(
                name: "IX_Material_tid",
                table: "Material");

            migrationBuilder.DropIndex(
                name: "IX_AdminSubscription_mid",
                table: "AdminSubscription");

            migrationBuilder.DropIndex(
                name: "IX_AdminSubscription_sid",
                table: "AdminSubscription");
        }
    }
}
