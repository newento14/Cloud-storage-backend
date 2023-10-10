using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_storage_API.Migrations
{
    /// <inheritdoc />
    public partial class fieldsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StorageSize",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "StorageUsed",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageSize",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StorageUsed",
                table: "Users");
        }
    }
}
