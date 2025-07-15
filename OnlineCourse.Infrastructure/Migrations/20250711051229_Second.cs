using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OnlineCourse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "blog_id",
                table: "comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "blogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    details = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    img_url = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_blogs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comments_blog_id",
                table: "comments",
                column: "blog_id");

            migrationBuilder.CreateIndex(
                name: "IX_blogs_user_id",
                table: "blogs",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_blogs_blog_id",
                table: "comments",
                column: "blog_id",
                principalTable: "blogs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_blogs_blog_id",
                table: "comments");

            migrationBuilder.DropTable(
                name: "blogs");

            migrationBuilder.DropIndex(
                name: "IX_comments_blog_id",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "blog_id",
                table: "comments");
        }
    }
}
