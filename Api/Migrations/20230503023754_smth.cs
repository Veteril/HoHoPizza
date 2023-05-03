using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class smth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngridientComposition_FoodSlots_FoodSlotId",
                table: "IngridientComposition");

            migrationBuilder.DropForeignKey(
                name: "FK_IngridientComposition_Ingridients_IngridientId",
                table: "IngridientComposition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngridientComposition",
                table: "IngridientComposition");

            migrationBuilder.RenameTable(
                name: "IngridientComposition",
                newName: "IngridientCompositions");

            migrationBuilder.RenameIndex(
                name: "IX_IngridientComposition_IngridientId",
                table: "IngridientCompositions",
                newName: "IX_IngridientCompositions_IngridientId");

            migrationBuilder.RenameIndex(
                name: "IX_IngridientComposition_FoodSlotId",
                table: "IngridientCompositions",
                newName: "IX_IngridientCompositions_FoodSlotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngridientCompositions",
                table: "IngridientCompositions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IngridientCompositions_FoodSlots_FoodSlotId",
                table: "IngridientCompositions",
                column: "FoodSlotId",
                principalTable: "FoodSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngridientCompositions_Ingridients_IngridientId",
                table: "IngridientCompositions",
                column: "IngridientId",
                principalTable: "Ingridients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngridientCompositions_FoodSlots_FoodSlotId",
                table: "IngridientCompositions");

            migrationBuilder.DropForeignKey(
                name: "FK_IngridientCompositions_Ingridients_IngridientId",
                table: "IngridientCompositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngridientCompositions",
                table: "IngridientCompositions");

            migrationBuilder.RenameTable(
                name: "IngridientCompositions",
                newName: "IngridientComposition");

            migrationBuilder.RenameIndex(
                name: "IX_IngridientCompositions_IngridientId",
                table: "IngridientComposition",
                newName: "IX_IngridientComposition_IngridientId");

            migrationBuilder.RenameIndex(
                name: "IX_IngridientCompositions_FoodSlotId",
                table: "IngridientComposition",
                newName: "IX_IngridientComposition_FoodSlotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngridientComposition",
                table: "IngridientComposition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IngridientComposition_FoodSlots_FoodSlotId",
                table: "IngridientComposition",
                column: "FoodSlotId",
                principalTable: "FoodSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngridientComposition_Ingridients_IngridientId",
                table: "IngridientComposition",
                column: "IngridientId",
                principalTable: "Ingridients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
