using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RedirectUriIsList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RedirectUri_AspNetUsers_UserId",
                table: "RedirectUri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RedirectUri",
                table: "RedirectUri");

            migrationBuilder.RenameColumn(
                name: "ClientSecret",
                table: "RedirectUri",
                newName: "RedirectUrl");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RedirectUri",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RedirectUri",
                table: "RedirectUri",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RedirectUri_UserId",
                table: "RedirectUri",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RedirectUri_AspNetUsers_UserId",
                table: "RedirectUri",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RedirectUri_AspNetUsers_UserId",
                table: "RedirectUri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RedirectUri",
                table: "RedirectUri");

            migrationBuilder.DropIndex(
                name: "IX_RedirectUri_UserId",
                table: "RedirectUri");

            migrationBuilder.RenameColumn(
                name: "RedirectUrl",
                table: "RedirectUri",
                newName: "ClientSecret");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RedirectUri",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RedirectUri",
                table: "RedirectUri",
                columns: new[] { "UserId", "ClientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RedirectUri_AspNetUsers_UserId",
                table: "RedirectUri",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
