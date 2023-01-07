using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Data.Migrations
{
    public partial class Final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "PurchaseReceipts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "Value",
                table: "PurchaseReceipts",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "PurchaseReceipts");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "PurchaseReceipts");
        }
    }
}
