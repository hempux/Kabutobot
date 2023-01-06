using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net.hempux.kabuto.Migrations
{
    public partial class Relationshipfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId1",
                table: "Devices",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_OrganizationId1",
                table: "Devices",
                column: "OrganizationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Organizations_OrganizationId1",
                table: "Devices",
                column: "OrganizationId1",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Organizations_OrganizationId1",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_OrganizationId1",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "OrganizationId1",
                table: "Devices");
        }
    }
}
