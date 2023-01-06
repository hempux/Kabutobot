using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace net.hempux.kabuto.Migrations
{
    public partial class NewNinjaTables2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "approvalStatus",
                table: "Devices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "dnsName",
                table: "Devices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "organizationName",
                table: "Devices",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "approvalStatus",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "dnsName",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "organizationName",
                table: "Devices");
        }
    }
}
