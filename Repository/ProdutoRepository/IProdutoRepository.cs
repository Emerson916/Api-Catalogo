using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
    public IEnumerable<Produto> GetProdutosPorCategoriaRepository(int id);
}