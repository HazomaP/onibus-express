using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnibusExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Passageiros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passageiros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rotas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Origem = table.Column<string>(type: "text", nullable: true),
                    Destino = table.Column<string>(type: "text", nullable: true),
                    DuracaoEstimada = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rotas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Viagens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RotaId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHoraPartida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PrecoBase = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalAssentos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Viagens_Rotas_RotaId",
                        column: x => x.RotaId,
                        principalTable: "Rotas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoReserva = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ViagemId = table.Column<Guid>(type: "uuid", nullable: false),
                    PassageiroId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroAssento = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservas_Passageiros_PassageiroId",
                        column: x => x.PassageiroId,
                        principalTable: "Passageiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservas_Viagens_ViagemId",
                        column: x => x.ViagemId,
                        principalTable: "Viagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_CodigoReserva",
                table: "Reservas",
                column: "CodigoReserva",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_PassageiroId",
                table: "Reservas",
                column: "PassageiroId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ViagemId",
                table: "Reservas",
                column: "ViagemId");

            migrationBuilder.CreateIndex(
                name: "IX_Viagens_RotaId",
                table: "Viagens",
                column: "RotaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Passageiros");

            migrationBuilder.DropTable(
                name: "Viagens");

            migrationBuilder.DropTable(
                name: "Rotas");
        }
    }
}
