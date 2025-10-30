using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CinemaApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atores",
                columns: table => new
                {
                    AtorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nacionalidade = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atores", x => x.AtorId);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Produtoras",
                columns: table => new
                {
                    ProdutoraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaisOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtoras", x => x.ProdutoraId);
                });

            migrationBuilder.CreateTable(
                name: "Filmes",
                columns: table => new
                {
                    FilmeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnoLancamento = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    ProdutoraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filmes", x => x.FilmeId);
                    table.ForeignKey(
                        name: "FK_Filmes_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Filmes_Produtoras_ProdutoraId",
                        column: x => x.ProdutoraId,
                        principalTable: "Produtoras",
                        principalColumn: "ProdutoraId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmesAtores",
                columns: table => new
                {
                    FilmeId = table.Column<int>(type: "int", nullable: false),
                    AtorId = table.Column<int>(type: "int", nullable: false),
                    Personagem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmesAtores", x => new { x.FilmeId, x.AtorId });
                    table.ForeignKey(
                        name: "FK_FilmesAtores_Atores_AtorId",
                        column: x => x.AtorId,
                        principalTable: "Atores",
                        principalColumn: "AtorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmesAtores_Filmes_FilmeId",
                        column: x => x.FilmeId,
                        principalTable: "Filmes",
                        principalColumn: "FilmeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Atores",
                columns: new[] { "AtorId", "Nacionalidade", "Nome" },
                values: new object[,]
                {
                    { 1, "Americano", "Tom Hanks" },
                    { 2, "Americana", "Meryl Streep" },
                    { 3, "Americano", "Leonardo DiCaprio" },
                    { 4, "Brasileira", "Fernanda Montenegro" }
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "CategoriaId", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, "Filmes de ação e aventura", "Ação" },
                    { 2, "Filmes dramáticos", "Drama" },
                    { 3, "Filmes cômicos", "Comédia" },
                    { 4, "Filmes de ficção científica", "Ficção Científica" }
                });

            migrationBuilder.InsertData(
                table: "Produtoras",
                columns: new[] { "ProdutoraId", "Nome", "PaisOrigem" },
                values: new object[,]
                {
                    { 1, "Warner Bros", "EUA" },
                    { 2, "Universal Pictures", "EUA" },
                    { 3, "Paramount", "EUA" },
                    { 4, "Globo Filmes", "Brasil" }
                });

            migrationBuilder.InsertData(
                table: "Filmes",
                columns: new[] { "FilmeId", "AnoLancamento", "CategoriaId", "ProdutoraId", "Titulo" },
                values: new object[,]
                {
                    { 1, 1994, 2, 1, "Forrest Gump" },
                    { 2, 1999, 1, 1, "Matrix" },
                    { 3, 1997, 2, 2, "Titanic" },
                    { 4, 1972, 2, 3, "O Poderoso Chefão" },
                    { 5, 1998, 2, 4, "Central do Brasil" }
                });

            migrationBuilder.InsertData(
                table: "FilmesAtores",
                columns: new[] { "AtorId", "FilmeId", "Personagem" },
                values: new object[,]
                {
                    { 1, 1, "Forrest Gump" },
                    { 1, 2, "Personagem em Matrix" },
                    { 3, 3, "Jack Dawson" },
                    { 4, 5, "Dora" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Filmes_CategoriaId",
                table: "Filmes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Filmes_ProdutoraId",
                table: "Filmes",
                column: "ProdutoraId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmesAtores_AtorId",
                table: "FilmesAtores",
                column: "AtorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmesAtores");

            migrationBuilder.DropTable(
                name: "Atores");

            migrationBuilder.DropTable(
                name: "Filmes");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Produtoras");
        }
    }
}
