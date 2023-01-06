using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net.hempux.kabuto.Migrations
{
    public partial class NewNinjaTables5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Organizations_DeviceModelId",
                table: "Devices");

            migrationBuilder.AlterColumn<int>(
                name: "DeviceModelId",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "DeviceForeignKey",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceForeignKey",
                table: "Devices",
                column: "DeviceForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Organizations_DeviceForeignKey",
                table: "Devices",
                column: "DeviceForeignKey",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Organizations_DeviceForeignKey",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceForeignKey",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "DeviceForeignKey",
                table: "Devices");

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
    }
}
