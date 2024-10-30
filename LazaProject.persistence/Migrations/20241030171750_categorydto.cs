using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LazaProject.persistence.Migrations
{
    /// <inheritdoc />
    public partial class categorydto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "categories");

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "categories");

            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
