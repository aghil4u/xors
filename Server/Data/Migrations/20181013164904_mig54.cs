using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Data.Migrations
{
    public partial class mig54 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetDescription",
                table: "Verification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EquipmentNumber",
                table: "Verification",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetDescription",
                table: "Verification");

            migrationBuilder.DropColumn(
                name: "EquipmentNumber",
                table: "Verification");
        }
    }
}
