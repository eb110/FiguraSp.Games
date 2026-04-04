using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiguraSp.Games.Model.Migrations
{
    /// <inheritdoc />
    public partial class gameDateParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "GameDate",
                table: "Game",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameDate",
                table: "Game");
        }
    }
}
