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

    public IQueryable<Produto> GetProdutosRepository()
    {
        return _context.Produtos!;
    }

    public Produto GetProdutoByIdRepository(int id)
    {
        if (_context == null || _context.Produtos == null)
        {
            throw new InvalidOperationException("O contexto ou a lista de produtos está nula.");
        }
        var response = _context.Produtos.FirstOrDefault(i => i.ProdutoId == id);

        if (response == null)
        {
            throw new InvalidOperationException("A response ou a lista de produtos está nula.");
        }

        return response;
    }

    public Produto PostProdutoRepository(Produto data)
    {
        if (data is null)
        {
            throw new InvalidOperationException("Dados do produto é null");
        }

        if (_context == null || _context.Produtos == null)
        {
            throw new InvalidOperationException("O contexto esta nulo.");
        }

        _context.Produtos.Add(data);
        _context.SaveChanges();
        return data;
    }

    public bool PutProdutoRepository(int id, Produto data)
    {
        if (data is null)
            throw new ArgumentException(nameof(data));

        if (_context == null || _context.Produtos == null)
        {
            throw new InvalidOperationException("O contexto esta nulo.");
        }

        if (_context.Produtos.Any(p => p.ProdutoId == data.ProdutoId))
        {
            // _context.Entry(data).State = EntityState.Modified;
            _context.Produtos.Update(data);
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public Produto DeleteProdutoRepository(int id)
    {
        if (_context == null || _context.Produtos == null)
        {
            throw new InvalidOperationException("O contexto esta nulo.");
        }
        var produto = _context.Produtos.Find(id);

        if (produto is null)
            throw new ArgumentException(nameof(produto));

        _context.Produtos.Remove(produto);
        _context.SaveChanges();

        return produto;
    }
}
