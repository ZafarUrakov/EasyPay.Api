using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPay.Api.Migrations
{
    /// <inheritdoc />
    public partial class FullTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceAccountNumber",
                table: "Transfers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceAccountNumber",
                table: "Transfers");
        }
    }
}
