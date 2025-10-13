using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rise.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSemesterToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcademicSemesterId",
                table: "Course",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Course_AcademicSemesterId",
                table: "Course",
                column: "AcademicSemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_AcademicSemester_AcademicSemesterId",
                table: "Course",
                column: "AcademicSemesterId",
                principalTable: "AcademicSemester",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_AcademicSemester_AcademicSemesterId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_AcademicSemesterId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "AcademicSemesterId",
                table: "Course");
        }
    }
}
