using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakuganApi.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionBakuganDetailDto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BakuganCategoryBakuganModel");

            migrationBuilder.AddColumn<int>(
                name: "BakuganCategoryId",
                table: "Bakugans",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Bakugans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bakugans_BakuganCategoryId",
                table: "Bakugans",
                column: "BakuganCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bakugans_BakuganCategories_BakuganCategoryId",
                table: "Bakugans",
                column: "BakuganCategoryId",
                principalTable: "BakuganCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bakugans_BakuganCategories_BakuganCategoryId",
                table: "Bakugans");

            migrationBuilder.DropIndex(
                name: "IX_Bakugans_BakuganCategoryId",
                table: "Bakugans");

            migrationBuilder.DropColumn(
                name: "BakuganCategoryId",
                table: "Bakugans");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Bakugans");

            migrationBuilder.CreateTable(
                name: "BakuganCategoryBakuganModel",
                columns: table => new
                {
                    BakugansId = table.Column<int>(type: "integer", nullable: false),
                    CategoriesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BakuganCategoryBakuganModel", x => new { x.BakugansId, x.CategoriesId });
                    table.ForeignKey(
                        name: "FK_BakuganCategoryBakuganModel_BakuganCategories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "BakuganCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BakuganCategoryBakuganModel_Bakugans_BakugansId",
                        column: x => x.BakugansId,
                        principalTable: "Bakugans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BakuganCategoryBakuganModel_CategoriesId",
                table: "BakuganCategoryBakuganModel",
                column: "CategoriesId");
        }
    }
}
