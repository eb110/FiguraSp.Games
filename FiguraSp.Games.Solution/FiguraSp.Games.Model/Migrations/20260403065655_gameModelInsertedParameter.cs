using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiguraSp.Games.Model.Migrations
{
    /// <inheritdoc />
    public partial class gameModelInsertedParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Inserted",
                table: "Game",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inserted",
                table: "Game");
        }
    }
}
