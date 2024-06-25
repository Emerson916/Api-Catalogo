using APICatalogo.Models;

namespace APICatalogo.Repository;

public interface IProdutoRepository
{
    public IQueryable<Produto> GetProdutosRepository();

    public Produto GetProdutoByIdRepository(int id);

    public Produto PostProdutoRepository(Produto data);
    public bool PutProdutoRepository(int id, Produto data);

    public Produto DeleteProdutoRepository(int id);
}