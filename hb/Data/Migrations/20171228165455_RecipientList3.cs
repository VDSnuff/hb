using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace hb.Data.Migrations
{
    public partial class RecipientList3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipientAccountId",
                table: "Recipient",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipient_RecipientAccountId",
                table: "Recipient",
                column: "RecipientAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipient_BankAccounts_RecipientAccountId",
                table: "Recipient",
                column: "RecipientAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipient_BankAccounts_RecipientAccountId",
                table: "Recipient");

            migrationBuilder.DropIndex(
                name: "IX_Recipient_RecipientAccountId",
                table: "Recipient");

            migrationBuilder.DropColumn(
                name: "RecipientAccountId",
                table: "Recipient");
        }
    }
}
