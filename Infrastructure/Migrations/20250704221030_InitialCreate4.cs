using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResolucionReclamo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IdReclamo = table.Column<Guid>(type: "uuid", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolucionReclamo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResolucionReclamo_Reclamo_IdReclamo",
                        column: x => x.IdReclamo,
                        principalTable: "Reclamo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResolucionReclamo_Id",
                table: "ResolucionReclamo",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResolucionReclamo_IdReclamo",
                table: "ResolucionReclamo",
                column: "IdReclamo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResolucionReclamo");
        }
    }
}
