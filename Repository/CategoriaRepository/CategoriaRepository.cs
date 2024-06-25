using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository;

public class CategoriaRepository : Repository<Categoria>, IcategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }
}
