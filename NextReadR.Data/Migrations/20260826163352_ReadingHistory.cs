using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextReadR.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReadingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadingHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<long>(type: "INTEGER", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    ISBN = table.Column<string>(type: "TEXT", nullable: false),
                    DetailedReview = table.Column<string>(type: "TEXT", nullable: false),
                    ShelfName = table.Column<int>(type: "INTEGER", nullable: false),
                    DateRead = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingHistory_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingHistory_BookId",
                table: "ReadingHistory",
                column: "BookId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReadingHistory_BookId",
                table: "ReadingHistory");

            migrationBuilder.DropTable(
                name: "ReadingHistory");
        }
    }
}
