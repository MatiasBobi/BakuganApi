using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BakuganApi.Migrations
{
    /// <inheritdoc />
    public partial class InitPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BakuganCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BakuganCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bakugans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Precio = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bakugans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    TipoUsuario = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "BakuganSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Dano = table.Column<int>(type: "integer", nullable: false),
                    BakuganId = table.Column<int>(type: "integer", nullable: false),
                    SkillDano = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BakuganSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BakuganSkills_Bakugans_BakuganId",
                        column: x => x.BakuganId,
                        principalTable: "Bakugans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BakuganModelUsuario",
                columns: table => new
                {
                    BakugansId = table.Column<int>(type: "integer", nullable: false),
                    UsuariosId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BakuganModelUsuario", x => new { x.BakugansId, x.UsuariosId });
                    table.ForeignKey(
                        name: "FK_BakuganModelUsuario_Bakugans_BakugansId",
                        column: x => x.BakugansId,
                        principalTable: "Bakugans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BakuganModelUsuario_Usuarios_UsuariosId",
                        column: x => x.UsuariosId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BakuganCategoryBakuganModel_CategoriesId",
                table: "BakuganCategoryBakuganModel",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_BakuganModelUsuario_UsuariosId",
                table: "BakuganModelUsuario",
                column: "UsuariosId");

            migrationBuilder.CreateIndex(
                name: "IX_BakuganSkills_BakuganId",
                table: "BakuganSkills",
                column: "BakuganId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BakuganCategoryBakuganModel");

            migrationBuilder.DropTable(
                name: "BakuganModelUsuario");

            migrationBuilder.DropTable(
                name: "BakuganSkills");

            migrationBuilder.DropTable(
                name: "BakuganCategories");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Bakugans");
        }
    }
}
