using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeltaDrive.DA.Migrations
{
    /// <inheritdoc />
    public partial class Booking_Fix_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "VehicleBookings");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "VehicleBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "VehicleBookings");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "VehicleBookings",
                type: "tinyint(1)",
                nullable: true);
        }
    }
}
