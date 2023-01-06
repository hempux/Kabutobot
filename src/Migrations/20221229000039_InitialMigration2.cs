using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net.hempux.kabuto.Migrations
{
    public partial class InitialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Organizations_OrganizationId",
                table: "Devices");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "Devices",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Organizations_OrganizationId",
                table: "Devices",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }
    }
}
