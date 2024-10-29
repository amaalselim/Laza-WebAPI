using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LazaProject.persistence.Migrations
{
    /// <inheritdoc />
    public partial class s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
