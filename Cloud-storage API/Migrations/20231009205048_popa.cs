using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_storage_API.Migrations
{
    /// <inheritdoc />
    public partial class popa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
