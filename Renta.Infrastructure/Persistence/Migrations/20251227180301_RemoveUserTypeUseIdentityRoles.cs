using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Renta.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserTypeUseIdentityRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
