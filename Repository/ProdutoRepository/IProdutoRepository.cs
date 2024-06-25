using APICatalogo.Models;

namespace APICatalogo.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    public IEnumerable<Produto> GetProdutosPorCategoriaRepository(int id);
}