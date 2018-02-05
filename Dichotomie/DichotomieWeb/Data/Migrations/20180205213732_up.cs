using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DichotomieWeb.Data.Migrations
{
    public partial class up : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reputations_AspNetUsers_FromUserFK",
                table: "Reputations");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropIndex(
                name: "IX_Reputations_FromUserFK",
                table: "Reputations");

            migrationBuilder.DropColumn(
                name: "FromUserFK",
                table: "Reputations");

            migrationBuilder.AddColumn<int>(
                name: "TopicFK",
                table: "Reputations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HomeNews",
                columns: table => new
                {
                    HomenewsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(maxLength: 200, nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeNews", x => x.HomenewsId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reputations_TopicFK",
                table: "Reputations",
                column: "TopicFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Reputations_Topics_TopicFK",
                table: "Reputations",
                column: "TopicFK",
                principalTable: "Topics",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reputations_Topics_TopicFK",
                table: "Reputations");

            migrationBuilder.DropTable(
                name: "HomeNews");

            migrationBuilder.DropIndex(
                name: "IX_Reputations_TopicFK",
                table: "Reputations");

            migrationBuilder.DropColumn(
                name: "TopicFK",
                table: "Reputations");

            migrationBuilder.AddColumn<string>(
                name: "FromUserFK",
                table: "Reputations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    TopicId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(maxLength: 50, nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.TopicId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reputations_FromUserFK",
                table: "Reputations",
                column: "FromUserFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Reputations_AspNetUsers_FromUserFK",
                table: "Reputations",
                column: "FromUserFK",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
