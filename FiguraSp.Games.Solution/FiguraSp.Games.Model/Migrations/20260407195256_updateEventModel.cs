using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiguraSp.Games.Model.Migrations
{
    /// <inheritdoc />
    public partial class updateEventModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomeAway",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeAway",
                table: "Events");
        }
    }
}
