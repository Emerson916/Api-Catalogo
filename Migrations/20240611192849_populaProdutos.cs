using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class populaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO \"Produtos\" (\"Nome\", \"Descricao\", \"Preco\", \"ImageUrl\", \"Estoque\", \"DataCadastro\", \"CategoriaId\") VALUES ('Coca-cola', 'Refrigerante de Cola 350 ml', 5.45, 'coca.jpg', 50, now(), 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM \"Produtos\"");
        }
    }
}
