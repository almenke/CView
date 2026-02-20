using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CView.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSprintStartNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SprintStartNumber",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SprintStartNumber",
                table: "Projects");
        }
    }
}
