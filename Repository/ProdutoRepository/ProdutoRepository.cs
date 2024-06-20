using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Produto> GetProdutosRepository()
    {
        if (_context.Produtos is null) return [];

        var response = _context.Produtos.ToList();

        return response;
    }

    public Produto GetProdutoByIdRepository(int id)
    {
        var response = _context.Produtos.FirstOrDefault(i => i.ProdutoId == id);

        return response;
    }
    public Produto PostProdutoRepository(Produto data)
    {
        _context.Produtos.Add(data);
        _context.SaveChanges();
        return data;
    }

    public Produto PutProdutoRepository(int id, Produto data)
    {
        if (data is null) throw new ArgumentException(nameof(data));

        _context.Entry(data).State = EntityState.Modified;
        _context.SaveChanges();
        return data;
    }

    public Produto DeleteProdutoRepository(int id) {
        var produto = _context.Produtos.Find(id);

        if (produto is null) throw new ArgumentException(nameof(produto));

        _context.Produtos.Remove(produto);
        _context.SaveChanges();

        return produto;
    }
}