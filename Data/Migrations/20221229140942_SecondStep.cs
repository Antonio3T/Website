using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website.Data.Migrations
{
    public partial class SecondStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseReceipts",
                columns: table => new
                {
                    GameID = table.Column<int>(type: "int", nullable: false),
                    ProfileID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseReceipts", x => new { x.GameID, x.ProfileID });
                    table.ForeignKey(
                        name: "FK_PurchaseReceipts_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseReceipts_Profiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReceipts_ProfileID",
                table: "PurchaseReceipts",
                column: "ProfileID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseReceipts");
        }
    }
}
