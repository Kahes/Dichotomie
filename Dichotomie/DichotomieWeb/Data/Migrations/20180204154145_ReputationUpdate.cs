using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DichotomieWeb.Data.Migrations
{
    public partial class ReputationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Reputations");

            migrationBuilder.AddColumn<string>(
                name: "UserFK",
                table: "Reputations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reputations_UserFK",
                table: "Reputations",
                column: "UserFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Reputations_AspNetUsers_UserFK",
                table: "Reputations",
                column: "UserFK",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reputations_AspNetUsers_UserFK",
                table: "Reputations");

            migrationBuilder.DropIndex(
                name: "IX_Reputations_UserFK",
                table: "Reputations");

            migrationBuilder.DropColumn(
                name: "UserFK",
                table: "Reputations");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Reputations",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
