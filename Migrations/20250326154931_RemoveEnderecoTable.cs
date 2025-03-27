using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monergia.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEnderecoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Condominios_Endereco_EnderecoId",
                table: "Condominios");

            migrationBuilder.DropTable(
                name: "Endereco");

            migrationBuilder.DropIndex(
                name: "IX_Condominios_EnderecoId",
                table: "Condominios");

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Condominios",
                type: "character varying(95)",
                maxLength: 95,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CEP",
                table: "Condominios",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Condominios",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Logradouro",
                table: "Condominios",
                type: "character varying(95)",
                maxLength: 95,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Municipio",
                table: "Condominios",
                type: "character varying(95)",
                maxLength: 95,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                table: "Condominios",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Condominios");

            migrationBuilder.DropColumn(
                name: "CEP",
                table: "Condominios");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Condominios");

            migrationBuilder.DropColumn(
                name: "Logradouro",
                table: "Condominios");

            migrationBuilder.DropColumn(
                name: "Municipio",
                table: "Condominios");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Condominios");

            migrationBuilder.CreateTable(
                name: "Endereco",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Bairro = table.Column<string>(type: "character varying(95)", maxLength: 95, nullable: false),
                    CEP = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Logradouro = table.Column<string>(type: "character varying(95)", maxLength: 95, nullable: false),
                    Municipio = table.Column<string>(type: "character varying(95)", maxLength: 95, nullable: false),
                    Numero = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endereco", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Condominios_EnderecoId",
                table: "Condominios",
                column: "EnderecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Condominios_Endereco_EnderecoId",
                table: "Condominios",
                column: "EnderecoId",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
