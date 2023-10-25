using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyPay.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_AccountsAccountId",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "AccountsAccountId",
                table: "Transfers",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_AccountsAccountId",
                table: "Transfers",
                newName: "IX_Transfers_AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_AccountId",
                table: "Transfers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_AccountId",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Transfers",
                newName: "AccountsAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_AccountId",
                table: "Transfers",
                newName: "IX_Transfers_AccountsAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_AccountsAccountId",
                table: "Transfers",
                column: "AccountsAccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId");
        }
    }
}
