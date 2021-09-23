using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniNexus.Domain.Migrations
{
    public partial class UserBanned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BannedUntil",
                table: "User",
                type: "datetime2",
                nullable: true,
                comment: "The UTC time until which the user is banned. A null value will permanently ban the user.");

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether the user is banned.");

            migrationBuilder.CreateTable(
                name: "UserBanReason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    BannedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The UTC time at which the user was banned."),
                    BannedUntil = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "The UTC time until which the user is banned."),
                    Reason = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false, comment: "The reason the user was banned.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBanReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBanReason_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBanReason_UserId",
                table: "UserBanReason",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBanReason");

            migrationBuilder.DropColumn(
                name: "BannedUntil",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "User");
        }
    }
}
