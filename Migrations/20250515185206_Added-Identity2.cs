using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamNest.Migrations
{
    /// <inheritdoc />
    public partial class AddedIdentity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_QuestionBank_QuestionBankQuestionId",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_QuestionBankQuestionId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "QuestionBankQuestionId",
                table: "Exams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionBankQuestionId",
                table: "Exams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_QuestionBankQuestionId",
                table: "Exams",
                column: "QuestionBankQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_QuestionBank_QuestionBankQuestionId",
                table: "Exams",
                column: "QuestionBankQuestionId",
                principalTable: "QuestionBank",
                principalColumn: "QuestionID");
        }
    }
}
