using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiguraSp.Games.Model.Migrations
{
    /// <inheritdoc />
    public partial class gameModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LevelId",
                table: "Game",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Game_LevelId",
                table: "Game",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_PicklistGameLevel_LevelId",
                table: "Game",
                column: "LevelId",
                principalTable: "PicklistGameLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_PicklistGameLevel_LevelId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_LevelId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Game");
        }
    }
}
