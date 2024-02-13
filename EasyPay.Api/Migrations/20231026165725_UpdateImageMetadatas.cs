using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPay.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageMetadatas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageMetadatas_Clients_ClientId",
                table: "ImageMetadatas");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "ImageMetadatas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageMetadatas_Clients_ClientId",
                table: "ImageMetadatas",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageMetadatas_Clients_ClientId",
                table: "ImageMetadatas");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "ImageMetadatas",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageMetadatas_Clients_ClientId",
                table: "ImageMetadatas",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId");
        }
    }
}
