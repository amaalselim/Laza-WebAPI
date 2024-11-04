using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LazaProject.persistence.Migrations
{
    /// <inheritdoc />
    public partial class category : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
