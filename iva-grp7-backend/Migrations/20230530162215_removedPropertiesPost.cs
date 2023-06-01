using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iva_grp7_backend.Migrations
{
    /// <inheritdoc />
    public partial class removedPropertiesPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_User_DislikedByUserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_User_LikedByUserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_User_UserId1",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_DislikedByUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DislikedByUserId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "LikedByUserId",
                table: "Posts",
                newName: "UserId3");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_LikedByUserId",
                table: "Posts",
                newName: "IX_Posts_UserId3");

            migrationBuilder.AlterColumn<string>(
                name: "UserId1",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId2",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId2",
                table: "Posts",
                column: "UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_User_UserId1",
                table: "Posts",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_User_UserId2",
                table: "Posts",
                column: "UserId2",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_User_UserId3",
                table: "Posts",
                column: "UserId3",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_User_UserId1",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_User_UserId2",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_User_UserId3",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId2",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserId2",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "UserId3",
                table: "Posts",
                newName: "LikedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_UserId3",
                table: "Posts",
                newName: "IX_Posts_LikedByUserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId1",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DislikedByUserId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_DislikedByUserId",
                table: "Posts",
                column: "DislikedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_User_DislikedByUserId",
                table: "Posts",
                column: "DislikedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_User_LikedByUserId",
                table: "Posts",
                column: "LikedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_User_UserId1",
                table: "Posts",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
