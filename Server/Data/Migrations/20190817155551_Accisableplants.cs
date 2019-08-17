using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Data.Migrations
{
    public partial class Accisableplants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccesiblePlants",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccesiblePlants",
                table: "AspNetUsers");
        }
    }
}
