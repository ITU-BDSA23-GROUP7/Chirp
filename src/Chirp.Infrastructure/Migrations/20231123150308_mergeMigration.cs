using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.EFCoreDatabase.Migrations
{
    /// <inheritdoc />
    public partial class mergeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorAuthor",
                columns: table => new
                {
                    FollowersAuthorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FollowingAuthorId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorAuthor", x => new { x.FollowersAuthorId, x.FollowingAuthorId });
                    table.ForeignKey(
                        name: "FK_AuthorAuthor_Authors_FollowersAuthorId",
                        column: x => x.FollowersAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorAuthor_Authors_FollowingAuthorId",
                        column: x => x.FollowingAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorAuthor_FollowingAuthorId",
                table: "AuthorAuthor",
                column: "FollowingAuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorAuthor");
        }
    }
}
