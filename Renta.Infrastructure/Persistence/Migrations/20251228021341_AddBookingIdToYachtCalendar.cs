using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renta.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingIdToYachtCalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "YachtCalendars",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "YachtCalendars",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_YachtCalendars_BookingId",
                table: "YachtCalendars",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_YachtCalendars_YachtBookings_BookingId",
                table: "YachtCalendars",
                column: "BookingId",
                principalTable: "YachtBookings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YachtCalendars_YachtBookings_BookingId",
                table: "YachtCalendars");

            migrationBuilder.DropIndex(
                name: "IX_YachtCalendars_BookingId",
                table: "YachtCalendars");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "YachtCalendars");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "YachtCalendars");
        }
    }
}
