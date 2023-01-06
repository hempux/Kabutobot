using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net.hempux.kabuto.Migrations
{
    public partial class Relationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Devices_OrganizationId",
                table: "Devices",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Organizations_OrganizationId",
                table: "Devices",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Organizations_OrganizationId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_OrganizationId",
                table: "Devices");
        }
    }
}
