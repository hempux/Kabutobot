using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net.hempux.kabuto.Migrations
{
    public partial class NewNinjaTables4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Organizations_OrganizationId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_OrganizationId",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Devices",
                newName: "DeviceModelId");

            migrationBuilder.AlterColumn<int>(
                name: "DeviceModelId",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Organizations_DeviceModelId",
                table: "Devices",
                column: "DeviceModelId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Organizations_DeviceModelId",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "DeviceModelId",
                table: "Devices",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

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
    }
}
