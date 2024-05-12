using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeltaDrive.DA.Migrations
{
    /// <inheritdoc />
    public partial class Booking_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleBookings_Vehicles_DriverId",
                table: "VehicleBookings");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "VehicleBookings");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "VehicleBookings",
                newName: "VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleBookings_DriverId",
                table: "VehicleBookings",
                newName: "IX_VehicleBookings_VehicleId");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "VehicleBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleBookings_Vehicles_VehicleId",
                table: "VehicleBookings",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleBookings_Vehicles_VehicleId",
                table: "VehicleBookings");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "VehicleBookings");

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "VehicleBookings",
                newName: "DriverId");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleBookings_VehicleId",
                table: "VehicleBookings",
                newName: "IX_VehicleBookings_DriverId");

            migrationBuilder.AddColumn<float>(
                name: "Distance",
                table: "VehicleBookings",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleBookings_Vehicles_DriverId",
                table: "VehicleBookings",
                column: "DriverId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
