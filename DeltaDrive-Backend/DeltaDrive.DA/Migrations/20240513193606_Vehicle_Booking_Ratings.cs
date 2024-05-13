using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeltaDrive.DA.Migrations
{
    /// <inheritdoc />
    public partial class Vehicle_Booking_Ratings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "VehicleBookings");

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "VehicleBookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VehicleBookingRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleBookingRatings", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleBookings_RatingId",
                table: "VehicleBookings",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleBookings_VehicleBookingRatings_RatingId",
                table: "VehicleBookings",
                column: "RatingId",
                principalTable: "VehicleBookingRatings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleBookings_VehicleBookingRatings_RatingId",
                table: "VehicleBookings");

            migrationBuilder.DropTable(
                name: "VehicleBookingRatings");

            migrationBuilder.DropIndex(
                name: "IX_VehicleBookings_RatingId",
                table: "VehicleBookings");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "VehicleBookings");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "VehicleBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
