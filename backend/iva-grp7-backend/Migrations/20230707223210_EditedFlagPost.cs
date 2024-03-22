using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iva_grp7_backend.Migrations
{
    /// <inheritdoc />
    public partial class EditedFlagPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Edited",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edited",
                table: "Posts");
        }
    }
}
