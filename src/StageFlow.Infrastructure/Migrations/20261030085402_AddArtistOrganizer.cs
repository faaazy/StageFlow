using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddArtistOrganizer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizerId",
                table: "Artists",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Artists_OrganizerId",
                table: "Artists",
                column: "OrganizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_AspNetUsers_OrganizerId",
                table: "Artists",
                column: "OrganizerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artists_AspNetUsers_OrganizerId",
                table: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_Artists_OrganizerId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "OrganizerId",
                table: "Artists");
        }
    }
}
