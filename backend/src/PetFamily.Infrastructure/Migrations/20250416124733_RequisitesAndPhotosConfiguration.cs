using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RequisitesAndPhotosConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photos_list",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "requisite_list",
                table: "pets");

            migrationBuilder.AddColumn<string>(
                name: "photos",
                table: "pets",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "requisites",
                table: "pets",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photos",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "requisites",
                table: "pets");

            migrationBuilder.AddColumn<string>(
                name: "photos_list",
                table: "pets",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "requisite_list",
                table: "pets",
                type: "jsonb",
                nullable: true);
        }
    }
}
