using APICatalogo.Models;

namespace APICatalogo.Repository;

public interface IProdutoRepository
{
    public IEnumerable<Produto> GetProdutosRepository();

    public Produto GetProdutoByIdRepository(int id);

    public Produto PostProdutoRepository(Produto data);
    public Produto PutProdutoRepository(int id, Produto data);

    public Produto DeleteProdutoRepository(int id);
}