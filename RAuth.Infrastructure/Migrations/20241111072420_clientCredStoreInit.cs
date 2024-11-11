using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class clientCredStoreInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientCredStore",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCredStore", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "RedirectUri",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientCredStoreClientId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedirectUri", x => new { x.UserId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_RedirectUri_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RedirectUri_ClientCredStore_ClientCredStoreClientId",
                        column: x => x.ClientCredStoreClientId,
                        principalTable: "ClientCredStore",
                        principalColumn: "ClientId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RedirectUri_ClientCredStoreClientId",
                table: "RedirectUri",
                column: "ClientCredStoreClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedirectUri");

            migrationBuilder.DropTable(
                name: "ClientCredStore");
        }
    }
}
