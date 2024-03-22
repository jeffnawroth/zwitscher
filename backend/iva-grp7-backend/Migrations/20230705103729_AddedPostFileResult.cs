using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iva_grp7_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedPostFileResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "PostFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "PostFiles");
        }
    }
}
