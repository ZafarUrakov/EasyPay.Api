using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPay.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "Clients");
        }
    }
}
