using Microsoft.EntityFrameworkCore.Migrations;

namespace FrontToBack.Migrations
{
    public partial class AddedPromotion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ButtonText",
                table: "Promos2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonUrl",
                table: "Promos2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title1",
                table: "Promos2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title2",
                table: "Promos2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ButtonText",
                table: "Promos2");

            migrationBuilder.DropColumn(
                name: "ButtonUrl",
                table: "Promos2");

            migrationBuilder.DropColumn(
                name: "Title1",
                table: "Promos2");

            migrationBuilder.DropColumn(
                name: "Title2",
                table: "Promos2");
        }
    }
}
