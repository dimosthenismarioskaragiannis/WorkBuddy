using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkBuddy.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedMail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Users");
        }
    }
}
