using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class AddedIngridientComposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Ingridients");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "FoodSlots",
                newName: "TotalWeight");

            migrationBuilder.CreateTable(
                name: "IngridientComposition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    FoodSlotId = table.Column<int>(type: "int", nullable: false),
                    IngridientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngridientComposition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngridientComposition_FoodSlots_FoodSlotId",
                        column: x => x.FoodSlotId,
                        principalTable: "FoodSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngridientComposition_Ingridients_IngridientId",
                        column: x => x.IngridientId,
                        principalTable: "Ingridients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngridientComposition_FoodSlotId",
                table: "IngridientComposition",
                column: "FoodSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_IngridientComposition_IngridientId",
                table: "IngridientComposition",
                column: "IngridientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngridientComposition");

            migrationBuilder.RenameColumn(
                name: "TotalWeight",
                table: "FoodSlots",
                newName: "Weight");

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Ingridients",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
