using APICatalogo.Context;

namespace APICatalogo.Repository;

public class UnitOfWork : IUnitOfWork {
    private IProdutoRepository? _produtoRepo;
    private IcategoriaRepository? _categoriaRepo;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext context) {
        _context = context;
    }

    public IProdutoRepository ProdutoRepository {
        get {
            return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context);
        }
    }

    public IcategoriaRepository CategoriaRepository {
        get {
            return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context);
        }
    }

    public void Commit() {
        _context.SaveChanges();
    }

    public void Dispose() {
        _context.Dispose();
    }
}