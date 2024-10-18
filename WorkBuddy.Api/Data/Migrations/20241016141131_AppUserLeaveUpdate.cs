using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkBuddy.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AppUserLeaveUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemainingStudentLeave",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RemainingVaccationDays",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingStudentLeave",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RemainingVaccationDays",
                table: "Users");
        }
    }
}
