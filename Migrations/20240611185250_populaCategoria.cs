using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class populaCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO \"Categorias\" (\"Nome\", \"ImageUrl\") VALUES ('Bebidas', 'bebidas.jpg')");
            mb.Sql("INSERT INTO \"Categorias\" (\"Nome\", \"ImageUrl\") VALUES ('Lanches', 'lanches.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM \"Categorias\"");
        }
    }
}
