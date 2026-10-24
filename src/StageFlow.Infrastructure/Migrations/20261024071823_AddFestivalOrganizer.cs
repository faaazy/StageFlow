using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFestivalOrganizer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizerId",
                table: "Festivals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Festivals_OrganizerId",
                table: "Festivals",
                column: "OrganizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Festivals_AspNetUsers_OrganizerId",
                table: "Festivals",
                column: "OrganizerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Festivals_AspNetUsers_OrganizerId",
                table: "Festivals");

            migrationBuilder.DropIndex(
                name: "IX_Festivals_OrganizerId",
                table: "Festivals");

            migrationBuilder.DropColumn(
                name: "OrganizerId",
                table: "Festivals");
        }
    }
}
