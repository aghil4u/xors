using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Data.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EquipmentNumber = table.Column<string>(nullable: true),
                    AssetNumber = table.Column<string>(nullable: true),
                    AcquisitionDate = table.Column<DateTime>(nullable: false),
                    PendingUpdate = table.Column<bool>(nullable: false),
                    AcquisitionValue = table.Column<float>(nullable: false),
                    BookValue = table.Column<string>(nullable: true),
                    AssetDescription = table.Column<string>(nullable: true),
                    EquipmentDescription = table.Column<string>(nullable: true),
                    OperationId = table.Column<string>(nullable: true),
                    SubType = table.Column<string>(nullable: true),
                    Weight = table.Column<string>(nullable: true),
                    WeightUnit = table.Column<string>(nullable: true),
                    Dimensions = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Connection = table.Column<string>(nullable: true),
                    Length = table.Column<string>(nullable: true),
                    ModelNumber = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<string>(nullable: true),
                    AssetLocation = table.Column<string>(nullable: true),
                    AssetLocationText = table.Column<string>(nullable: true),
                    EquipmentLocation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipment");
        }
    }
}
