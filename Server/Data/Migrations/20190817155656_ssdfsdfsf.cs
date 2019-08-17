using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Data.Migrations
{
    public partial class ssdfsdfsf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccesibleFunctions",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccesibleLocations",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Authority",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccesibleFunctions",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccesibleLocations",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Authority",
                table: "AspNetUsers");
        }
    }
}
