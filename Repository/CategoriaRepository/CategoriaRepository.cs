using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository;

public class CategoriaRepository : IcategoriaRepository
{

    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Categoria> GetCategoriasRepository()
    {
        if (_context.Categorias is null) return [];

        var response = _context.Categorias.ToList();
        return response;
    }

    public IEnumerable<Categoria> GetCategoriasComProdutos(){ 
        if (_context.Categorias is null) return [];

        var response = _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList();
        
        return response;
    }

    public Categoria GetCategoriaByIdRepository(int id) {
        // if (_context.Categorias is null) return "";
        var response = _context.Categorias.FirstOrDefault(i => i.CategoriaId == id);

        // if (response is null) return ;
        return response;
    }

    public Categoria PostCategoriaRepository(Categoria data) {
        if (data is null ) throw new ArgumentException(nameof(data));

        _context.Categorias.Add(data);
        _context.SaveChanges();
        return data;
    }

    public Categoria PutCategoriaRepository(int id, Categoria data) {
        if (data is null) throw new ArgumentNullException(nameof(data));

        _context.Entry(data).State = EntityState.Modified;
        _context.SaveChanges();
        return data;
    }

    public Categoria DeleteCategoriaRepository(int id) {
        var categoria = _context.Categorias.Find(id);

        if (categoria is null ) throw new ArgumentException(nameof(categoria));

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();

        return categoria;
    }
}