using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renta.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TicketPaymentProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Tickets",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Tickets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTransactionId",
                table: "Tickets",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionId",
                table: "Tickets");
        }
    }
}
