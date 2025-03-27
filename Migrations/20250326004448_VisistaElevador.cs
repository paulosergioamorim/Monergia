using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monergia.Migrations
{
    /// <inheritdoc />
    public partial class VisistaElevador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DevolucaoChave",
                table: "VisitasTecnicas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DocumentoRecebedor",
                table: "VisitasTecnicas",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EmpregoMaterial",
                table: "VisitasTecnicas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string[]>(
                name: "Imagens",
                table: "VisitasTecnicas",
                type: "text[]",
                maxLength: 500,
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.AddColumn<string>(
                name: "InformacaoTecnica",
                table: "VisitasTecnicas",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeRecebedor",
                table: "VisitasTecnicas",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TempoParalizacao",
                table: "VisitasTecnicas",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "VisitasTecnicas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VisitasElevadores",
                columns: table => new
                {
                    VisitaTecnicaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ElevadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitasElevadores", x => new { x.VisitaTecnicaId, x.ElevadorId });
                    table.ForeignKey(
                        name: "FK_VisitasElevadores_Elevadores_ElevadorId",
                        column: x => x.ElevadorId,
                        principalTable: "Elevadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitasElevadores_VisitasTecnicas_VisitaTecnicaId",
                        column: x => x.VisitaTecnicaId,
                        principalTable: "VisitasTecnicas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitasElevadores_ElevadorId",
                table: "VisitasElevadores",
                column: "ElevadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitasElevadores");

            migrationBuilder.DropColumn(
                name: "DevolucaoChave",
                table: "VisitasTecnicas");

            migrationBuilder.DropColumn(
                name: "DocumentoRecebedor",
                table: "VisitasTecnicas");

            migrationBuilder.DropColumn(
                name: "EmpregoMaterial",
                table: "VisitasTecnicas");

            migrationBuilder.DropColumn(
                name: "Imagens",
                table: "VisitasTecnicas");

            migrationBuilder.DropColumn(
                name: "InformacaoTecnica",
                table: "VisitasTecnicas");

            migrationBuilder.DropColumn(
                name: "NomeRecebedor",
                table: "VisitasTecnicas");

            migrationBuilder.DropColumn(
                name: "TempoParalizacao",
                table: "VisitasTecnicas");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "VisitasTecnicas");
        }
    }
}
