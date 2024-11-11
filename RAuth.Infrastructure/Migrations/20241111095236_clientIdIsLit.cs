using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class clientIdIsLit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_ClientCredStore_ClientUser_ClientId",
                table: "ClientCredStore",
                column: "ClientId",
                principalTable: "ClientUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCredStore_ClientUser_ClientId",
                table: "ClientCredStore");
        }
    }
}
