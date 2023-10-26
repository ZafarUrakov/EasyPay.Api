using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPay.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddImageMetadatas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageMetadatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Size = table.Column<float>(type: "real", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageMetadatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageMetadatas_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageMetadatas_ClientId",
                table: "ImageMetadatas",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageMetadatas");
        }
    }
}
