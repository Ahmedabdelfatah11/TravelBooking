using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTourQuestionColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Question",
                table: "TourQuestion",
                newName: "QuestionText");

            migrationBuilder.RenameColumn(
                name: "Answer",
                table: "TourQuestion",
                newName: "AnswerText");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionText",
                table: "TourQuestion",
                newName: "Question");

            migrationBuilder.RenameColumn(
                name: "AnswerText",
                table: "TourQuestion",
                newName: "Answer");
        }
    }
}
