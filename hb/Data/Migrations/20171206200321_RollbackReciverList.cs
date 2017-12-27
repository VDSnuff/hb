using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace hb.Data.Migrations
{
    public partial class RollbackReciverList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Reciver_ReciverListId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Reciver");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReciverListId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReciverListId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReciverListId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Reciver",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reciver", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ReciverListId",
                table: "AspNetUsers",
                column: "ReciverListId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Reciver_ReciverListId",
                table: "AspNetUsers",
                column: "ReciverListId",
                principalTable: "Reciver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
