using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iva_grp7_backend.Migrations
{
    /// <inheritdoc />
    public partial class followingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollowers_AspNetUsers_ApplicationUserId",
                table: "UserFollowers");

            migrationBuilder.DropIndex(
                name: "IX_UserFollowers_ApplicationUserId",
                table: "UserFollowers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserFollowers");

            migrationBuilder.CreateTable(
                name: "UserFollowings",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowings", x => new { x.UserId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_UserFollowings_AspNetUsers_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFollowings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowings_FollowingId",
                table: "UserFollowings",
                column: "FollowingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFollowings");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserFollowers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowers_ApplicationUserId",
                table: "UserFollowers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollowers_AspNetUsers_ApplicationUserId",
                table: "UserFollowers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
