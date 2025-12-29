using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renta.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProperPhotoRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Cars_EntityId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Events_EntityId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Yachts_EntityId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Cars_EntityId",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Events_EntityId",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Yachts_EntityId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_EntityId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_EntityId_VisualizationOrder",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Photos");

            migrationBuilder.AddColumn<Guid>(
                name: "CarId",
                table: "Videos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Videos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "YachtId",
                table: "Videos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CarId",
                table: "Photos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Photos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "YachtId",
                table: "Photos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_CarId",
                table: "Videos",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_EventId",
                table: "Videos",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_YachtId",
                table: "Videos",
                column: "YachtId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CarId",
                table: "Photos",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CarId_VisualizationOrder",
                table: "Photos",
                columns: new[] { "CarId", "VisualizationOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EventId",
                table: "Photos",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EventId_VisualizationOrder",
                table: "Photos",
                columns: new[] { "EventId", "VisualizationOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_YachtId",
                table: "Photos",
                column: "YachtId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_YachtId_VisualizationOrder",
                table: "Photos",
                columns: new[] { "YachtId", "VisualizationOrder" });

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Cars_CarId",
                table: "Photos",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Events_EventId",
                table: "Photos",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Yachts_YachtId",
                table: "Photos",
                column: "YachtId",
                principalTable: "Yachts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Cars_CarId",
                table: "Videos",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Events_EventId",
                table: "Videos",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Yachts_YachtId",
                table: "Videos",
                column: "YachtId",
                principalTable: "Yachts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Cars_CarId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Events_EventId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Yachts_YachtId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Cars_CarId",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Events_EventId",
                table: "Videos");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Yachts_YachtId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_CarId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_EventId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_YachtId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_CarId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_CarId_VisualizationOrder",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_EventId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_EventId_VisualizationOrder",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_YachtId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_YachtId_VisualizationOrder",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "YachtId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "YachtId",
                table: "Photos");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "Photos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EntityId",
                table: "Photos",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EntityId_VisualizationOrder",
                table: "Photos",
                columns: new[] { "EntityId", "VisualizationOrder" });

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Cars_EntityId",
                table: "Photos",
                column: "EntityId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Events_EntityId",
                table: "Photos",
                column: "EntityId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Yachts_EntityId",
                table: "Photos",
                column: "EntityId",
                principalTable: "Yachts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Cars_EntityId",
                table: "Videos",
                column: "EntityId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Events_EntityId",
                table: "Videos",
                column: "EntityId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Yachts_EntityId",
                table: "Videos",
                column: "EntityId",
                principalTable: "Yachts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
