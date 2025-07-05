using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReclamoPremio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    IdSubasta = table.Column<Guid>(type: "uuid", nullable: false),
                    DireccionEnvio = table.Column<string>(type: "text", nullable: false),
                    MetodoEntrega = table.Column<string>(type: "text", nullable: false),
                    FechaReclamo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReclamoPremio", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReclamoPremio_Id",
                table: "ReclamoPremio",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReclamoPremio");
        }
    }
}
