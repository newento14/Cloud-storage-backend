using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_storage_API.Migrations
{
    /// <inheritdoc />
    public partial class tablya : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Added",
                table: "Files",
                newName: "Date");

            migrationBuilder.AlterColumn<Guid>(
                name: "Link",
                table: "Files",
                type: "char(255)",
                maxLength: 255,
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Starred",
                table: "Files",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Starred",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Files",
                newName: "Added");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Files",
                type: "char(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }
    }
}
