namespace APICatalogo.Repository;

public interface IUnitOfWork 
{
    IProdutoRepository ProdutoRepository {get;}
    IcategoriaRepository CategoriaRepository {get;}

    void Commit();
}