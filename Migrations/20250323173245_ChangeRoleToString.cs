using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monergia.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRoleToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Usuarios",
                type: "character varying(95)",
                maxLength: 95,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Usuarios",
                type: "character varying(95)",
                maxLength: 95,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Usuarios",
                type: "character varying(95)",
                maxLength: 95,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(95)",
                oldMaxLength: 95);

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Usuarios",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(95)",
                oldMaxLength: 95);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(95)",
                oldMaxLength: 95);
        }
    }
}
